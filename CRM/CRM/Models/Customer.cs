using System.Collections.Generic;

namespace CRM.Models
{
    public class Customer
    {
        public static string PluralDbTableName { get { return "Customers"; } }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
