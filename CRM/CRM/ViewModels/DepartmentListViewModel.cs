using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using Xamarin.Forms;

namespace CRM.ViewModels
{
    public class DepartmentListViewModel : INotifyPropertyChanged
    {
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
        List<Department> _departmentList;
        public List<Department> DepartmentList {
            get { return _departmentList; }
            set {
                _departmentList = value;
                OnPropertyChanged(nameof(DepartmentList));
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

        public DepartmentListViewModel()
        {
            _departmentList = new List<Department>();
            _refreshCommand = new Command(async () => await RefreshList());

            Task.Run(async () =>
            {
                IsBusy = true;
                DepartmentList = await PopulateList();
                IsBusy = false;
            });
        }

        #region Methods

        async Task<List<Department>> PopulateList()
        {
            _departmentList = await DataLayer.Instance.GetDataAsync<Department>().ConfigureAwait(false);
            return _departmentList;
        }

        async Task RefreshList()
        {
            IsRefreshing = true;
            DepartmentList = await PopulateList();
            IsRefreshing = false;
        }

        #endregion

        #region INotifyPropertyChanged implementation

        //To let the UI know that something changed on the View Model
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
