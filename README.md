# csms_g69

- Chạy postgres và pgadmin của speckle trên Docker
=> connect pgadmin với postgres
=> vào postgres thông qua pgadmin tạo DB csms (nếu chưa tạo)
- Sử dụng connection string trong BE hiện tại
- Chạy BE bằng visual studio
- Chạy FE: cd CSMSFE => bun run dev (nếu chưa install thì chạy "bun install" trước)


- Hướng dẫn chạy migration để update DB:
Bật terminal ở path "Project/CSMSBE/CSMSBE.Entity"
Bật DB lên (docker) + Xóa DB "csms" cũ + Tạo DB "csms" trống
Chạy lệnh: "dotnet ef database update"
- Tài khoản mặc định: admin@gmail.com - 123@123aA