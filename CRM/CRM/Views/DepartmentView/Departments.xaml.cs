using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

using CRM.Models;
using CRM.ViewModels;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Departments : ContentPage
    {
        public Departments()
        {
            InitializeComponent();

            if (App.IsUserLoggedIn)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    DepartmentList.IsPullToRefreshEnabled = true;
                }

                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;

                var departmentViewModel = new DepartmentListViewModel();
                BindingContext = departmentViewModel;
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    DepartmentList.IsPullToRefreshEnabled = false;
                }

                MessageStackLayout.IsVisible = true;
                RefreshStackLayout.IsVisible = false;
            }

            DepartmentList.ItemSelected += (sender, e) => {
                Navigation.PushAsync(new UserPage());
            };
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new UserPage());
        }
    }
}
