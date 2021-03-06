﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using Xamarin.Forms;

namespace CRM.ViewModels
{
    public class OrderListViewModel : INotifyPropertyChanged
    {
        public User user = null;
        public Customer customer = null;

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
        List<Order> _orderList;
        public List<Order> OrderList {
            get { return _orderList; }
            set {
                _orderList = value;
                OnPropertyChanged(nameof(OrderList));
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

        public OrderListViewModel(User user = null, Customer customer = null)
        {
            this.user = user;
            this.customer = customer;

            _orderList = new List<Order>();
            _refreshCommand = new Command(async () => await RefreshList());

            Task.Run(async () =>
            {
                IsBusy = true;
                OrderList = await PopulateList();
                IsBusy = false;
            });
        }

        #region Methods

        async Task<List<Order>> PopulateList()
        {
            _orderList = await DataLayer.Instance.GetDataAsync<Order>().ConfigureAwait(false);

            if (user != null)
                _orderList = _orderList.Where(o => o.DeliveryDriverId == user.Id || o.OwnerId == user.Id).ToList();
            else if (customer != null)
                _orderList = _orderList.Where(o => o.ReceiverId == customer.Id).ToList();

            return _orderList.OrderByDescending(o => o.ModifiedOn).ToList();
        }

        public async Task RefreshList()
        {
            IsRefreshing = true;
            OrderList = await PopulateList();
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
