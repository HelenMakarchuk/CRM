using CRM.Models;
using CRM.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.DepartmentView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Departments : ContentPage
    {
        protected DepartmentListViewModel _vm = new DepartmentListViewModel();

        public Departments()
        {
            InitializeComponent();

            if (App.LoggedInUser != null)
            {
                AddToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/add_new.png" : "add_new.png";

                MessageStackLayout.IsVisible = false;
                RefreshStackLayout.IsVisible = true;
                MainSearchBar.IsVisible = true;

                BindingContext = _vm;

                if (Device.RuntimePlatform == Device.Android)
                {
                    DepartmentList.IsPullToRefreshEnabled = true;
                    DepartmentList.RefreshCommand = _vm.RefreshCommand;
                    DepartmentList.SetBinding(ListView.IsRefreshingProperty, nameof(_vm.IsRefreshing));
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
                MainSearchBar.IsVisible = false;
            }

            DepartmentList.ItemSelected += (sender, e) => {
                Navigation.PushAsync(new DepartmentPage(((ListView)sender).SelectedItem as Department));
            };
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            if (App.LoggedInUser != null)
            {
                await Navigation.PushAsync(new NewDepartmentPage());
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                DepartmentList.ItemsSource = _vm.DepartmentList;
            }

            else
            {
                DepartmentList.ItemsSource = _vm.DepartmentList
                    .Where(x => 
                        ((x.Name ?? "").StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase) 
                        || (x.Phone ?? "").StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase)))
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