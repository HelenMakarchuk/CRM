using CRM.Models;
using CRM.ViewModels;
using System;
using System.Linq;
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
                MainSearchBar.IsVisible = true;

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
                MainSearchBar.IsVisible = false;
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

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                CustomerList.ItemsSource = _vm.CustomerList;
            }

            else
            {
                CustomerList.ItemsSource = _vm.CustomerList
                    .Where(x => 
                        (x.Name.StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase) 
                        || x.Phone.StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase) 
                        || x.Email.StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)))
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