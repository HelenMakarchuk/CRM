using System;

namespace CRM.Models
{
    class User
    {
        public User()
        {
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public Nullable<int> Department { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Gender { get; set; }
    }
}
