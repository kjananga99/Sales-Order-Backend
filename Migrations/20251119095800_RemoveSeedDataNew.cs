using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SalesOrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedDataNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address1", "Address2", "Address3", "CreatedAt", "Name", "PostCode", "State", "Suburb", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "123 Main St", null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "John Doe", "62701", "IL", "Springfield", null },
                    { 2, "456 Oak Ave", null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jane Smith", "92501", "CA", "Riverside", null }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "ITEM001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Widget A", 10.50m, null },
                    { 2, "ITEM002", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Widget B", 25.00m, null },
                    { 3, "ITEM003", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gadget C", 15.75m, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
