using System;
using System.Collections.Generic;

namespace UnitTests
{
    public class Department
    {
        public Department()
        {
            this.Employees = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> HeadId { get; set; }

        public virtual User Heads { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Employees { get; set; }
    }
}
