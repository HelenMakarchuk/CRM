using CRM.Data;
using CRM.Models;
using CRM.Models.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.OrderView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditableOrderPage : ContentPage
	{
        Order CurrentOrder { get; set; }
        OrderDeliveryStatusConverter orderStatusConverter = new OrderDeliveryStatusConverter();
        SumConverter sumConverter = new SumConverter();

        public EditableOrderPage()
        {
            InitializeComponent();
        }

        public EditableOrderPage(Order order)
        {
            InitializeComponent();

            CurrentOrder = order;

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            List<string> deliveryStatuses = new List<string>();
            foreach (OrderPickerData.DeliveryStatus status in Enum.GetValues(typeof(OrderPickerData.DeliveryStatus)))
            {
                var field = status.GetType().GetField(status.ToString());
                var attr = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var result = attr.Length == 0 ? status.ToString() : (attr[0] as DescriptionAttribute).Description;

                deliveryStatuses.Add(result);
            }

            DeliveryStatusPicker.ItemsSource = deliveryStatuses;
            DeliveryStatusPicker.SelectedItem = orderStatusConverter.Convert(CurrentOrder.DeliveryStatus);

            DeliveryAddressEntry.Text = CurrentOrder.DeliveryAddress;
            CommentEntry.Text = CurrentOrder.Comment;
            SumEntry.Text = sumConverter.Convert(CurrentOrder.Sum).ToString();

            DeliveryDatePicker.Date = order.DeliveryDate ?? DateTime.MinValue;

            SetOrderDeliveryDriver();
            SetOrderReceiver();
        }

        protected async void SetOrderDeliveryDriver()
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

                    var selected = DeliveryDriverPicker.SelectedItem = users.Where(u => u.Id == CurrentOrder.DeliveryDriverId).FirstOrDefault();
                    if (selected != null && ((User)selected).FullName == String.Empty)
                        DeliveryDriverPicker.Title = "";
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                    DeliveryDriverPicker.ItemsSource = new List<User>();
                }
            }
            else
            {
                await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                DeliveryDriverPicker.ItemsSource = new List<User>();
            }
        }

        protected async void SetOrderReceiver()
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

                    var selected = ReceiverPicker.SelectedItem = customers.Where(c => c.Id == CurrentOrder.ReceiverId).FirstOrDefault();
                    if (selected != null && ((Customer)selected).Name == String.Empty)
                        ReceiverPicker.Title = "";
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                    ReceiverPicker.ItemsSource = new List<Customer>();
                }
            }
            else
            {
                await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                ReceiverPicker.ItemsSource = new List<Customer>();
            }
        }

        async void UpdateOrder(Order order)
        {
            try
            {
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Order.PluralDbTableName}/{CurrentOrder.Id}");
                var client = new HttpClient();

                HttpResponseMessage response = await client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    if (Navigation.NavigationStack.Count > 0)
                    {
                        int currentPageIndex = Navigation.NavigationStack.Count - 1;
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                        await Navigation.PushAsync(new OrderPage(order));
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                    }
                }
                else
                {
                    await DisplayAlert("Update operation", $"Order wasn't updated. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Update operation", $"Order wasn't updated. {ex.Message}", "OK");
            }
        }

        async void SetPaymentStatus(Order order)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}/$OrderId={CurrentOrder.Id}/$sum"),
                    Method = HttpMethod.Get,
                    Headers = { { "Accept", "application/json" } }
                };

                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();

                    Decimal paymentSum = JsonConvert.DeserializeObject<Decimal>(json);

                    if (paymentSum >= order.Sum)
                        order.PaymentStatus = (byte)OrderPickerData.PaymentStatus.Paid;
                    else
                        order.PaymentStatus = (byte)OrderPickerData.PaymentStatus.Unpaid;

                    UpdateOrder(order);
                }
                else
                {
                    await DisplayAlert("Payment status setting operation", $"Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Payment status setting operation", $"An error occured while payment status setting. {ex.Message}", "OK");
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region Checks

                if (SumEntry.Text == string.Empty || SumEntry.Text == null)
                {
                    await DisplayAlert("Update operation", "Sum must be set", "OK");
                    return;
                }
                else if (!sumConverter.TryConvertBack(SumEntry.Text))
                {
                    await DisplayAlert("Update operation", "Sum: incorrect value", "OK");
                    return;
                }

                #endregion

                #region Updated order assembling

                Order order = CurrentOrder;
                order.ModifiedOn = DateTime.Now;
                order.DeliveryDate = DeliveryDatePicker.Date;
                order.DeliveryStatus = (byte)orderStatusConverter.ConvertBack(DeliveryStatusPicker.SelectedItem);
                order.DeliveryAddress = DeliveryAddressEntry.Text;
                order.Comment = CommentEntry.Text;

                //if (DeliveryDriverPicker.SelectedIndex == 0) { user want to set delivery driver empty (null) } 
                if (DeliveryDriverPicker.SelectedIndex != 0 && DeliveryDriverPicker.SelectedItem is User selectedDeliveryDriver)
                {
                    order.DeliveryDriverId = selectedDeliveryDriver.Id;
                }
                else
                {
                    order.DeliveryDriverId = null;
                }

                //if (ReceiverPicker.SelectedIndex == 0) { user want to set receiver empty (null) } 
                if (ReceiverPicker.SelectedIndex != 0 && ReceiverPicker.SelectedItem is Customer selectedReceiver)
                {
                    order.ReceiverId = selectedReceiver.Id;
                }
                else
                {
                    order.ReceiverId = null;
                }

                order.Sum = (Decimal)sumConverter.ConvertBack(SumEntry.Text);

                //if updated sum doesn't equal previous sum
                if (order.Sum != CurrentOrder.Sum)
                    SetPaymentStatus(order);
                else
                    UpdateOrder(order);

                #endregion
            }
            catch (Exception ex)
            {
                await DisplayAlert("Updated order assembling operation", $"{ex.Message}", "OK");
            }
        }
    }
}