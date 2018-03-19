using System;

namespace UnitTests
{
    public class Order
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Status { get; set; }
        public int OwnerId { get; set; }
        public Nullable<int> DeliveryDriverId { get; set; }
        public string DeliveryAddress { get; set; }
        public Nullable<int> ReceiverId { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<int> PaymentId { get; set; }

        public virtual Customer Customers { get; set; }
        public virtual User DeliveryDrivers { get; set; }
        public virtual User Owners { get; set; }
        public virtual Payment Payments { get; set; }
    }
}
