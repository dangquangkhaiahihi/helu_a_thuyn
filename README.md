# csms_g69

- Chạy postgres và pgadmin của speckle trên Docker
=> connect pgadmin với postgres

- Các bước setup DB: 
    + đứng ở root project => docker compose up -d
    + mở pgadmin, đăng nhập bằng tkhoan admin@gmail.com - 123456
    + ở sidebar của pgadmin => chuột phải vào "Servers" => "Register" => "Server"
    + ở tab đầu: name gì cũng đc
    + ở tab 2: 
        * Hostname/address: postgres
        * Port: 5432
        * Maintenance DB: speckle
        * Username: speckle
        * Password: speckle

- Chạy migration của BE:
    + Bật terminal ở path "Project/CSMSBE/CSMSBE.Entity"
    + Chạy lệnh: "dotnet ef database update"
- Chạy BE bằng visual studio

- Cài bun
- Cài node version 18.19.0 (bằng nvm hay cài rời đều đc)
- Chạy FE: cd CSMSFE => bun run dev (nếu chưa install thì chạy "bun install" trước)

- Tài khoản mặc định để đăng nhập: admin@gmail.com - 123@123aA
