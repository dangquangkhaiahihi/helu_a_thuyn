using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class UpdateTableAndDataProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE csms.\"Project\" SET \"SpeckleProjectId\"  = 'ID1' WHERE \"SpeckleProjectId\" IS NULL");
            migrationBuilder.Sql("UPDATE csms.\"Project\" SET \"IsPublic\" = true WHERE \"IsPublic\" IS NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
