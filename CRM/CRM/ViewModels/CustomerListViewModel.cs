using CRM.Data;
using CRM.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CRM.ViewModels
{
    public class CustomerListViewModel : INotifyPropertyChanged
    {
        #region Properties

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

        List<Customer> _customerList;
        public List<Customer> CustomerList {
            get { return _customerList; }
            set {
                _customerList = value;
                OnPropertyChanged(nameof(CustomerList));
            }
        }

        Command _refreshCommand;
        public Command RefreshCommand {
            get {
                return _refreshCommand;
            }
        }

        #endregion

        public CustomerListViewModel()
        {
            _customerList = new List<Customer>();
            _refreshCommand = new Command(async () => await RefreshList());

            Task.Run(async () =>
            {
                IsBusy = true;
                CustomerList = await PopulateList();
                IsBusy = false;
            });
        }

        #region Methods

        async Task<List<Customer>> PopulateList()
        {
            _customerList = await DataLayer.Instance.GetDataAsync<Customer>().ConfigureAwait(false);
            return _customerList;
        }

        public async Task RefreshList()
        {
            IsRefreshing = true;
            CustomerList = await PopulateList();
            IsRefreshing = false;
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
