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
using CRM.Models.Converters;
using System.Text;

namespace CRM.Views
{
    /// <summary>
    /// Deprecated
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPaymentPage : ContentPage
    {
        OrderPaymentStatusConverter orderPaymentStatusConverter = new OrderPaymentStatusConverter();
        PaymentStatusConverter paymentStatusConverter = new PaymentStatusConverter();
        PaymentMethodConverter paymentMethodConverter = new PaymentMethodConverter();

        public NewPaymentPage()
        {
            InitializeComponent();

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            StatusPicker.ItemsSource = Enum.GetValues(typeof(PaymentPickerData.Status));
            MethodPicker.ItemsSource = Enum.GetValues(typeof(PaymentPickerData.Method));
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

        async void UpdateOrder(Order order)
        {
            try
            {
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Order.PluralDbTableName}/{order.Id}");
                var client = new HttpClient();

                HttpResponseMessage response = await client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Update order operation", $"Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Update order operation", $"An error occured while order status updating. {ex.Message}", "OK");
            }
        }

        async void UpdateOrderPaymentStatus(Order order)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}/$OrderId={order.Id}/$sum"),
                    Method = HttpMethod.Get,
                    Headers = { { "Accept", "application/json" } }
                };

                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();

                    Decimal paymentSum = JsonConvert.DeserializeObject<Decimal>(json);

                    if (paymentSum >= order.Sum && orderPaymentStatusConverter.Convert(order.PaymentStatus) != "Paid")
                    {
                        order.PaymentStatus = (byte)OrderPickerData.PaymentStatus.Paid;
                        UpdateOrder(order);
                    }
                    else if (paymentSum < order.Sum && orderPaymentStatusConverter.Convert(order.PaymentStatus) != "Unpaid")
                    {
                        order.PaymentStatus = (byte)OrderPickerData.PaymentStatus.Unpaid;
                        UpdateOrder(order);
                    }
                    else
                    {
                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    await DisplayAlert("Order payment status setting operation", $"Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Order payment status setting operation", $"An error occured while order payment status setting. {ex.Message}", "OK");
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

                if (MethodPicker.SelectedItem == null)
                {
                    await DisplayAlert("Create operation", "Method must be set", "OK");
                    return;
                }

                if (OrderPicker.SelectedItem == null)
                {
                    await DisplayAlert("Create operation", "Order must be set", "OK");
                    return;
                }

                if (SumEntry.Text == string.Empty || SumEntry.Text == null)
                {
                    await DisplayAlert("Create operation", "Sum must be set", "OK");
                    return;
                }
                else if (!(Decimal.TryParse(SumEntry.Text.Replace(".", ","), out var i)))
                {
                    await DisplayAlert("Create operation", "Sum: incorrect value", "OK");
                    return;
                }

                #endregion

                #region New payment assembling

                Payment payment = new Payment();
                payment.Sum = Decimal.Parse(SumEntry.Text.Replace(".", ","));

                int statusIndex = paymentStatusConverter.ConvertBack(StatusPicker.SelectedItem.ToString());
                if (Enum.IsDefined(typeof(PaymentPickerData.Status), statusIndex))
                {
                    payment.Status = (byte)statusIndex;
                }

                var methodValue = MethodPicker.SelectedItem?.ToString() ?? "";
                if (methodValue != String.Empty && methodValue != null)
                {
                    int methodIndex = paymentMethodConverter.ConvertBack(methodValue);

                    if (Enum.IsDefined(typeof(PaymentPickerData.Method), methodIndex))
                    {
                        payment.Method = (byte)methodIndex;
                    }
                }

                var selectedOrder = (Order)OrderPicker.SelectedItem;
                payment.OrderId = selectedOrder.Id;

                #endregion

                string json = JsonConvert.SerializeObject(payment);
                var content = new StringContent(json);

                var client = new HttpClient();
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //var newPaymentUri = response.Headers.Location;

                    UpdateOrderPaymentStatus(selectedOrder);
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