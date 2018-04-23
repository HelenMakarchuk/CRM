using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data
{
    public class PickerData
    {
        public static Dictionary<string, string> genders = new Dictionary<string, string>() {
            { "M", "Male" },
            { "F", "Female" },
        };

        public enum OrderStatuses {
            New,
            Assigned,
            Cancelled,
            Completed,
        };

        public enum PaymentStatuses
        {
            Paid,
            Unpaid,
        };

        public enum PaymentMethods
        {
            Cash,
            Card,
        };
    }
}
