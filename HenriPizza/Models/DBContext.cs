using System.Data.Entity;
using HenriPizza.Models;

namespace HenriPizza.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=DBContext")
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<OrderSummary> OrderSummaries { get; set; }


    }
    }

