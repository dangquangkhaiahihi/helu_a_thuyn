using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class AddDocumentAndFilExtensionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {        
            migrationBuilder.CreateTable(
                name: "Document",
                schema: "csms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsFile = table.Column<bool>(type: "boolean", nullable: false),
                    UrlPath = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: true),
                    FileExtension = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<string>(type: "text", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "text", nullable: true),
                    modified_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.id);
                    table.ForeignKey(
                        name: "FK_Document_Document_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "csms",
                        principalTable: "Document",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Document_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "csms",
                        principalTable: "Project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileExtensions",
                schema: "csms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileExtensions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Document_ParentId",
                schema: "csms",
                table: "Document",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_ProjectId",
                schema: "csms",
                table: "Document",
                column: "ProjectId");
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "Document",
                schema: "csms");

            migrationBuilder.DropTable(
                name: "FileExtensions",
                schema: "csms");

        }
    }
}
