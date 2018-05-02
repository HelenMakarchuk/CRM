using CRM.Data;
using CRM.Models;
using CRM.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Payments : ContentPage
    {
        protected PaymentListViewModel _vm = new PaymentListViewModel();

        public Payments()
        {
            InitializeComponent();

            if (App.IsUserLoggedIn)
            {
                AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;

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
            }

            PaymentList.ItemSelected += (sender, e) => {
                Navigation.PushAsync(new PaymentPage(((ListView)sender).SelectedItem as Payment));
            };
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            if (App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new NewPaymentPage());
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