using System;
using System.Collections.Generic;

namespace UnitTests
{
    public class User
    {
        public User()
        {
            this.HeadedDepartments = new HashSet<Department>();
            this.OrdersForDelivery = new HashSet<Order>();
            this.CreatedOrders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Gender { get; set; }

        public virtual ICollection<Department> HeadedDepartments { get; set; }
        public virtual Department WorkplaceDepartments { get; set; }
        public virtual ICollection<Order> OrdersForDelivery { get; set; }
        public virtual ICollection<Order> CreatedOrders { get; set; }
    }
}
