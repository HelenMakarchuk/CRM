using CRM.Models;
using CRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.UserView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DepartmentUsers : ContentPage
    {
        protected DepartmentUserListViewModel _vm;

        public DepartmentUsers(Department department)
        {
            InitializeComponent();
            _vm = new DepartmentUserListViewModel(department);

            if (App.LoggedInUser != null)
            {
                AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;
                MainSearchBar.IsVisible = true;

                BindingContext = _vm;

                if (Device.RuntimePlatform == Device.Android)
                {
                    UserList.IsPullToRefreshEnabled = true;
                    UserList.RefreshCommand = _vm.RefreshCommand;
                    UserList.SetBinding(ListView.IsRefreshingProperty, nameof(_vm.IsRefreshing));
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
                MainSearchBar.IsVisible = false;
            }

            UserList.ItemSelected += (sender, e) => {
                Navigation.PushAsync(new UserPage(((ListView)sender).SelectedItem as User));
            };
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            if (App.LoggedInUser != null)
            {
                await Navigation.PushAsync(new NewUserPage(_vm.department));
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                UserList.ItemsSource = _vm.UserList;
            }

            else
            {
                UserList.ItemsSource = _vm.UserList
                    .Where(x =>
                        (x.FullName.StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)
                        || x.Position.StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)
                        || x.Phone.StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)
                        || x.Email.StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)))
                    .ToList();
            }
        }

        async protected override void OnAppearing()
        {
            if (App.LoggedInUser != null)
                await _vm.RefreshList();

            base.OnAppearing();
        }
    }
}