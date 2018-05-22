using CRM.Data;
using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.CustomerView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	  public partial class EditableCustomerPage : ContentPage
	  {
        Customer CurrentCustomer { get; set; }

        public EditableCustomerPage()
        {
            InitializeComponent();
        }

        public EditableCustomerPage(Customer customer)
        {
            InitializeComponent();

            CurrentCustomer = customer;

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            NameEntry.Text = customer.Name;
            PhoneEntry.Text = customer.Phone;
            EmailEntry.Text = customer.Email;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region Checks

                if (NameEntry.Text == string.Empty || NameEntry.Text == null)
                {
                    await DisplayAlert("Create operation", "Name must be set", "OK");
                    return;
                }

                #endregion

                #region Updated customer assembling

                Customer customer = new Customer();
                customer.Id = CurrentCustomer.Id;
                customer.Name = NameEntry.Text;
                customer.Phone = PhoneEntry.Text;
                customer.Email = EmailEntry.Text;

                #endregion

                var json = JsonConvert.SerializeObject(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Customer.PluralDbTableName}/{CurrentCustomer.Id}");
                var client = new HttpClient();

                HttpResponseMessage response = await client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Update operation", "Customer was updated", "OK");

                    if (Navigation.NavigationStack.Count > 0)
                    {
                        int currentPageIndex = Navigation.NavigationStack.Count - 1;
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                        await Navigation.PushAsync(new CustomerPage(customer));
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                    }
                }
                else
                {
                    await DisplayAlert("Update operation", $"Customer wasn't updated. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Update operation", $"Customer wasn't updated. {ex.Message}", "OK");
            }
        }
    }
}