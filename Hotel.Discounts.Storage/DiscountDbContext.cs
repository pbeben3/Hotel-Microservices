using Hotel.Discounts.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Discounts.Storage
{
    public class DiscountDbContext : DbContext
    {
        private IConfiguration _configuration { get; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<CustomerDiscount> CustomerDiscounts { get; set; }
        public DbSet<Customer> Customers { get; set; }


        public DiscountDbContext(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"server=LEGION;database=discounts-dev;Trusted_Connection=True;TrustServerCertificate=True",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Discounts"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Discount>()
                .Property(cd => cd.Type)
                .HasConversion<string>();
        }
    }
}
