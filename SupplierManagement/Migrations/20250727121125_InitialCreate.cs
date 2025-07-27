using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SupplierManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedByUser = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "SupplierRates",
                columns: table => new
                {
                    SupplierRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RateEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUser = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierRates", x => x.SupplierRateId);
                    table.ForeignKey(
                        name: "FK_SupplierRates_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "Address", "CreatedByUser", "CreatedOn", "Name" },
                values: new object[,]
                {
                    { 1, "1, Main Street, The District, City1, XXX-AADA", "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), "BestValue" },
                    { 2, "2, Industrial Ave, Business Park, City2, YYY-BBBB", "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Quality Supplies" },
                    { 3, "3, Commerce St, Trade Center, City3, ZZZ-CCCC", "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Premium Partners" }
                });

            migrationBuilder.InsertData(
                table: "SupplierRates",
                columns: new[] { "SupplierRateId", "CreatedByUser", "CreatedOn", "Rate", "RateEndDate", "RateStartDate", "SupplierId" },
                values: new object[,]
                {
                    { 1, "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), 10m, new DateTime(2015, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), 20m, new DateTime(2015, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2015, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), 10m, new DateTime(2015, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2015, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 4, "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), 25m, null, new DateTime(2015, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 5, "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), 100m, null, new DateTime(2016, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 6, "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), 30m, new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 7, "admin.user", new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), 30m, null, new DateTime(2017, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRates_RateEndDate",
                table: "SupplierRates",
                column: "RateEndDate");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRates_RateStartDate",
                table: "SupplierRates",
                column: "RateStartDate");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRates_SupplierId",
                table: "SupplierRates",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Name",
                table: "Suppliers",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierRates");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
