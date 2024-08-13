using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class SeedRoleAndSercurityMatrix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*START SEED Screen*/
            migrationBuilder.InsertData(
                table: "Screen",
                schema: "sys",
                columns: new[] { "parent_id", "code", "name", "title", "icon", "order" },
                values: new object[] {
                    1,
                    "Admin",
                    "Admin",
                    "Admin",
                    "Admin",
                    1
                });

            migrationBuilder.InsertData(
                table: "Screen",
                schema: "sys",
                columns: new[] { "parent_id", "code", "name", "title", "icon", "order" },
                values: new object[] {
                    1,
                    "SECURITY_MATRIX",
                    "SECURITY_MATRIX",
                    "SECURITY_MATRIX",
                    "SECURITY_MATRIX",
                    2
                });

            migrationBuilder.InsertData(
                table: "Screen",
                schema: "sys",
                columns: new[] { "parent_id", "code", "name", "title", "icon", "order" },
                values: new object[] {
                    1,
                    "ROLE_MANAGEMENT",
                    "ROLE_MANAGEMENT",
                    "ROLE_MANAGEMENT",
                    "ROLE_MANAGEMENT",
                    3
                });

            migrationBuilder.InsertData(
                table: "Screen",
                schema: "sys",
                columns: new[] {"parent_id", "code", "name", "title", "icon", "order" },
                values: new object[] {
                    1,
                    "USER_MANAGEMENT",
                    "USER_MANAGEMENT",
                    "USER_MANAGEMENT",
                    "USER_MANAGEMENT",
                    4
                });

            /*START SEED Action*/
            migrationBuilder.InsertData(
                table: "Action",
                schema: "sys",
                columns: new[] { "code", "name" },
                values: new object[] {
                    "CREATE",
                    "CREATE"
                });

            migrationBuilder.InsertData(
                table: "Action",
                schema: "sys",
                columns: new[] {"code", "name" },
                values: new object[] {
                    "UPDATE",
                    "UPDATE"
                });

            migrationBuilder.InsertData(
                table: "Action",
                schema: "sys",
                columns: new[] { "code", "name" },
                values: new object[] {
                    "DELETE",
                    "DELETE"
                });

            migrationBuilder.InsertData(
                table: "Action",
                schema: "sys",
                columns: new[] { "code", "name" },
                values: new object[] {
                    "VIEW",
                    "VIEW"
                });

            /*START SEED Screen*/
            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                    1, 1, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                    2, 1, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                     1, 2, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] {  "action_id", "screen_id", "role_id" },
                values: new object[] {
                    2, 2, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                    3, 2, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                    4, 2, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                    1, 3, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                    2, 3, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                   3, 3, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                    4, 3, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                    1, 4, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                  2, 4, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                     3, 4, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });

            migrationBuilder.InsertData(
                table: "SecurityMatrix",
                schema: "sys",
                columns: new[] { "action_id", "screen_id", "role_id" },
                values: new object[] {
                    4, 4, "bb994fe3-2bf9-4c8e-bccc-339aa09b6e3f"
                });
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            // Remove data from SecurityMatrix
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 1
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 2
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 3
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 4
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 5
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 6
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 7
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 8
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 9
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 10
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 11
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 12
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 13
            );
            migrationBuilder.DeleteData(
                table: "SecurityMatrix",
                schema: "sys",
                keyColumn: "id",
                keyValue: 14
            );

            // Remove data from Screen
            migrationBuilder.DeleteData(
                table: "Screen",
                schema: "sys",
                keyColumn: "id",
                keyValue: 1
            );
            migrationBuilder.DeleteData(
                table: "Screen",
                schema: "sys",
                keyColumn: "id",
                keyValue: 2
            );
            migrationBuilder.DeleteData(
                table: "Screen",
                schema: "sys",
                keyColumn: "id",
                keyValue: 3
            );
            migrationBuilder.DeleteData(
                table: "Screen",
                schema: "sys",
                keyColumn: "id",
                keyValue: 4
            );

            // Remove data from Action
            migrationBuilder.DeleteData(
                table: "Action",
                schema: "sys",
                keyColumn: "id",
                keyValue: 1
            );
            migrationBuilder.DeleteData(
                table: "Action",
                schema: "sys",
                keyColumn: "id",
                keyValue: 2
            );
            migrationBuilder.DeleteData(
                table: "Action",
                schema: "sys",
                keyColumn: "id",
                keyValue: 3
            );
            migrationBuilder.DeleteData(
                table: "Action",
                schema: "sys",
                keyColumn: "id",
                keyValue: 4
            );
        }

    }
}
