using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class CreateSecurityMatrixProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "security_matrix_project");

            migrationBuilder.CreateTable(
                name: "ActionProjects",
                schema: "security_matrix_project",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleProjects",
                schema: "security_matrix_project",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "text", nullable: true),
                    modified_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUserRoles",
                schema: "security_matrix_project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUserRoles_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "csms",
                        principalTable: "Project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUserRoles_RoleProjects_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "security_matrix_project",
                        principalTable: "RoleProjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SecurityMatrixProjects",
                schema: "security_matrix_project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ActionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityMatrixProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityMatrixProjects_ActionProjects_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "security_matrix_project",
                        principalTable: "ActionProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityMatrixProjects_RoleProjects_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "security_matrix_project",
                        principalTable: "RoleProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRoles_ProjectId",
                schema: "security_matrix_project",
                table: "ProjectUserRoles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRoles_RoleId",
                schema: "security_matrix_project",
                table: "ProjectUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRoles_UserId",
                schema: "security_matrix_project",
                table: "ProjectUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityMatrixProjects_ActionId",
                schema: "security_matrix_project",
                table: "SecurityMatrixProjects",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityMatrixProjects_RoleId",
                schema: "security_matrix_project",
                table: "SecurityMatrixProjects",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUserRoles",
                schema: "security_matrix_project");

            migrationBuilder.DropTable(
                name: "SecurityMatrixProjects",
                schema: "security_matrix_project");

            migrationBuilder.DropTable(
                name: "ActionProjects",
                schema: "security_matrix_project");

            migrationBuilder.DropTable(
                name: "RoleProjects",
                schema: "security_matrix_project");
        }
    }
}
