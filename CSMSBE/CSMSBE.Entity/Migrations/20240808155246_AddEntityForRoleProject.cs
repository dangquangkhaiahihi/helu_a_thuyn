using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class AddEntityForRoleProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                schema: "security_matrix_project",
                table: "RoleProjects",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "security_matrix_project",
                table: "ActionProjects",
                type: "text",
                nullable: true);

        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                schema: "security_matrix_project",
                table: "RoleProjects");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "security_matrix_project",
                table: "ActionProjects");
        }
    }
}
