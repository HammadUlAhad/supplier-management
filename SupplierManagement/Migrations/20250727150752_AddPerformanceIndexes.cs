using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupplierManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Critical indexes for high-performance overlap detection
            
            // Composite index for overlap queries by supplier and date range
            migrationBuilder.Sql(@"
                CREATE NONCLUSTERED INDEX [IX_SupplierRates_SupplierId_DateRange] 
                ON [SupplierRates] ([SupplierId], [RateStartDate], [RateEndDate])
                INCLUDE ([SupplierRateId], [Rate])");

            // Index optimized for overlap detection queries
            migrationBuilder.Sql(@"
                CREATE NONCLUSTERED INDEX [IX_SupplierRates_Overlap_Optimized] 
                ON [SupplierRates] ([RateStartDate], [RateEndDate]) 
                INCLUDE ([SupplierId], [SupplierRateId], [Rate])");

            // Index for supplier name searches
            migrationBuilder.Sql(@"
                CREATE NONCLUSTERED INDEX [IX_Suppliers_Name_Performance] 
                ON [Suppliers] ([Name]) 
                INCLUDE ([SupplierId], [Address])");

            // Index for date range queries
            migrationBuilder.Sql(@"
                CREATE NONCLUSTERED INDEX [IX_SupplierRates_DateRange_Only] 
                ON [SupplierRates] ([RateStartDate]) 
                INCLUDE ([RateEndDate], [SupplierId], [SupplierRateId])");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the performance indexes
            migrationBuilder.Sql("DROP INDEX [IX_SupplierRates_SupplierId_DateRange] ON [SupplierRates]");
            migrationBuilder.Sql("DROP INDEX [IX_SupplierRates_Overlap_Optimized] ON [SupplierRates]");
            migrationBuilder.Sql("DROP INDEX [IX_Suppliers_Name_Performance] ON [Suppliers]");
            migrationBuilder.Sql("DROP INDEX [IX_SupplierRates_DateRange_Only] ON [SupplierRates]");
        }
    }
}
