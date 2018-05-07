using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CRM.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using CRM.Data;
using static CRM.Data.PickerData;
using CRM.Models.Converters;

namespace CRM.Views
{
    /// <summary>
    /// Deprecated
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPaymentPage : ContentPage
    {
        PaymentStatusConverter paymentStatusConverter = new PaymentStatusConverter();
        PaymentMethodConverter paymentMethodConverter = new PaymentMethodConverter();

        public NewPaymentPage()
        {
            InitializeComponent();

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            StatusPicker.ItemsSource = Enum.GetValues(typeof(PickerData.PaymentStatuses));
            MethodPicker.ItemsSource = Enum.GetValues(typeof(PickerData.PaymentMethods));
            FillOrderNumberPicker();

            BindingContext = this;
        }

        protected async void FillOrderNumberPicker()
        {
            try
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
                    }
                    catch (Exception)
                    {
                        OrderPicker.ItemsSource = new List<Order>();
                    }
                }
                else
                {
                    await DisplayAlert("OrderNumberPicker", $"Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("OrderNumberPicker", $"{ex.Message}", "OK");
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region Checks

                if (StatusPicker.SelectedItem == null)
                {
                    await DisplayAlert("Create operation", "Status must be set", "OK");
                    return;
                }

                #endregion

                #region New payment assembling

                Payment payment = new Payment();

                if (SumEntry.Text != String.Empty && SumEntry.Text != null && Decimal.TryParse(SumEntry.Text.Replace(".", ","), out var i))
                {
                    payment.Sum = Decimal.Parse(SumEntry.Text.Replace(".", ","));
                }

                int statusIndex = paymentStatusConverter.ConvertBack(StatusPicker.SelectedItem.ToString());
                if (Enum.IsDefined(typeof(PickerData.PaymentStatuses), statusIndex))
                {
                    payment.Status = (byte)statusIndex;
                }

                var methodValue = MethodPicker.SelectedItem?.ToString() ?? "";
                if (methodValue != String.Empty && methodValue != null)
                {
                    int methodIndex = paymentMethodConverter.ConvertBack(methodValue);

                    if (Enum.IsDefined(typeof(PickerData.PaymentMethods), methodIndex))
                    {
                        payment.Method = (byte)methodIndex;
                    }
                }

                if (OrderPicker.SelectedItem is Order selectedOrder)
                {
                    payment.OrderId = selectedOrder.Id;
                }

                #endregion

                string json = JsonConvert.SerializeObject(payment);
                var content = new StringContent(json);

                var client = new HttpClient();
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //var newPaymentUri = response.Headers.Location;

                    await DisplayAlert("Create operation", $"Payment was created.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Create operation", $"Payment wasn't created. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Create operation", $"An error occured while creating payment. {ex.Message}", "OK");
            }
        }
    }
}