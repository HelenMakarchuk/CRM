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
                    users = users.Select(u => { u.FullName = (u.FullName ?? ""); return u; }).ToList();

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

        async void Save_Clicked(object sender, EventArgs e)
        {
        }
    }
}