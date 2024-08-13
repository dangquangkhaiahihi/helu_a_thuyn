using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class AddNewEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeProject",
                schema: "sys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by = table.Column<string>(type: "text", nullable: false),
                    modified_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeProject", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                schema: "sys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by = table.Column<string>(type: "text", nullable: false),
                    modified_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "District",
                schema: "sys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProvinceId = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by = table.Column<string>(type: "text", nullable: false),
                    modified_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.id);
                    table.ForeignKey(
                        name: "FK_District_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalSchema: "sys",
                        principalTable: "Province",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commune",
                schema: "sys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DistrictId = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by = table.Column<string>(type: "text", nullable: false),
                    modified_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commune", x => x.id);
                    table.ForeignKey(
                        name: "FK_Commune_District_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "sys",
                        principalTable: "District",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "csms",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TypeProjectId = table.Column<int>(type: "integer", nullable: false),
                    ProvinceId = table.Column<int>(type: "integer", nullable: false),
                    DistrictId = table.Column<int>(type: "integer", nullable: false),
                    CommuneId = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by = table.Column<string>(type: "text", nullable: false),
                    modified_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.id);
                    table.ForeignKey(
                        name: "FK_Project_Commune_CommuneId",
                        column: x => x.CommuneId,
                        principalSchema: "sys",
                        principalTable: "Commune",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_District_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "sys",
                        principalTable: "District",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_TypeProject_TypeProjectId",
                        column: x => x.TypeProjectId,
                        principalSchema: "sys",
                        principalTable: "TypeProject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalSchema: "sys",
                        principalTable: "Province",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commune_DistrictId",
                schema: "sys",
                table: "Commune",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_District_ProvinceId",
                schema: "sys",
                table: "District",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CommuneId",
                schema: "csms",
                table: "Project",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_DistrictId",
                schema: "csms",
                table: "Project",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProvinceId",
                schema: "csms",
                table: "Project",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_TypeProjectId",
                schema: "csms",
                table: "Project",
                column: "TypeProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project",
                schema: "csms");

            migrationBuilder.DropTable(
                name: "Commune",
                schema: "sys");

            migrationBuilder.DropTable(
                name: "TypeProject",
                schema: "sys");

            migrationBuilder.DropTable(
                name: "District",
                schema: "sys");

            migrationBuilder.DropTable(
                name: "Province",
                schema: "sys");
        }
    }
}
