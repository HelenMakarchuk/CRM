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
    public class PaymentListViewModel : INotifyPropertyChanged
    {
        public Order order = null;

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
        List<Payment> _paymentList;
        public List<Payment> PaymentList {
            get { return _paymentList; }
            set {
                _paymentList = value;
                OnPropertyChanged(nameof(PaymentList));
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

        public PaymentListViewModel(Order order = null)
        {
            this.order = order;
            _paymentList = new List<Payment>();
            _refreshCommand = new Command(async () => await RefreshList());

            Task.Run(async () =>
            {
                IsBusy = true;
                PaymentList = await PopulateList();
                IsBusy = false;
            });
        }

        #region Methods

        async Task<List<Payment>> PopulateList()
        {
            _paymentList = await DataLayer.Instance.GetDataAsync<Payment>().ConfigureAwait(false);

            if (order != null)
                _paymentList = _paymentList.Where(p => p.OrderId == order.Id).ToList();

            return _paymentList;
        }

        public async Task RefreshList()
        {
            IsRefreshing = true;
            PaymentList = await PopulateList();
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
