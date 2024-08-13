using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class SeedDataFileExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "csms",
                table: "FileExtensions",
                columns: new[] { "id", "Name" },
                values: new object[,]
                {
                    { 1, ".jpg" },
                    { 2, ".png" },
                    { 3, ".gif" },
                    { 4, ".jpeg" },
                    { 5, ".tiff" },
                    { 6, ".docx" },
                    { 7, ".doc" },
                    { 8, ".xls" },
                    { 9, ".xlsx" },
                    { 10, ".pdf" },
                    { 11, ".txt" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "FileExtensions",
                keyColumn: "id",
                keyValue: 11);
        }
    }
}
