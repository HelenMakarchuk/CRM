using CRM.Models;
using CRM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace CRM.ViewModels
{
    public class MenuPageMasterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Item> MenuItems { get; set; }

        public MenuPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<Item>(new[]
            {
                new Item {
                    Id = 0,
                    Title = "Sign in",
                    Description = "Sign in to CRM",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/login_door.png" : "login_door.png",
                    TargetType = typeof(LoginPage)
                },
                new Item {
                    Id = 1,
                    Title = "Website",
                    Description = "Company website",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/picture.png" : "picture.png",
                    TargetType = typeof(Website)
                },
                new Item {
                    Id = 2,
                    Title = "Departments",
                    Description = "Company departments",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/user_group.png" : "user_group.png",
                    TargetType = typeof(Departments)
                },
                new Item {
                    Id = 3,
                    Title = "Orders",
                    Description = "Customer orders",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/sales_order.png" : "sales_order.png",
                    TargetType = typeof(Orders)
                },
                new Item {
                    Id = 4,
                    Title = "Users",
                    Description = "Employees",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/employee.png" : "employee.png",
                    TargetType = typeof(Users)
                },
                new Item {
                    Id = 5,
                    Title = "Customers",
                    Description = "Customers",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/customer.png" : "customer.png",
                    TargetType = typeof(Customers)
                },
                    new Item {
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
