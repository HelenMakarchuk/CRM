using CRM.Data;
using CRM.Models;
using CRM.Models.Converters;
using CRM.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.PaymentView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Payments : ContentPage
    {
        protected PaymentListViewModel _vm;
        PaymentStatusConverter paymentStatusConverter = new PaymentStatusConverter();
        PaymentMethodConverter paymentMethodConverter = new PaymentMethodConverter();

        public Payments()
        {
            InitializeComponent();

            if (App.IsUserLoggedIn)
            {
                AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;
                MainSearchBar.IsVisible = true;

                _vm = new PaymentListViewModel();
                BindingContext = _vm;

                if (Device.RuntimePlatform == Device.Android)
                {
                    PaymentList.IsPullToRefreshEnabled = true;
                    PaymentList.RefreshCommand = _vm.RefreshCommand;
                    PaymentList.SetBinding(ListView.IsRefreshingProperty, nameof(_vm.IsRefreshing));
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    PaymentList.RowHeight = PaymentList.RowHeight * 2;
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    PaymentList.IsPullToRefreshEnabled = false;
                }

                MessageStackLayout.IsVisible = true;
                RefreshStackLayout.IsVisible = false;
                MainSearchBar.IsVisible = false;
            }

            PaymentList.ItemSelected += (sender, e) => {
                Navigation.PushAsync(new PaymentPage(((ListView)sender).SelectedItem as Payment));
            };
        }

        public Payments(Order order)
        {
            InitializeComponent();

            AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

            MessageStackLayout.IsVisible = false;
            RefreshStackLayout.IsVisible = true;
            MainSearchBar.IsVisible = true;

            _vm = new PaymentListViewModel(order);
            BindingContext = _vm;

            if (Device.RuntimePlatform == Device.Android)
            {
                PaymentList.IsPullToRefreshEnabled = true;
                PaymentList.RefreshCommand = _vm.RefreshCommand;
                PaymentList.SetBinding(ListView.IsRefreshingProperty, nameof(_vm.IsRefreshing));
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                PaymentList.RowHeight = PaymentList.RowHeight * 2;
            }

            PaymentList.ItemSelected += (sender, e) => {
                Navigation.PushAsync(new PaymentPage(((ListView)sender).SelectedItem as Payment));
            };
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                PaymentList.ItemsSource = _vm.PaymentList;
            }

            else
            {
                PaymentList.ItemsSource = _vm.PaymentList
                    .Where(x =>
                        (x.Status != null && paymentStatusConverter.Convert((byte)x.Status).ToString().StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase))
                        || (x.Method != null && paymentMethodConverter.Convert((byte)x.Method).ToString().StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase))
                        || x.Sum.ToString().StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
            }
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            if (App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new NewPaymentPage(_vm.order));
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