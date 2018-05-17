using System;
using System.Collections.Generic;

namespace CRM.Models
{
    public partial class Order
    {
        public Order()
        {
            this.Payments = new HashSet<Payment>();
        }

        public static string PluralDbTableName { get { return "Orders"; } }

        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public byte DeliveryStatus { get; set; }
        public int OwnerId { get; set; }
        public Nullable<int> DeliveryDriverId { get; set; }
        public string DeliveryAddress { get; set; }
        public Nullable<int> ReceiverId { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<decimal> Sum { get; set; }
        public byte PaymentStatus { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
