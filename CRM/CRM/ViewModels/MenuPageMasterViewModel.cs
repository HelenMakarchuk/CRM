using CRM.Models;
using CRM.Views.BiReportsView;
using CRM.Views.CustomerView;
using CRM.Views.DepartmentView;
using CRM.Views.LoginView;
using CRM.Views.OrderView;
using CRM.Views.PaymentView;
using CRM.Views.UserView;
using CRM.Views.WebsiteView;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
                    Name = "Sign in",
                    Description = "Sign in to CRM",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/login_door.png" : "login_door.png",
                    TargetType = typeof(LoginPage)
                },
                new Item {
                    Id = 1,
                    Title = "Website",
                    Name = "Website",
                    Description = "Company website",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/picture.png" : "picture.png",
                    TargetType = typeof(Website)
                },
                new Item {
                    Id = 2,
                    Title = "BI Reports",
                    Name = "BI Reports",
                    Description = "BI dashboard",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/bi_report.png" : "bi_report.png",
                    TargetType = typeof(BiReports)
                },
                new Item {
                    Id = 3,
                    Title = "Departments",
                    Name = "Departments",
                    Description = "Company departments",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/user_group.png" : "user_group.png",
                    TargetType = typeof(Departments)
                },
                new Item {
                    Id = 4,
                    Title = "Orders",
                    Name = "Orders",
                    Description = "Customer orders",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/sales_order.png" : "sales_order.png",
                    TargetType = typeof(Orders)
                },
                new Item {
                    Id = 5,
                    Title = "Users",
                    Name = "Users",
                    Description = "Employees",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/employee.png" : "employee.png",
                    TargetType = typeof(Users)
                },
                new Item {
                    Id = 6,
                    Title = "Customers",
                    Name = "Customers",
                    Description = "Customers",
                    ImageSource = Device.RuntimePlatform == Device.UWP ? "Assets/customer.png" : "customer.png",
                    TargetType = typeof(Customers)
                },
                    new Item {
                    Id = 7,
                    Title = "Payments",
                    Name = "Payments",
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
