using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Remood_Place.Migrations
{
    /// <inheritdoc />
    public partial class Initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ac8afe1-a44a-45dd-ac2d-9e0b27297f5b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e5a1670-cbe7-4b4d-be6d-456c762f5b83");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4bc8eb89-4129-40f9-a625-a44fc30b1e6d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05f2494f-be0d-4d1f-90e4-5f9aa54d749c", "56502fd5-eee4-4efc-9ed3-52c48f657a5d", "Admin", "admin" },
                    { "9772dbfb-d5f0-4195-95f1-f3dc084e39d8", "74926676-aed9-46c2-a17a-48c22c9832e5", "SuperAdmin", "SuperAdmin" },
                    { "d59c4061-8880-4867-90fa-fd4386edc1ab", "049ea24a-8d33-4333-9b63-dd48b66b733e", "User", "user" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05f2494f-be0d-4d1f-90e4-5f9aa54d749c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9772dbfb-d5f0-4195-95f1-f3dc084e39d8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d59c4061-8880-4867-90fa-fd4386edc1ab");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ac8afe1-a44a-45dd-ac2d-9e0b27297f5b", "a5479016-f4f4-427d-a9a3-469ecbdc2f59", "SuperAdmin", "SuperAdmin" },
                    { "1e5a1670-cbe7-4b4d-be6d-456c762f5b83", "2bf95587-709b-48c7-a1b7-2f32e4fba33a", "Admin", "admin" },
                    { "4bc8eb89-4129-40f9-a625-a44fc30b1e6d", "d5f31354-7eac-4519-92a3-a76a18598c11", "User", "user" }
                });
        }
    }
}
