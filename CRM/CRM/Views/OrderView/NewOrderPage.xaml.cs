using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CRM.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Text;
using CRM.Data;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewOrderPage : ContentPage
    {
        public NewOrderPage()
        {
            InitializeComponent();

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            FillDeliveryDriverPicker();
            FillReceiverPicker();

            BindingContext = this;
        }

        protected async void FillDeliveryDriverPicker()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}"),
                Method = HttpMethod.Get,
                Headers = { { "Accept", "application/json" } }
            };

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                string json = await content.ReadAsStringAsync();

                try
                {
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                    users = users.Where(u => u.Position == "Delivery Driver").Select(u => { u.FullName = (u.FullName ?? ""); return u; }).ToList();

                    //Add empty value opportunity
                    users.Insert(0, new User() { FullName = "Empty value" });

                    DeliveryDriverPicker.ItemsSource = users;
                }
                catch (Exception)
                {
                    DeliveryDriverPicker.ItemsSource = new List<User>();
                }
            }
        }

        protected async void FillReceiverPicker()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Customer.PluralDbTableName}"),
                Method = HttpMethod.Get,
                Headers = { { "Accept", "application/json" } }
            };

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                string json = await content.ReadAsStringAsync();

                try
                {
                    List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(json);
                    customers = customers.Select(c => { c.Name = (c.Name ?? ""); return c; }).ToList();

                    //Add empty value opportunity
                    customers.Insert(0, new Customer() { Name = "Empty value" });

                    ReceiverPicker.ItemsSource = customers;
                }
                catch (Exception)
                {
                    ReceiverPicker.ItemsSource = new List<Customer>();
                }
            }
        }

        async void SetNumber(Order order, int id)
        {
            try
            {
                order.Id = id;
                order.Number = $"ORD_{id + 1}";

                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Order.PluralDbTableName}/{id}");
                var client = new HttpClient();

                HttpResponseMessage response = await client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Create operation", $"Order \"{order.Number}\" was created.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Set number operation", $"Order wasn't updated. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Set number operation", $"Order wasn't updated. {ex.Message}", "OK");
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region Checks

                if (SumEntry.Text == string.Empty || SumEntry.Text == null)
                {
                    await DisplayAlert("Create operation", "Sum must be set", "OK");
                    return;
                }
                else if (!(Decimal.TryParse(SumEntry.Text.Replace(".", ","), out var i)))
                {
                    await DisplayAlert("Create operation", "Sum: incorrect value", "OK");
                    return;
                }

                #endregion

                #region New order assembling

                Order order = new Order();
                order.CreatedOn = DateTime.Now;
                order.ModifiedOn = DateTime.Now;
                order.DeliveryDate = DeliveryDatePicker.Date;
                order.OwnerId = App.CurrentUserId;
                order.DeliveryStatus = (byte)OrderPickerData.DeliveryStatus.NotAssigned;
                order.PaymentStatus = (byte)OrderPickerData.PaymentStatus.Unpaid;
                order.Sum = Decimal.Parse(SumEntry.Text.Replace(".", ","));

                if (DeliveryAddressEntry.Text != string.Empty)
                    order.DeliveryAddress = DeliveryAddressEntry.Text;

                if (CommentEntry.Text != string.Empty)
                    order.Comment = CommentEntry.Text;

                //if (DeliveryDriverPicker.SelectedIndex == 0) { user want to leave delivery driver empty (null) } 
                if (DeliveryDriverPicker.SelectedIndex != 0 && DeliveryDriverPicker.SelectedItem is User selectedDeliveryDriver)
                {
                    order.DeliveryDriverId = selectedDeliveryDriver.Id;
                    order.DeliveryStatus = (byte)OrderPickerData.DeliveryStatus.Assigned;
                }

                //if (ReceiverPicker.SelectedIndex == 0) { user want to leave receiver empty (null) } 
                if (ReceiverPicker.SelectedIndex != 0 && ReceiverPicker.SelectedItem is Customer selectedReceiver)
                {
                    order.ReceiverId = selectedReceiver.Id;
                }

                #endregion

                string json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json);

                var client = new HttpClient();
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{Constants.WebAPIUrl}/api/{Order.PluralDbTableName}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    SetNumber(order, int.Parse(responseJson));
                }
                else
                {
                    await DisplayAlert("Create operation", $"Order wasn't created. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Create operation", $"An error occured while creating order. {ex.Message}", "OK");
            }
        }
    }
}