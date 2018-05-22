using CRM.Data;
using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.CustomerView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCustomerPage : ContentPage
    {
        public NewCustomerPage()
        {
            InitializeComponent();

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region New customer assembling

                Customer customer = new Customer();

                if (NameEntry.Text != string.Empty)
                    customer.Name = NameEntry.Text;

                if (PhoneEntry.Text != string.Empty)
                    customer.Phone = PhoneEntry.Text;

                if (EmailEntry.Text != string.Empty)
                    customer.Email = EmailEntry.Text;

                #endregion

                string json = JsonConvert.SerializeObject(customer);
                var content = new StringContent(json);

                var client = new HttpClient();
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{Constants.WebAPIUrl}/api/{Customer.PluralDbTableName}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //var newCustomerUri = response.Headers.Location;

                    await DisplayAlert("Create operation", $"Customer \"{NameEntry.Text}\" was created.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Create operation", $"Customer \"{NameEntry.Text}\" wasn't created. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Create operation", $"An error occured while creating customer \"{NameEntry.Text}\". {ex.Message}", "OK");
            }
        }
    }
}