using System;

namespace CRM.Models
{
    class Order
    {
        public Order()
        {
        }

        public int Id { get; set; }
        public string Number { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Status { get; set; }
        public int Owner { get; set; }
        public Nullable<int> DeliveryDriver { get; set; }
        public string DeliveryAddress { get; set; }
        public Nullable<int> Receiver { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<int> Payment { get; set; }
    }
}
