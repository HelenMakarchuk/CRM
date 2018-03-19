using System;
using System.Collections.Generic;

namespace UnitTests
{
    public class Payment
    {
        public Payment()
        {
            this.Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> Sum { get; set; }
        public string Method { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
