using CRM.Models;
using CRM.Views.CustomerView;
using System;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
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

            NameEntry.Text = CurrentCustomer.Name;
            PhoneEntry.Text = CurrentCustomer.Phone;
            EmailEntry.Text = CurrentCustomer.Email;
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

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Are you sure?", "An item will be deleted", "Yes", "No");

            if (userResponse)
            {
                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Customer.PluralDbTableName}/{CurrentCustomer.Id}");

                var client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await DisplayAlert("Delete operation", "Customer was deleted", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Delete operation", $"Customer wasn't deleted. Response status: {response.StatusCode}", "OK");
                }
            }
        }
    }
}