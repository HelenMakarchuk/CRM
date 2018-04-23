using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public EditableOrderPage()
        {
            InitializeComponent();
        }

        public EditableOrderPage(Order order)
        {
            InitializeComponent();

            CurrentOrder = order;

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            StatusEntry.Text = CurrentOrder.Status;
            DeliveryAddressEntry.Text = CurrentOrder.DeliveryAddress;
            CommentEntry.Text = CurrentOrder.Comment;

            DeliveryDatePicker.Date = order.DeliveryDate ?? DateTime.MinValue;

            SetOrderOwner();
            SetOrderDeliveryDriver();
            SetOrderReceiver();
        }

        protected async void SetOrderOwner()
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
                    users = users.Select(u => { u.FullName = (u.FullName ?? ""); return u; }).ToList();

                    //Add empty value opportunity
                    users.Insert(0, new User() { FullName = "Empty value" });

                    OwnerPicker.ItemsSource = users;

                    var selected = OwnerPicker.SelectedItem = users.Where(u => u.Id == CurrentOrder.OwnerId).FirstOrDefault();
                    if (selected != null && ((User)selected).FullName == String.Empty)
                        OwnerPicker.Title = "";
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                    OwnerPicker.ItemsSource = new List<User>();
                }
            }
            else
            {
                await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                OwnerPicker.ItemsSource = new List<User>();
            }
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
                    users = users.Select(u => { u.FullName = (u.FullName ?? ""); return u; }).ToList();

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

        async void Save_Clicked(object sender, EventArgs e)
        {
        }
    }
}