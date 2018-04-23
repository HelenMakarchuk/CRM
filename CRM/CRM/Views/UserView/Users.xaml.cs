using CRM.Models;
using CRM.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Users : ContentPage
    {
        protected UserListViewModel viewModel;

        public Users()
        {
            InitializeComponent();

            if (App.IsUserLoggedIn)
            {
                AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;

                viewModel = new UserListViewModel();
                BindingContext = viewModel;

                if (Device.RuntimePlatform == Device.Android)
                {
                    UserList.IsPullToRefreshEnabled = true;
                    UserList.RefreshCommand = viewModel.RefreshCommand;
                    UserList.SetBinding(ListView.IsRefreshingProperty, nameof(viewModel.IsRefreshing));
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    UserList.RowHeight = UserList.RowHeight * 2;
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    UserList.IsPullToRefreshEnabled = false;
                }

                MessageStackLayout.IsVisible = true;
                RefreshStackLayout.IsVisible = false;
            }

            UserList.ItemSelected += (sender, e) => {
                Navigation.PushAsync(new UserPage(((ListView)sender).SelectedItem as User));
            };
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            if (App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new NewUserPage());
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