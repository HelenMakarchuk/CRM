using System;

namespace CRM.Models
{
    class Department
    {
        public Department()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Head { get; set; }
    }
}
