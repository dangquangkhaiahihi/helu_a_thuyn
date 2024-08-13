using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class UpdateEntityModelAndIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {         
            migrationBuilder.AlterColumn<string>(
                name: "SpeckleBranchId",
                schema: "csms",
                table: "Model",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "IsUpload",
                schema: "csms",
                table: "Model",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                schema: "csms",
                table: "Model",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewImg",
                schema: "csms",
                table: "Model",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpeckleBranchName",
                schema: "csms",
                table: "Model",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "csms",
                table: "Model",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Assignee",
                schema: "csms",
                table: "Issue",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "csms",
                table: "Issue",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
               name: "ModelId",
               schema: "csms",
               table: "Issue",
               type: "text",
               nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Model_ParentId",
                schema: "csms",
                table: "Model",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_ModelId",
                schema: "csms",
                table: "Issue",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                 name: "FK_Issue_Model_ModelId",
                 schema: "csms",
                 table: "Issue",
                 column: "ModelId",
                 principalSchema: "csms",
                 principalTable: "Model",
                 principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Model_Model_ParentId",
                schema: "csms",
                table: "Model",
                column: "ParentId",
                principalSchema: "csms",
                principalTable: "Model",
                principalColumn: "id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Model_ModelId",
                schema: "csms",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Model_Model_ParentId",
                schema: "csms",
                table: "Model");

            migrationBuilder.DropIndex(
                name: "IX_Model_ParentId",
                schema: "csms",
                table: "Model");

            migrationBuilder.DropIndex(
                name: "IX_Issue_ModelId",
                schema: "csms",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "IsUpload",
                schema: "csms",
                table: "Model");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "csms",
                table: "Model");

            migrationBuilder.DropColumn(
                name: "PreviewImg",
                schema: "csms",
                table: "Model");

            migrationBuilder.DropColumn(
                name: "SpeckleBranchName",
                schema: "csms",
                table: "Model");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "csms",
                table: "Model");

            migrationBuilder.DropColumn(
                name: "Assignee",
                schema: "csms",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "csms",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "ModelId",
                schema: "csms",
                table: "Issue");

            migrationBuilder.AlterColumn<string>(
                name: "SpeckleBranchId",
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
