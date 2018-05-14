using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using Xamarin.Forms;

namespace CRM.ViewModels
{
    public class DepartmentUserListViewModel : INotifyPropertyChanged
    {
        public Department department;

        #region Properties

        //To let the user know that we are working on something
        bool _isBusy;
        public bool IsBusy {
            get { return _isBusy; }
            set {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        bool _isRefreshing;
        public bool IsRefreshing {
            get { return _isRefreshing; }
            set {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        //Our list of objects!
        List<User> _userList;
        public List<User> UserList {
            get { return _userList; }
            set {
                _userList = value;
                OnPropertyChanged(nameof(UserList));
            }
        }

        //Refresh command
        Command _refreshCommand;
        public Command RefreshCommand {
            get {
                return _refreshCommand;
            }
        }

        #endregion

        public DepartmentUserListViewModel(Department department)
        {
            this.department = department;

            _userList = new List<User>();
            _refreshCommand = new Command(async () => await RefreshList());

            Task.Run(async () =>
            {
                IsBusy = true;
                UserList = await PopulateList();
                IsBusy = false;
            });
        }

        #region Methods

        async Task<List<User>> PopulateList()
        {
            _userList = await DataLayer.Instance.GetDataAsync<User>().ConfigureAwait(false);
            return _userList.Where(u => u.DepartmentId == department.Id).ToList();
        }

        public async Task RefreshList()
        {
            IsRefreshing = true;
            UserList = await PopulateList();
            IsRefreshing = false;
        }

        #endregion

        #region INotifyPropertyChanged implementation

        //To let the UI know that something changed on the View Model
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
