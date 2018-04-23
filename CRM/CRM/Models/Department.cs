using System;
using System.Collections.Generic;

namespace CRM.Models
{
    public class Department
    {
        public Department()
        {
            this.Employees = new HashSet<User>();
        }

        public static string PluralDbTableName { get { return "Departments"; } }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> HeadId { get; set; }
        public string Phone { get; set; }

        public virtual User Head { get; set; }

        public virtual ICollection<User> Employees { get; set; }
    }
}
