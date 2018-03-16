using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPageMaster : ContentPage
    {
        public ListView ListView;

        public MenuPageMaster()
        {
            InitializeComponent();

            BindingContext = new MenuPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MenuPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuPageMenuItem> MenuItems { get; set; }

            public MenuPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MenuPageMenuItem>(new[]
                {
                    new MenuPageMenuItem {
                        Id = 0,
                        Title = "Website",
                        Description = "Company website",
                        ImageSource = "picture.png",
                        TargetType = typeof(Website)
                    },
                    new MenuPageMenuItem {
                        Id = 1,
                        Title = "Departments",
                        Description = "Company departments",
                        ImageSource = "user_group.png",
                        TargetType = typeof(Departments)
                    },
                    new MenuPageMenuItem {
                        Id = 2,
                        Title = "Orders",
                        Description = "Customer orders",
                        ImageSource = "sales_order.png",
                        TargetType = typeof(Orders)
                    },
                    new MenuPageMenuItem {
                        Id = 3,
                        Title = "Users",
                        Description = "Employees",
                        ImageSource = "employee.png",
                        TargetType = typeof(Users)
                    },
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