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
        protected CustomerListViewModel viewModel;

        public Customers()
        {
            InitializeComponent();

            if (App.IsUserLoggedIn)
            {
                AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;

                viewModel = new CustomerListViewModel();
                BindingContext = viewModel;

                if (Device.RuntimePlatform == Device.Android)
                {
                    CustomerList.IsPullToRefreshEnabled = true;
                    CustomerList.RefreshCommand = viewModel.RefreshCommand;
                    CustomerList.SetBinding(ListView.IsRefreshingProperty, nameof(viewModel.IsRefreshing));
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
                await viewModel.RefreshList();

            base.OnAppearing();
        }
    }
}