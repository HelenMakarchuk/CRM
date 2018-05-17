using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRM.Data
{
    public class UserPickerData
    {
        public static Dictionary<string, string> genders = new Dictionary<string, string>() {
            { "M", "Male" },
            { "F", "Female" },
        };
    }

    public class OrderPickerData
    {
        public enum DeliveryStatus
        {
            [Display(Name = "Not assigned")]
            NotAssigned,
            Assigned,
            [Display(Name = "In process")]
            InProcess,
            Completed,
            Cancelled,
        };

        public enum PaymentStatus
        {
            Paid,
            Unpaid,
        };
    }

    public class PaymentPickerData
    {
        public enum Status
        {
            Paid,
            [Display(Name = "Unpaid (error occured)")]
            Unpaid,
        };

        public enum Method
        {
            Cash,
            Card,
        };
    }
}
