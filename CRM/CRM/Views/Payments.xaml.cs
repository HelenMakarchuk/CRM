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
    public partial class Payments : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public Payments()
        {
            InitializeComponent();

            if (App.IsUserLoggedIn)
            {
                messageLabel.IsVisible = false;
                SetPaymentsListView(); //Xamarin doesn't support generic classes
            }
            else
            {
                messageLabel.IsVisible = true;
                PaymentsListView.ItemsSource = new List<string>();
            }
        }

        protected async void SetPaymentsListView()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}"),
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
                    List<Payment> payments = JsonConvert.DeserializeObject<List<Payment>>(json);
                    PaymentsListView.ItemsSource = payments.Select(payment => payment.Status).ToList();
                }
                catch (Exception)
                {
                    PaymentsListView.ItemsSource = new List<string>();
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
