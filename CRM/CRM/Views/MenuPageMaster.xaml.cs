using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CRM.Models;
using System.Net;
using System.IO;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPageMaster : ContentPage
    {
        public MenuPageMaster()
        {
            InitializeComponent();

            BindingContext = new MenuPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        public ListView ListView;

        class MenuPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuPageMenuItem> MenuItems { get; set; }

            public MenuPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MenuPageMenuItem>(new[]
                {
                    new MenuPageMenuItem {
                        Id = 0,
                        Title = "Sign in",
                        Description = "Sign in to CRM",
                        ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/login_door.png" : "login_door.png",
                        TargetType = typeof(LoginPage)
                    },
                    new MenuPageMenuItem {
                        Id = 1,
                        Title = "Website",
                        Description = "Company website",
                        ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/picture.png" : "picture.png",
                        TargetType = typeof(Website)
                    },
                    new MenuPageMenuItem {
                        Id = 2,
                        Title = "Departments",
                        Description = "Company departments",
                        ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/user_group.png" : "user_group.png",
                        TargetType = typeof(Departments)
                    },
                    new MenuPageMenuItem {
                        Id = 3,
                        Title = "Orders",
                        Description = "Customer orders",
                        ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/sales_order.png" : "sales_order.png",
                        TargetType = typeof(Orders)
                    },
                    new MenuPageMenuItem {
                        Id = 4,
                        Title = "Users",
                        Description = "Employees",
                        ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/employee.png" : "employee.png",
                        TargetType = typeof(Users)
                    },
                    new MenuPageMenuItem {
                        Id = 5,
                        Title = "Customers",
                        Description = "Customers",
                        ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/customer.png" : "customer.png",
                        TargetType = typeof(Customers)
                    },
                     new MenuPageMenuItem {
                        Id = 6,
                        Title = "Payments",
                        Description = "Payments",
                        ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/payment.png" : "payment.png",
                        TargetType = typeof(Payments)
                    }
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}