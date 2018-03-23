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
                UserList.IsPullToRefreshEnabled = true;
                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;

                var userViewModel = new UserListViewModel();
                BindingContext = userViewModel;
            }
            else
            {
                UserList.IsPullToRefreshEnabled = false;
                MessageStackLayout.IsVisible = true;
                RefreshStackLayout.IsVisible = false;
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            if (App.IsUserLoggedIn)
            {
                await Navigation.PushModalAsync(new NavigationPage(new NewUserPage()));
            }
        }
    }
}