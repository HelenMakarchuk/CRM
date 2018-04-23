﻿using CRM.Models;
using CRM.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Departments : ContentPage
    {
        protected DepartmentListViewModel viewModel;

        public Departments()
        {
            InitializeComponent();

            if (App.IsUserLoggedIn)
            {
                AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;

                viewModel = new DepartmentListViewModel();
                BindingContext = viewModel;

                if (Device.RuntimePlatform == Device.Android)
                {
                    DepartmentList.IsPullToRefreshEnabled = true;
                    DepartmentList.RefreshCommand = viewModel.RefreshCommand;
                    DepartmentList.SetBinding(ListView.IsRefreshingProperty, nameof(viewModel.IsRefreshing));
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    DepartmentList.RowHeight = DepartmentList.RowHeight * 2;
                }
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
                Navigation.PushAsync(new DepartmentPage(((ListView)sender).SelectedItem as Department));
            };
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            if (App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new NewDepartmentPage());
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