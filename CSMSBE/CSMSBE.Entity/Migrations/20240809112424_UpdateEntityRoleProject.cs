using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class UpdateEntityRoleProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
                name: "ProjectId",
                schema: "security_matrix_project",
                table: "RoleProjects",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                schema: "security_matrix_project",
                table: "ProjectUserRoles",
                type: "boolean",
                nullable: true);

           
            migrationBuilder.AddForeignKey(
                name: "FK_RoleProjects_Project_ProjectId",
                schema: "security_matrix_project",
                table: "RoleProjects",
                column: "ProjectId",
                principalSchema: "csms",
                principalTable: "Project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleProjects_Project_ProjectId",
                schema: "security_matrix_project",
                table: "RoleProjects");

            migrationBuilder.DropIndex(
                name: "IX_RoleProjects_ProjectId",
                schema: "security_matrix_project",
                table: "RoleProjects");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                schema: "security_matrix_project",
                table: "RoleProjects");

            migrationBuilder.DropColumn(
                name: "IsPending",
                schema: "security_matrix_project",
                table: "ProjectUserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                schema: "csms",
                table: "Model",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
