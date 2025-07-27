using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SupplierManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddOverlappingTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SupplierRates",
                columns: new[] { "SupplierRateId", "CreatedByUser", "CreatedOn", "Rate", "RateEndDate", "RateStartDate", "SupplierId" },
                values: new object[,]
                {
                    { 8, "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), 15m, new DateTime(2015, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2015, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 9, "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), 90m, new DateTime(2017, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 10, "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), 25m, new DateTime(2017, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SupplierRates",
                keyColumn: "SupplierRateId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "SupplierRates",
                keyColumn: "SupplierRateId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "SupplierRates",
                keyColumn: "SupplierRateId",
                keyValue: 10);
        }
    }
}
