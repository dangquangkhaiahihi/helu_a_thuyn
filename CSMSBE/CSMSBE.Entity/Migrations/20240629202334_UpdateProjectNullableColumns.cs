using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class UpdateProjectNullableColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Commune_CommuneId",
                schema: "csms",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_District_DistrictId",
                schema: "csms",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Province_ProvinceId",
                schema: "csms",
                table: "Project");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinceId",
                schema: "csms",
                table: "Project",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                schema: "csms",
                table: "Project",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CommuneId",
                schema: "csms",
                table: "Project",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Commune_CommuneId",
                schema: "csms",
                table: "Project",
                column: "CommuneId",
                principalSchema: "sys",
                principalTable: "Commune",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_District_DistrictId",
                schema: "csms",
                table: "Project",
                column: "DistrictId",
                principalSchema: "sys",
                principalTable: "District",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Province_ProvinceId",
                schema: "csms",
                table: "Project",
                column: "ProvinceId",
                principalSchema: "sys",
                principalTable: "Province",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Commune_CommuneId",
                schema: "csms",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_District_DistrictId",
                schema: "csms",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Province_ProvinceId",
                schema: "csms",
                table: "Project");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinceId",
                schema: "csms",
                table: "Project",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                schema: "csms",
                table: "Project",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommuneId",
                schema: "csms",
                table: "Project",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Commune_CommuneId",
                schema: "csms",
                table: "Project",
                column: "CommuneId",
                principalSchema: "sys",
                principalTable: "Commune",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_District_DistrictId",
                schema: "csms",
                table: "Project",
                column: "DistrictId",
                principalSchema: "sys",
                principalTable: "District",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Province_ProvinceId",
                schema: "csms",
                table: "Project",
                column: "ProvinceId",
                principalSchema: "sys",
                principalTable: "Province",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
