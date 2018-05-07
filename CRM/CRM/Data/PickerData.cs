using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            [Display(Name = "In process")]
            InProcess,
            Completed,
            Cancelled
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
