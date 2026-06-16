using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ContractStatuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Signed" },
                    { 3, "Cancelled" }
                });

            migrationBuilder.InsertData(
                table: "SoftwareCategories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Sys" },
                    { 2, "Test" },
                    { 3, "Finanse" }
                });

            migrationBuilder.InsertData(
                table: "Softwares",
                columns: new[] { "SoftwareId", "BasePrice", "CategoryId", "Description", "LatestVersion", "Name" },
                values: new object[,]
                {
                    { 1, 5000.00m, 1, "Kompleksowy system", "3.2.1", "System Pro" },
                    { 2, 3000.00m, 2, "System testowy", "2.0.0", "TestMax" },
                    { 3, 2000.00m, 3, "Oprogramowanie finansowo-księgowe", "1.5.0", "FinanceTracker" }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "DiscountId", "DateFrom", "DateTo", "Name", "SoftwareId", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Promocja letnia", 1, 10.00m },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rabat na test", 2, 15.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractStatuses",
                keyColumn: "StatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ContractStatuses",
                keyColumn: "StatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ContractStatuses",
                keyColumn: "StatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SoftwareCategories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SoftwareCategories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SoftwareCategories",
                keyColumn: "CategoryId",
                keyValue: 2);
        }
    }
}
