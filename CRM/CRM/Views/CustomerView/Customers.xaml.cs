using CRM.Models;
using CRM.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Customers : ContentPage
    {
        protected CustomerListViewModel _vm = new CustomerListViewModel();

        public Customers()
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
                    CustomerList.IsPullToRefreshEnabled = true;
                    CustomerList.RefreshCommand = _vm.RefreshCommand;
                    CustomerList.SetBinding(ListView.IsRefreshingProperty, nameof(_vm.IsRefreshing));
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    CustomerList.RowHeight = CustomerList.RowHeight * 2;
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    CustomerList.IsPullToRefreshEnabled = false;
                }

                MessageStackLayout.IsVisible = true;
                RefreshStackLayout.IsVisible = false;
            }

            CustomerList.ItemSelected += (sender, e) => {
                Navigation.PushAsync(new CustomerPage(((ListView)sender).SelectedItem as Customer));
            };
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            if (App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new NewCustomerPage());
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