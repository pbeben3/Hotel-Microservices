using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Reservations.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hotel.Reservations.Storage
{
    public class ReservationDbContext : DbContext
    {
        private IConfiguration _configuration { get; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerDiscount> CustomerDiscounts { get; set; }


        public ReservationDbContext(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"server=LEGION;database=reservations-dev;Trusted_Connection=True;TrustServerCertificate=True",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Reservations"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
