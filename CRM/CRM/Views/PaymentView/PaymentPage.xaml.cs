using CRM.Models;
using CRM.Views.PaymentView;
using System;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        Payment CurrentPayment { get; set; }

        public PaymentPage()
        {
            InitializeComponent();
        }

        public PaymentPage(Payment payment)
        {
            InitializeComponent();

            CurrentPayment = payment;

            EditToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/data_edit.png" : "data_edit.png";
            DeleteToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/garbage_closed.png" : "garbage_closed.png";

            SumEntry.Text = CurrentPayment.Sum.ToString();
            StatusEntry.Text = CurrentPayment.Status;
            MethodEntry.Text = CurrentPayment.Method;
        }

        void Edit_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new EditablePaymentPage(CurrentPayment));
            }
            catch (Exception ex)
            {
                DisplayAlert("Update operation", $"Payment wasn't updated. {ex.Message}", "OK");
            }
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Are you sure?", "An item will be deleted", "Yes", "No");

            if (userResponse)
            {
                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}/{CurrentPayment.Id}");

                var client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await DisplayAlert("Delete operation", "Payment was deleted", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Delete operation", $"Payment wasn't deleted. Response status: {response.StatusCode}", "OK");
                }
            }
        }
    }
}