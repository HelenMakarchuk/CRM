using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

using CRM.Models;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Customers : ContentPage
    {
        public Customers()
        {
            InitializeComponent();

            if (App.IsUserLoggedIn)
            {
                messageLabel.IsVisible = false;
                SetCustomersListView(); //Xamarin doesn't support generic classes
            }
            else
            {
                messageLabel.IsVisible = true;
                CustomersListView.ItemsSource = new List<string>();
            }
        }

        protected async void SetCustomersListView()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Customer.PluralDbTableName}"),
                Method = HttpMethod.Get,
                Headers = { { "Accept", "application/json" } }
            };

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                HttpContent content = response.Content;
                string json = await content.ReadAsStringAsync();

                try
                {
                    List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(json);
                    CustomersListView.ItemsSource = customers.Select(customer => customer.Name).ToList();
                }
                catch (Exception)
                {
                    CustomersListView.ItemsSource = new List<string>();
                }
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
