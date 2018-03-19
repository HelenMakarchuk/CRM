using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ORM;

namespace WebAPI.Models
{
    public class PaymentsContext : DbContext
    {
        public PaymentsContext (DbContextOptions<PaymentsContext> options)
            : base(options)
        {
        }

        public DbSet<ORM.Payment> Payment { get; set; }
    }
}
