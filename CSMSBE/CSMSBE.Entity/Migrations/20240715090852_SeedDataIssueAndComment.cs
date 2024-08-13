using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class SeedDataIssueAndComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               schema: "csms",
               table: "Issue",
               columns: new[] { "Id", "Name", "Type", "Status", "Description", "UserId", "ProjectId", "created_by", "created_date", "modified_by", "modified_date", "is_delete" },
               values: new object[,]
               {
                    { 1, "Issue 1", "Type 1", "Open", "Description 1", "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f", "P1", "Admin", DateTimeOffset.Now, "Admin", DateTimeOffset.Now, false },
                    { 2, "Issue 2", "Type 2", "Closed", "Description 2", "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f", "P1", "Admin", DateTimeOffset.Now, "Admin", DateTimeOffset.Now, false },
                    { 3, "Issue 3", "Type 3", "InProgress", "Description 3", "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f", "P2", "Admin", DateTimeOffset.Now, "Admin", DateTimeOffset.Now, false }
               });

            migrationBuilder.InsertData(
                schema: "csms",
                table: "Comment",
                columns: new[] { "Id", "Content", "IssueId", "UserId", "created_by", "created_date", "modified_by", "modified_date", "is_delete" },
                values: new object[,]
                {
                    { 1, "Comment 1 for Issue 1", 1, "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f", "admin", DateTimeOffset.Now, "admin", DateTimeOffset.Now, false },
                    { 2, "Comment 2 for Issue 1", 1, "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f", "admin", DateTimeOffset.Now, "admin", DateTimeOffset.Now, false },
                    { 3, "Comment 1 for Issue 2", 2, "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f", "admin", DateTimeOffset.Now, "admin", DateTimeOffset.Now, false },
                    { 4, "Comment 1 for Issue 3", 3, "aa994fe3-2bf9-4c8e-bccc-339aa09b6e3f", "admin", DateTimeOffset.Now, "admin", DateTimeOffset.Now, false }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
               schema: "csms",
               table: "Issue",
               keyColumn: "Id",
               keyValues: new object[] { 1, 2, 3 });

            migrationBuilder.DeleteData(
                schema: "csms",
                table: "Comment",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4 });
        }
    }
}
