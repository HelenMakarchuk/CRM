using CRM.Data;
using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.CustomerView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerPage : ContentPage
    {
        Customer CurrentCustomer { get; set; }

        public CustomerPage()
        {
            InitializeComponent();
        }

        public CustomerPage(Customer customer)
        {
            InitializeComponent();

            CurrentCustomer = customer;

            EditToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/data_edit.png" : "data_edit.png";
            DeleteToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/garbage_closed.png" : "garbage_closed.png";

            if (CurrentCustomer.Name != String.Empty && CurrentCustomer.Name != null)
            {
                NameLabel.Text += CurrentCustomer.Name;
                NameLabel.IsVisible = true;
            }

            if (CurrentCustomer.Phone != String.Empty && CurrentCustomer.Phone != null)
            {
                PhoneLabel.Text += CurrentCustomer.Phone;
                PhoneLabel.IsVisible = true;
            }

            if (CurrentCustomer.Email != String.Empty && CurrentCustomer.Email != null)
            {
                EmailLabel.Text += CurrentCustomer.Email;
                EmailLabel.IsVisible = true;
            }
        }

        void Edit_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new EditableCustomerPage(CurrentCustomer));
            }
            catch (Exception ex)
            {
                DisplayAlert("Update operation", $"Customer wasn't updated. {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Current customer deleting
        /// </summary>
        async void Delete()
        {
            try
            {
                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Customer.PluralDbTableName}/{CurrentCustomer.Id}");

                var client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Delete operation", $"Customer wasn't deleted. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Delete operation", $"Customer wasn't deleted. {ex.Message}", "OK");
            }
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Are you sure?", "An item will be deleted", "Yes", "No");

            if (userResponse)
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Order.PluralDbTableName}/$ReceiverId={CurrentCustomer.Id}"),
                        Method = HttpMethod.Get,
                        Headers = { { "Accept", "application/json" } }
                    };

                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContent content = response.Content;
                        string json = await content.ReadAsStringAsync();

                        List<string> orderNumbers = JsonConvert.DeserializeObject<List<string>>(json);

                        if (orderNumbers.Count > 0)
                        {
                            await DisplayAlert("Deleting is not allowed", $"The customer is linked to orders: {String.Join(", ", orderNumbers)}", "OK");
                            return;
                        }

                        Delete();
                    }
                    else
                    {
                        await DisplayAlert("Retrieve related orders operation", $"Response status: {response.StatusCode}", "OK");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Retrieve related orders operation", $"An error occured while retrieving related orders. {ex.Message}", "OK");
                    return;
                }
            }
        }
    }
}