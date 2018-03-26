using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

using CRM.Models;
using System.Threading.Tasks;
using CRM.ViewModels;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Users : ContentPage
    {
        public Users()
        {
            InitializeComponent();

            if (App.IsUserLoggedIn)
            {
                AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;

                var userViewModel = new UserListViewModel();
                BindingContext = userViewModel;

                if (Device.RuntimePlatform == Device.Android)
                {
                    UserList.IsPullToRefreshEnabled = true;
                    UserList.RefreshCommand = userViewModel.RefreshCommand;
                    UserList.SetBinding(ListView.IsRefreshingProperty, nameof(userViewModel.IsRefreshing));
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

                //await Navigation.PushModalAsync(new NavigationPage(new NewUserPage()));
            }
        }
    }
}