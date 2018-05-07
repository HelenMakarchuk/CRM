using CRM.Data;
using CRM.Models;
using CRM.Models.Converters;
using CRM.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Payments : ContentPage
    {
        protected PaymentListViewModel _vm = new PaymentListViewModel();
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
                        (x.Status != null && paymentStatusConverter.Convert((byte)x.Status).StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase))
                        || (x.Method != null && paymentMethodConverter.Convert((byte)x.Method).StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase))
                        || x.Sum.ToString().StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase))
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