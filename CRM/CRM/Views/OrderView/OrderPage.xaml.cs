using CRM.Models;
using CRM.Views.OrderView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        Order CurrentOrder { get; set; }

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

            NumberEntry.Text = CurrentOrder.Number;
            StatusEntry.Text = CurrentOrder.Status;
            DeliveryAddressEntry.Text = CurrentOrder.DeliveryAddress;
            CommentEntry.Text = CurrentOrder.Comment;

            CreatedOnPicker.Date = order.CreatedOn;
            DeliveryDatePicker.Date = order.DeliveryDate ?? DateTime.MinValue;

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
                    OwnerPicker.ItemsSource = new List<User>() { owner };
                    OwnerPicker.SelectedItem = owner;

                    if (owner.FullName == String.Empty || owner.FullName == null)
                        OwnerPicker.Title = "";
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                    OwnerPicker.ItemsSource = new List<User>();
                }
            }
            else
            {
                await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                OwnerPicker.ItemsSource = new List<User>();
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
                        DeliveryDriverPicker.ItemsSource = new List<User>() { deliveryDriver };
                        DeliveryDriverPicker.SelectedItem = deliveryDriver;

                        if (deliveryDriver.FullName == String.Empty || deliveryDriver.FullName == null)
                            DeliveryDriverPicker.Title = "";
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                        DeliveryDriverPicker.ItemsSource = new List<User>();
                    }
                }
                else
                {
                    await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                    DeliveryDriverPicker.ItemsSource = new List<User>();
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
                        ReceiverPicker.ItemsSource = new List<Customer>() { receiver };
                        ReceiverPicker.SelectedItem = receiver;

                        if (receiver.Name == String.Empty || receiver.Name == null)
                            ReceiverPicker.Title = "";
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                        ReceiverPicker.ItemsSource = new List<Customer>();
                    }
                }
                else
                {
                    await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                    ReceiverPicker.ItemsSource = new List<Customer>();
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

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Are you sure?", "An item will be deleted", "Yes", "No");

            if (userResponse)
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
        }
    }
}