using Microsoft.EntityFrameworkCore;
using SupplierManagement.Models.Domain;

namespace SupplierManagement.Database
{
    public class SupplierManagementDbContext : DbContext
    {
        public SupplierManagementDbContext(DbContextOptions<SupplierManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierRate> SupplierRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(s => s.SupplierId);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(450);
                entity.Property(s => s.Address).HasMaxLength(450);
                entity.Property(s => s.CreatedByUser).IsRequired().HasMaxLength(450);
                entity.Property(s => s.CreatedOn).IsRequired();

                // Index for better performance
                entity.HasIndex(s => s.Name);
            });

            // Configure SupplierRate entity
            modelBuilder.Entity<SupplierRate>(entity =>
            {
                entity.HasKey(sr => sr.SupplierRateId);
                entity.Property(sr => sr.Rate).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(sr => sr.RateStartDate).IsRequired().HasColumnType("date");
                entity.Property(sr => sr.RateEndDate).HasColumnType("date");
                entity.Property(sr => sr.CreatedByUser).IsRequired().HasMaxLength(450);
                entity.Property(sr => sr.CreatedOn).IsRequired();

                // Foreign key relationship
                entity.HasOne(sr => sr.Supplier)
                      .WithMany(s => s.SupplierRates)
                      .HasForeignKey(sr => sr.SupplierId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Indexes for better performance
                entity.HasIndex(sr => sr.SupplierId);
                entity.HasIndex(sr => sr.RateStartDate);
                entity.HasIndex(sr => sr.RateEndDate);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    SupplierId = 1,
                    Name = "BestValue",
                    Address = "1, Main Street, The District, City1, XXX-AADA",
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new Supplier
                {
                    SupplierId = 2,
                    Name = "Quality Supplies",
                    Address = "2, Industrial Ave, Business Park, City2, YYY-BBBB",
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new Supplier
                {
                    SupplierId = 3,
                    Name = "Premium Partners",
                    Address = "3, Commerce St, Trade Center, City3, ZZZ-CCCC",
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed SupplierRates
            modelBuilder.Entity<SupplierRate>().HasData(
                new SupplierRate
                {
                    SupplierRateId = 1,
                    SupplierId = 1,
                    Rate = 10m,
                    RateStartDate = new DateTime(2015, 1, 1),
                    RateEndDate = new DateTime(2015, 3, 31),
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new SupplierRate
                {
                    SupplierRateId = 2,
                    SupplierId = 1,
                    Rate = 20m,
                    RateStartDate = new DateTime(2015, 4, 1),
                    RateEndDate = new DateTime(2015, 5, 1),
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new SupplierRate
                {
                    SupplierRateId = 3,
                    SupplierId = 1,
                    Rate = 10m,
                    RateStartDate = new DateTime(2015, 5, 30),
                    RateEndDate = new DateTime(2015, 7, 25),
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new SupplierRate
                {
                    SupplierRateId = 4,
                    SupplierId = 1,
                    Rate = 25m,
                    RateStartDate = new DateTime(2015, 10, 1),
                    RateEndDate = null,
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new SupplierRate
                {
                    SupplierRateId = 5,
                    SupplierId = 2,
                    Rate = 100m,
                    RateStartDate = new DateTime(2016, 11, 1),
                    RateEndDate = null,
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new SupplierRate
                {
                    SupplierRateId = 6,
                    SupplierId = 3,
                    Rate = 30m,
                    RateStartDate = new DateTime(2016, 12, 1),
                    RateEndDate = new DateTime(2017, 1, 1),
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new SupplierRate
                {
                    SupplierRateId = 7,
                    SupplierId = 3,
                    Rate = 30m,
                    RateStartDate = new DateTime(2017, 1, 2),
                    RateEndDate = null,
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                // Add overlapping rates for testing Exercise 2
                new SupplierRate
                {
                    SupplierRateId = 8,
                    SupplierId = 1,
                    Rate = 15m,
                    RateStartDate = new DateTime(2015, 2, 15), // Overlaps with rate ID 1 (2015-01-01 to 2015-03-31)
                    RateEndDate = new DateTime(2015, 4, 15),   // Overlaps with rate ID 2 (2015-04-01 to 2015-05-01)
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new SupplierRate
                {
                    SupplierRateId = 9,
                    SupplierId = 2,
                    Rate = 90m,
                    RateStartDate = new DateTime(2016, 10, 15), // Overlaps with rate ID 5 (2016-11-01 to null)
                    RateEndDate = new DateTime(2017, 2, 1),
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new SupplierRate
                {
                    SupplierRateId = 10,
                    SupplierId = 3,
                    Rate = 25m,
                    RateStartDate = new DateTime(2016, 12, 15), // Overlaps with rate ID 6 (2016-12-01 to 2017-01-01)
                    RateEndDate = new DateTime(2017, 1, 15),    // Overlaps with rate ID 7 (2017-01-02 to null)
                    CreatedByUser = "admin.user",
                    CreatedOn = new DateTime(2021, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
