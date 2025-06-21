using Hotel.Customers.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Customers.Storage
{
    public class CustomerDbContext : DbContext
    {
        private IConfiguration _configuration { get; }
        public DbSet<Customer> Customers { get; set; }

        public CustomerDbContext(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"server=LEGION;database=customers-dev;Trusted_Connection=True;TrustServerCertificate=True",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Customers"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
