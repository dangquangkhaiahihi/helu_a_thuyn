using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CSMS.Entity.Migrations
{
    public partial class SeedTypeProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TypeProject",
                schema: "sys",
                columns: new[] { "id", "Name", "created_by", "created_date", "modified_by", "modified_date", "is_delete" },
                values: new object[,]
                {
                    { 1, "Dự án xây dựng", "ADMIN", DateTimeOffset.UtcNow, "ADMIN", DateTimeOffset.Now, false },
                    { 2, "Dự án đường ống nước", "ADMIN", DateTimeOffset.Now, "ADMIN", DateTimeOffset.UtcNow, false }
                },
                columnTypes: new[] { "int", "nvarchar(max)", "nvarchar(max)", "datetime2", "nvarchar(max)", "datetime2", "boolean" });
            migrationBuilder.InsertData(
               schema: "sys",
               table: "Province",
               columns: new[] { "id", "Name", "Code", "created_by", "created_date", "modified_by", "modified_date", "is_delete" },
               values: new object[,]
               {
                    { 1, "Province 1", "P1", "admin", DateTimeOffset.Now, "admin", DateTimeOffset.Now, false },
                    { 2, "Province 2", "P2", "admin", DateTimeOffset.Now, "admin", DateTimeOffset.Now, false },
               });
            migrationBuilder.InsertData(
                schema: "sys",
                table: "District",
                columns: new[] { "id", "name", "Code", "ProvinceId", "created_by", "created_date", "modified_by", "modified_date", "is_delete" },
                values: new object[,]
                {
                    { 1, "District 1", "D1", 1, "admin", DateTimeOffset.Now, "admin", DateTimeOffset.Now, false },
                    { 2, "District 2", "D2", 1, "admin", DateTimeOffset.Now, "admin", DateTimeOffset.Now, false },
                });
            migrationBuilder.InsertData(
               schema: "sys",
               table: "Commune",
               columns: new[] { "id", "Name", "Code", "DistrictId", "created_by", "created_date", "modified_by", "modified_date", "is_delete" },
               values: new object[,]
               {
                    { 1, "Commune 1", "C1", 1, "admin", DateTimeOffset.Now, "admin", DateTimeOffset.Now, false },
                    { 2, "Commune 2", "C2", 1, "admin", DateTimeOffset.Now, "admin", DateTimeOffset.Now, false },
               });
            migrationBuilder.Sql(
                @"INSERT INTO csms.""Project"" (""id"", ""Name"", ""Code"", ""Description"", ""TypeProjectId"", ""ProvinceId"", ""DistrictId"", ""CommuneId"", ""created_by"", ""created_date"", ""modified_by"", ""modified_date"", ""is_delete"")
                VALUES ('P1', 'Project 1', 'PRJ1', 'Description 1', 1, 1, 1, 1, 'admin', CURRENT_TIMESTAMP, 'admin', CURRENT_TIMESTAMP, FALSE);");

                        migrationBuilder.Sql(
                            @"INSERT INTO csms.""Project"" (""id"", ""Name"", ""Code"", ""Description"", ""TypeProjectId"", ""ProvinceId"", ""DistrictId"", ""CommuneId"", ""created_by"", ""created_date"", ""modified_by"", ""modified_date"", ""is_delete"")
                VALUES ('P2', 'Project 2', 'PRJ2', 'Description 2', 2, 2, 2, 2, 'admin', CURRENT_TIMESTAMP, 'admin', CURRENT_TIMESTAMP, FALSE);");
                    }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TypeProject",
                keyColumn: "id",
                keyValues: new object[] { 1, 2 });
        }
    }
}
