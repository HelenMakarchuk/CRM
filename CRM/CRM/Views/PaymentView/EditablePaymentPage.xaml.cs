using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.PaymentView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditablePaymentPage : ContentPage
	{
        Payment CurrentPayment { get; set; }

        public EditablePaymentPage()
        {
            InitializeComponent();
        }

        public EditablePaymentPage(Payment payment)
        {
            InitializeComponent();

            CurrentPayment = payment;

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            SumEntry.Text = CurrentPayment.Sum.ToString();
            StatusEntry.Text = CurrentPayment.Status;
            MethodEntry.Text = CurrentPayment.Method;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region Updated payment assembling

                Payment payment = new Payment();
                payment.Id = CurrentPayment.Id;
                payment.Status = StatusEntry.Text;
                payment.Method = MethodEntry.Text;

                if (SumEntry.Text != String.Empty && SumEntry.Text != null)
                {
                    if (Decimal.TryParse(SumEntry.Text, out var i))
                        payment.Sum = Decimal.Parse(SumEntry.Text);
                }
                else
                {
                    payment.Sum = null;
                }

                #endregion

                var json = JsonConvert.SerializeObject(payment);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}/{CurrentPayment.Id}");
                var client = new HttpClient();

                HttpResponseMessage response = await client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Update operation", "Payment was updated", "OK");

                    if (Navigation.NavigationStack.Count > 0)
                    {
                        int currentPageIndex = Navigation.NavigationStack.Count - 1;
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                        await Navigation.PushAsync(new PaymentPage(payment));
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                    }
                }
                else
                {
                    await DisplayAlert("Update operation", $"Payment wasn't updated. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Update operation", $"Payment wasn't updated. {ex.Message}", "OK");
            }
        }
    }
}