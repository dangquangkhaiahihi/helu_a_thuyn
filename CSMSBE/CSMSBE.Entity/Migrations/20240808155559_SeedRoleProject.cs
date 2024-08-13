using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSMS.Entity.Migrations
{
    public partial class SeedRoleProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm dữ liệu vào bảng ActionProject
            migrationBuilder.Sql("INSERT INTO security_matrix_project.\"ActionProjects\" (\"Id\", \"Code\", \"Name\") VALUES ('A1','EDIT_PROJECT', 'Chỉnh sửa dự án')");
            migrationBuilder.Sql("INSERT INTO security_matrix_project.\"ActionProjects\" (\"Id\", \"Code\", \"Name\") VALUES ('A2', 'INVITE','Mời thành viên')");
            migrationBuilder.Sql("INSERT INTO security_matrix_project.\"ActionProjects\" (\"Id\", \"Code\", \"Name\") VALUES ('A3', 'EDIT_RESOURCE', 'Chỉnh sửa tài nguyên')");
            migrationBuilder.Sql("INSERT INTO security_matrix_project.\"ActionProjects\" (\"Id\", \"Code\", \"Name\") VALUES ('A4','VIEW_RESOURCE', 'Xem tài nguyên')");
            migrationBuilder.Sql("INSERT INTO security_matrix_project.\"ActionProjects\" (\"Id\", \"Code\", \"Name\") VALUES ('A5','VIEW_ALL_RESOURCE', 'Xem tất cả tài nguyên')");
            migrationBuilder.Sql("INSERT INTO security_matrix_project.\"ActionProjects\" (\"Id\", \"Code\", \"Name\") VALUES ('A6','REVIEW_RESOURCE', 'Phê duyệt tài nguyên')");

        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM security_matrix_project.\"ActionProject\" WHERE Id IN ('EDIT_PROJECT', 'INVITE', 'EDIT_RESOURCE', 'VIEW_RESOURCE', 'VIEW_ALL_RESOURCE', 'REVIEW_RESOURCE')");
        }
    }
}
