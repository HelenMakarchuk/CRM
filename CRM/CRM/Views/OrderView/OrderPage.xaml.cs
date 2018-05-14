using CRM.Models;
using CRM.Models.Converters;
using CRM.Views.OrderView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        Order CurrentOrder { get; set; }
        OrderStatusConverter orderStatusConverter = new OrderStatusConverter();

        public OrderPage()
        {
            InitializeComponent();
        }

        public OrderPage(Order order)
        {
            InitializeComponent();

            CurrentOrder = order;

            EditToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/data_edit.png" : "data_edit.png";
            DeleteToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/garbage_closed.png" : "garbage_closed.png";

            NumberLabel.Text += CurrentOrder.Number;

            StatusLabel.Text += orderStatusConverter.Convert(CurrentOrder.Status);

            if (CurrentOrder.DeliveryAddress != String.Empty && CurrentOrder.DeliveryAddress != null)
            {
                DeliveryAddressLabel.Text = CurrentOrder.DeliveryAddress;
                DeliveryAddressLabel.IsVisible = true;
            }

            if (CurrentOrder.Comment != String.Empty && CurrentOrder.Comment != null)
            {
                CommentLabel.Text = CurrentOrder.Comment;
                CommentLabel.IsVisible = true;
            }

            if (CurrentOrder.CreatedOn != null)
            {
                CreatedOnLabel.Text = ((DateTime)CurrentOrder.CreatedOn).ToString("d MMMM yyyy", new CultureInfo("en-US"));
                CreatedOnLabel.IsVisible = true;
            }

            if (CurrentOrder.DeliveryDate != null)
            {
                DeliveryDateLabel.Text = ((DateTime)CurrentOrder.DeliveryDate).ToString("d MMMM yyyy", new CultureInfo("en-US"));
                DeliveryDateLabel.IsVisible = true;
            }

            SetOrderOwner();
            SetOrderDeliveryDriver();
            SetOrderReceiver();
        }

        protected async void SetOrderOwner()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/{CurrentOrder.OwnerId}"),
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
                    User owner = JsonConvert.DeserializeObject<User>(json);

                    if (owner.FullName != String.Empty && owner.FullName != null)
                    {
                        OwnerLabel.Text += owner.FullName;
                        OwnerLabel.IsVisible = true;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
            }
        }

        protected async void SetOrderDeliveryDriver()
        {
            if (CurrentOrder.DeliveryDriverId != null)
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/{CurrentOrder.DeliveryDriverId}"),
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
                        User deliveryDriver = JsonConvert.DeserializeObject<User>(json);

                        if (deliveryDriver.FullName != String.Empty && deliveryDriver.FullName != null)
                        {
                            DeliveryDriverLabel.Text += deliveryDriver.FullName;
                            DeliveryDriverLabel.IsVisible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                    }
                }
                else
                {
                    await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                }
            }
        }

        protected async void SetOrderReceiver()
        {
            if (CurrentOrder.ReceiverId != null)
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Customer.PluralDbTableName}/{CurrentOrder.ReceiverId}"),
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
                        Customer receiver = JsonConvert.DeserializeObject<Customer>(json);

                        if (receiver.Name != String.Empty && receiver.Name != null)
                        {
                            ReceiverLabel.Text += receiver.Name;
                            ReceiverLabel.IsVisible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                    }
                }
                else
                {
                    await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                }
            }
        }

        void Edit_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new EditableOrderPage(CurrentOrder));
            }
            catch (Exception ex)
            {
                DisplayAlert("Update operation", $"Order wasn't updated. {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Current order deleting
        /// </summary>
        async void Delete()
        {
            try
            { 
                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Order.PluralDbTableName}/{CurrentOrder.Id}");

                var client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await DisplayAlert("Delete operation", "Order was deleted", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Delete operation", $"Order wasn't deleted. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Delete operation", $"Order wasn't deleted. {ex.Message}", "OK");
            }
        }

        async void DeletePayments(List<int> paymentIds)
        {
            for (int i = 0; i < paymentIds.Count; i++)
            {
                try
                {
                    var uri = new Uri($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}/{paymentIds[i]}");

                    var client = new HttpClient();
                    HttpResponseMessage response = await client.DeleteAsync(uri);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (i == paymentIds.Count - 1)
                        {
                            Delete();
                        }
                    }
                    else
                    {
                        await DisplayAlert("Delete payment operation", $"Payment with id = {paymentIds[i]} wasn't deleted. Response status: {response.StatusCode}", "OK");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Delete payment operation", $"An error occured while deleting payment with id = {paymentIds[i]}. {ex.Message}", "OK");
                    return;
                }
            }
        }

        async void OpenPaymentPage(int id)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}/{id}"),
                    Method = HttpMethod.Get,
                    Headers = { { "Accept", "application/json" } }
                };

                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();

                    Payment payment = JsonConvert.DeserializeObject<Payment>(json);
                    Navigation.PushAsync(new PaymentPage(payment));
                }
                else
                {
                    await DisplayAlert("Payment information", $"Response status: {response.StatusCode}", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Payment information", $"An error occured while retrieving related payments. {ex.Message}", "OK");
                return;
            }
        }

        async void PaymentInfoButton_Clicked()
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}/$OrderId={CurrentOrder.Id}"),
                    Method = HttpMethod.Get,
                    Headers = { { "Accept", "application/json" } }
                };

                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();

                    List<int> paymentIds = JsonConvert.DeserializeObject<List<int>>(json);

                    if (paymentIds.Count > 0)
                        OpenPaymentPage(paymentIds[0]);
                    else
                        await DisplayAlert("Payment information", $"No one payment was linked to this order", "OK");
                }
                else
                {
                    await DisplayAlert("Payment information", $"Response status: {response.StatusCode}", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Payment information", $"An error occured while retrieving related payments. {ex.Message}", "OK");
                return;
            }
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Are you sure?", "The order will be deleted and related payment will be deleted too.", "Yes", "No");

            if (userResponse)
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Payment.PluralDbTableName}/$OrderId={CurrentOrder.Id}"),
                        Method = HttpMethod.Get,
                        Headers = { { "Accept", "application/json" } }
                    };

                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContent content = response.Content;
                        string json = await content.ReadAsStringAsync();

                        List<int> paymentIds = JsonConvert.DeserializeObject<List<int>>(json);

                        if (paymentIds.Count > 0)
                            DeletePayments(paymentIds);
                        else
                            Delete();
                    }
                    else
                    {
                        await DisplayAlert("Retrieve payments operation", $"Response status: {response.StatusCode}", "OK");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Retrieve payments operation", $"An error occured while retrieving related payments. {ex.Message}", "OK");
                    return;
                }               
            }
        }
    }
}