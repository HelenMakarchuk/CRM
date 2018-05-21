using CRM.Models;
using CRM.Views.PaymentView;
using System;
using System.Net.Http;
using CRM.Models.Converters;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using CRM.Data;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        Payment CurrentPayment { get; set; }
        PaymentStatusConverter paymentStatusConverter = new PaymentStatusConverter();
        SumConverter sumConverter = new SumConverter();

        public PaymentPage()
        {
            InitializeComponent();
        }

        public PaymentPage(Payment payment)
        {
            InitializeComponent();

            CurrentPayment = payment;

            if (CurrentPayment.Sum != null)
            {
                SumLabel.Text += sumConverter.Convert(CurrentPayment.Sum.ToString());
                SumLabel.IsVisible = true;
            }

            if (CurrentPayment.Status != null)
            {
                StatusLabel.Text += paymentStatusConverter.Convert((byte)CurrentPayment.Status);
                StatusLabel.IsVisible = true;
            }

            if (CurrentPayment.Method != null)
            {
                MethodLabel.Text += ((PaymentPickerData.Method)CurrentPayment.Method).ToString();
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
    }
}