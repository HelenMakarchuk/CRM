using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ORM;

namespace WebAPI.Models
{
    public class DepartmentsContext : DbContext
    {
        public DepartmentsContext (DbContextOptions<DepartmentsContext> options)
            : base(options)
        {
        }

        public DbSet<ORM.Department> Department { get; set; }
    }
}
