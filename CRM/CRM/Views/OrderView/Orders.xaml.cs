using CRM.Models;
using CRM.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Orders : ContentPage
    {
        protected OrderListViewModel _vm = new OrderListViewModel();

        public Orders()
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

        async protected override void OnAppearing()
        {
            if (App.IsUserLoggedIn)
                await _vm.RefreshList();

            base.OnAppearing();
        }
    }
}