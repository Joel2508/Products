using Backend.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Context
{
    public class ContextBackend : DbContext
    {
        public ContextBackend() : base("DefaultConnection")
        {
                
        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

    }
}
