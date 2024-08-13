using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class AlterCommentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                schema: "csms",
                table: "Comment",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentId",
                schema: "csms",
                table: "Comment",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_ParentId",
                schema: "csms",
                table: "Comment",
                column: "ParentId",
                principalSchema: "csms",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_ParentId",
                schema: "csms",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ParentId",
                schema: "csms",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "csms",
                table: "Comment");
        }
    }
}
