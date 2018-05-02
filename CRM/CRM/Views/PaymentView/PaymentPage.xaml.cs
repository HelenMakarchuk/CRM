using CRM.Models;
using CRM.Views.PaymentView;
using System;
using System.Net.Http;
using CRM.Models.Converters;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CRM.Data.PickerData;
using Newtonsoft.Json;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        Payment CurrentPayment { get; set; }
        PaymentStatusConverter paymentStatusConverter = new PaymentStatusConverter();

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

            if (CurrentPayment.Sum != null)
            {
                SumLabel.Text += CurrentPayment.Sum.ToString();
                SumLabel.IsVisible = true;
            }

            if (CurrentPayment.Status != null)
            {
                StatusLabel.Text += paymentStatusConverter.Convert((byte)CurrentPayment.Status);
                StatusLabel.IsVisible = true;
            }

            if (CurrentPayment.Method != null)
            {
                MethodLabel.Text += ((PaymentMethods)CurrentPayment.Method).ToString();
                MethodLabel.IsVisible = true;
            }

            SetOrderNumber();
        }

        protected async void SetOrderNumber()
        {
            if (CurrentPayment.OrderId != null)
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Order.PluralDbTableName}/{CurrentPayment.OrderId}"),
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
                        Order order = JsonConvert.DeserializeObject<Order>(json);
                        OrderNumberLabel.Text += order.Number;
                        OrderNumberLabel.IsVisible = true;
                    }
                    catch (Exception ex)
                    {
                        OrderNumberLabel.Text += ex.Message;
                    }
                }
                else
                {
                    OrderNumberLabel.Text += response.StatusCode.ToString();
                }
            }
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