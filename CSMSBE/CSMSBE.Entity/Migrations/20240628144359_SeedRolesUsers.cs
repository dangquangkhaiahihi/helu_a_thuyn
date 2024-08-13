using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class SeedRolesUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "Name", "NormalizedName" },
                values: new object[] {
                    "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f",
                    "admin",
                    "admin",
                    new DateTime(2024, 6, 28, 0, 0, 0, DateTimeKind.Utc),
                    "admin",
                    new DateTime(2024, 6, 28, 0, 0, 0, DateTimeKind.Utc),
                    "admin",
                    "ADMIN"
                });
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] {
                    "Id",
                    "FullName",
                    "DateOfBirth",
                    "Gender",
                    "Address",
                    "CreatedDate",
                    "ModifiedDate",
                    "CreatedBy",
                    "ModifiedBy",
                    "Status",
                    "Description",
                    "UserType",
                    "UserName",
                    "NormalizedUserName",
                    "Email",
                    "NormalizedEmail",
                    "EmailConfirmed",
                    "PasswordHash",
                    "SecurityStamp",
                    "ConcurrencyStamp",
                    "PhoneNumber",
                    "PhoneNumberConfirmed",
                    "TwoFactorEnabled",
                    "LockoutEnabled",
                    "AccessFailedCount",
                    "SecretKey"
                },
                values: new object[] {
                    "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f",
                    "ADMIN",
                    new DateTime(2024, 6, 28, 0, 0, 0, DateTimeKind.Utc),
                    true,
                    "string",
                    new DateTime(2024, 6, 28, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(2024, 6, 28, 0, 0, 0, DateTimeKind.Utc),
                    "ADMIN",
                    "ADMIN",
                    true,
                    "string",
                    "ADMINUSER",
                    "admin@gmail.com",
                    "ADMIN@GMAIL.COM",
                    "admin@gmail.com",
                    "ADMIN@GMAIL.COM",
                    true,
                    "AQAAAAEAACcQAAAAEG/dfhRDHzbbCvn4Dy7oyA+rrCRoSOIA/My+z5riUUe3LIBW0QLO4JUYLLOrfWGUfw==",
                    "O3YELAPEUDANLULSNR4BWYCLBW4KEYID",
                    "48b5940f-6836-4349-82a9-0d5e2ef26652",
                    "0559461826",
                    true,
                    false,
                    true,
                    0,
                    ""
                });
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] {
                    "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f",
                    "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f", "admin" });

            // Delete data from AspNetUsers
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f");

            // Delete data from AspNetRoles
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f");
        }
    }
}
