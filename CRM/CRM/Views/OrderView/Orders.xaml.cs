using CRM.Data;
using CRM.Models;
using CRM.Models.Converters;
using CRM.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.OrderView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Orders : ContentPage
    {
        protected OrderListViewModel _vm;
        OrderDeliveryStatusConverter orderStatusConverter = new OrderDeliveryStatusConverter();

        public Orders()
        {
            InitializeComponent();
            
            if (App.IsUserLoggedIn)
            {
                AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;
                MainSearchBar.IsVisible = true;

                _vm = new OrderListViewModel();
                BindingContext = _vm;

                if (Device.RuntimePlatform == Device.Android)
                {
                    OrderList.IsPullToRefreshEnabled = true;
                    OrderList.RefreshCommand = _vm.RefreshCommand;
                    OrderList.SetBinding(ListView.IsRefreshingProperty, nameof(_vm.IsRefreshing));
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    OrderList.RowHeight = OrderList.RowHeight * 2;
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    OrderList.IsPullToRefreshEnabled = false;
                }

                MessageStackLayout.IsVisible = true;
                RefreshStackLayout.IsVisible = false;
                MainSearchBar.IsVisible = false;
            }

            OrderList.ItemSelected += (sender, e) => {
                Navigation.PushAsync(new OrderPage(((ListView)sender).SelectedItem as Order));
            };
        }

        public Orders(User user)
        {
            InitializeComponent();

            AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

            MessageStackLayout.IsVisible = false;
            RefreshStackLayout.IsVisible = true;
            MainSearchBar.IsVisible = true;

            _vm = new OrderListViewModel(user);
            BindingContext = _vm;

            if (Device.RuntimePlatform == Device.Android)
            {
                OrderList.IsPullToRefreshEnabled = true;
                OrderList.RefreshCommand = _vm.RefreshCommand;
                OrderList.SetBinding(ListView.IsRefreshingProperty, nameof(_vm.IsRefreshing));
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                OrderList.RowHeight = OrderList.RowHeight * 2;
            }

            OrderList.ItemSelected += (sender, e) => {
                Navigation.PushAsync(new OrderPage(((ListView)sender).SelectedItem as Order));
            };
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            if (App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new NewOrderPage());
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                OrderList.ItemsSource = _vm.OrderList;
            }

            else
            {
                OrderList.ItemsSource = _vm.OrderList
                    .Where(x =>
                        ((x.Number ?? "").StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)
                        || (x.DeliveryAddress ?? "").StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)
                        || (x.Comment ?? "").StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)
                        || orderStatusConverter.Convert(x.DeliveryStatus).ToString().StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)))
                    .ToList();
            }
        }

        async protected override void OnAppearing()
        {
            if (App.IsUserLoggedIn)
                await _vm.RefreshList();

            base.OnAppearing();
        }
    }
}