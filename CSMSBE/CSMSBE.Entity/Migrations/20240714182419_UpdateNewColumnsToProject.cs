using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class UpdateNewColumnsToProject : Migration
    {
       
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "Model",
                schema: "csms",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),

                    SpeckleBranchId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProjectID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),

                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by = table.Column<string>(type: "text", nullable: false),
                    modified_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Model", x => x.id);
                    table.ForeignKey(
                        name: "FK_Model_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalSchema: "csms",
                        principalTable: "Project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<string>(
                name: "SpeckleProjectId",
                schema: "csms",
                table: "Project",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsPublic",
                schema: "csms",
                table: "Project",
                type: "boolean",
                nullable: true);
        }
        protected override void Down(MigrationBuilder migrationBuilder) { }

    }
}
