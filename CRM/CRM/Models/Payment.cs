using System;

namespace CRM.Models
{
    class Payment
    {
        public Payment()
        {
        }

        public int Id { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> Sum { get; set; }
        public string Method { get; set; }
    }
}
