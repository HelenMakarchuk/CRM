using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            [Description("Not assigned")]
            NotAssigned,
            Assigned,
            [Description("In process")]
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
            [Description("Unpaid (error occured)")]
            Unpaid,
        };

        public enum Method
        {
            Cash,
            Card,
        };
    }
}
