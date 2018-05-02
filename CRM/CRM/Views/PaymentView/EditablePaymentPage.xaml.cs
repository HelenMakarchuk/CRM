using CRM.Data;
using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using CRM.Models.Converters;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace CRM.Views.PaymentView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditablePaymentPage : ContentPage
	{
        Payment CurrentPayment { get; set; }
        PaymentStatusConverter paymentStatusConverter = new PaymentStatusConverter();
        PaymentMethodConverter paymentMethodConverter = new PaymentMethodConverter();

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

            //StatusEntry.Text = CurrentPayment.Status;
            StatusPicker.ItemsSource = Enum.GetValues(typeof(PickerData.PaymentStatuses)).Cast<PickerData.PaymentStatuses>().Select(x => x.ToString()).ToList();

            if (CurrentPayment.Status != null)
                StatusPicker.SelectedItem = paymentStatusConverter.Convert((byte)CurrentPayment.Status);

            //MethodEntry.Text = CurrentPayment.Method;
            MethodPicker.ItemsSource = Enum.GetValues(typeof(PickerData.PaymentMethods)).Cast<PickerData.PaymentMethods>().Select(x => x.ToString()).ToList();

            if (CurrentPayment.Method != null)
                MethodPicker.SelectedItem = paymentMethodConverter.Convert((byte)CurrentPayment.Method);

            SetPaymentOrder();
        }

        protected async void SetPaymentOrder()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Order.PluralDbTableName}"),
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
                    List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(json);
                    OrderPicker.ItemsSource = orders;
                    OrderPicker.SelectedItem = orders.Where(o => o.Id == CurrentPayment.OrderId).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                    OrderPicker.ItemsSource = new List<Order>();
                }
            }
            else
            {
                await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                OrderPicker.ItemsSource = new List<Order>();
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region Updated payment assembling

                Payment payment = new Payment();
                payment.Id = CurrentPayment.Id;

                if (StatusPicker.SelectedItem != null)
                    payment.Status = (byte)paymentStatusConverter.ConvertBack(StatusPicker.SelectedItem.ToString());
                else
                    payment.Status = null;

                if (MethodPicker.SelectedItem != null)
                    payment.Method = (byte)paymentMethodConverter.ConvertBack(MethodPicker.SelectedItem.ToString());
                else
                    payment.Method = null;

                if (OrderPicker.SelectedItem is Order selectedOrder)
                {
                    payment.OrderId = selectedOrder.Id;
                }
                else
                {
                    payment.OrderId = null;
                }

                if (SumEntry.Text != String.Empty && SumEntry.Text != null && Decimal.TryParse(SumEntry.Text.Replace(".", ","), out var i))
                {
                    payment.Sum = Decimal.Parse(SumEntry.Text.Replace(".", ","));
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