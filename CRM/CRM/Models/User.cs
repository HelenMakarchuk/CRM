using System;
using System.Collections.Generic;

namespace CRM.Models
{
    public class User
    {
        public User()
        {
            this.HeadedDepartments = new HashSet<Department>();
        }

        public static string PluralDbTableName { get { return "Users"; } }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Gender { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Department> HeadedDepartments { get; set; }
        public virtual Department WorkplaceDepartment { get; set; }
    }
}
