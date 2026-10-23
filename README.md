# Quản lý lương nhân viên

Bài tập lớn học phần Lập trình Web, Nhóm 6, lớp N07, Khoa Công nghệ thông tin, Trường Đại học Giao thông vận tải.

Giảng viên: Lại Mạnh Dũng.

## Thành viên và phân công

| Thành viên | Mã sinh viên | Phần việc phụ trách |
| --- | --- | --- |
| Lương Xuân Dũng | v624006 | Cơ sở dữ liệu và chức năng quản lý phòng ban |
| Cao Minh Huy | v624176 | Quản lý nhân viên và kiểm tra thông tin nhân viên |
| Nguyễn Đức Mạnh, nhóm trưởng | v624017 | Quản lý bảng lương, tính lương, tích hợp và tổng hợp báo cáo |
| Trần Minh Quang | v624186 | Đăng nhập, đăng xuất, phân quyền và xem lương cá nhân |

## Chức năng

- Admin thêm, sửa, xóa phòng ban, nhân viên và bảng lương.
- Tính thực lĩnh theo ngày công, phụ cấp, thưởng và khấu trừ.
- Kiểm tra trùng mã nhân viên và trùng bảng lương theo nhân viên, tháng, năm.
- Không xóa phòng còn nhân viên; không xóa nhân viên có bảng lương hoặc tài khoản.
- Đăng nhập bằng cookie, có sử dụng session và hai quyền Admin, NhanVien.
- Nhân viên chỉ xem được bảng lương gắn với tài khoản của mình.

## Công nghệ

C#, ASP.NET Core MVC, .NET 10, Entity Framework Core 10.0.13, SQL Server Express, Razor, Bootstrap và jQuery.

## Cách chạy

1. Chuẩn bị Visual Studio hỗ trợ .NET 10, workload ASP.NET and web development, .NET 10 SDK, SQL Server Express và SSMS.
2. Kết nối SQL Server trong SSMS. Trên môi trường mới, chạy `Quanlyluong/Quanlyluong.sql` để tạo database `QuanLyLuong` và bốn bảng. Script có lệnh CREATE nên không chạy lại toàn bộ khi database và các bảng đã tồn tại.
3. Mở `Quanlyluong.slnx` trong Visual Studio và khôi phục các gói NuGet.
4. Sửa `ConnectionStrings:KetNoi` trong `Quanlyluong/appsettings.json` cho đúng SQL Server trên máy. Bản mẫu dùng `localhost\SQLEXPRESS`, Windows Authentication và database `Quanlyluong`.
5. Build và chạy bằng F5, chọn profile https. Theo cấu hình mẫu, trang chạy tại `https://localhost:7250`.
6. Vào `/Taikhoan/DangNhap` để đăng nhập.

`TrustServerCertificate=True` trong cấu hình phục vụ SQL Server cục bộ. Nếu triển khai môi trường khác cần cấu hình chứng chỉ và kết nối phù hợp.

## Tài khoản chạy thử

| Tên đăng nhập | Mật khẩu mẫu | Quyền |
| --- | --- | --- |
| admin | 123456 | Admin |
| nv01 | 123456 | Nhân viên |

Ứng dụng tạo tài khoản admin khi chưa tồn tại. Tài khoản nv01 chỉ được tạo khi có ít nhất một nhân viên, và được gắn với nhân viên có Id nhỏ nhất. Với database mới, đăng nhập admin, thêm nhân viên rồi khởi động lại ứng dụng để tạo nv01. Mật khẩu được lưu dưới dạng đã băm; các mật khẩu trên chỉ dùng cho bài demo.

## Công thức tính

```text
Thực lĩnh = Lương cơ bản / 26 × Số ngày công + Phụ cấp + Thưởng - Khấu trừ
```

Ngày công là số nguyên từ 0 đến 26. Kết quả làm tròn đến đồng. Lương cơ bản và phụ cấp được chép vào bảng lương lúc lập, giữ nguyên khi sửa để không làm thay đổi mức lương đã lưu của kỳ trước.

## Phạm vi

Bài hiện tập trung vào CRUD, tính lương và đăng nhập hai quyền. Chưa có phân trang, lọc dữ liệu, AJAX, REST API, nhiều ngôn ngữ, thuế, bảo hiểm, tăng ca hay chức năng quản lý tài khoản trên giao diện. Cần tiếp tục rà soát thư viện và kiểm thử trước khi sử dụng thực tế.

## Quản lý công việc

Bảng GitHub Projects của nhóm được dùng để tổng hợp các phần đã làm và những việc cần kiểm thử, hoàn thiện trước khi nộp.
