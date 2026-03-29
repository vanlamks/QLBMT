# Hướng dẫn mở và chạy project bằng VS Code

## Những gì đã được nâng cấp
- Chuyển 3 project sang định dạng SDK-style để dễ mở và build hơn bằng toolchain .NET hiện nay.
- Nâng target framework từ .NET Framework 4.5 lên .NET Framework 4.8.1.
- Thay dependency ReportViewer cũ (tham chiếu tay bản 11) bằng NuGet package `Microsoft.ReportingServices.ReportViewerControl.Winforms`.
- Xóa dataset cũ không còn dùng và file license DevExpress thừa gây lỗi build trên máy mới.
- Sửa `DataProvider` để đọc chuỗi kết nối từ `App.config` thay vì hard-code.
- Sửa một số lỗi runtime ở tầng DAO (tính tổng hóa đơn, xóa tài khoản, lấy tổng tiền, thêm máy tính có tiếng Việt...).

## Yêu cầu môi trường
- Windows.
- VS Code bản mới.
- Extension **C#** hoặc **C# Dev Kit**.
- .NET SDK mới trên máy (khuyến nghị .NET 10 SDK; .NET 9 hoặc .NET 8 cũng dùng được).
- .NET Framework 4.8.1 Runtime trên Windows.
- SQL Server.

## Các bước chạy
1. Tạo mới database bằng file `database/QLCHMT.sql` (khuyến nghị) hoặc cập nhật lại các stored procedure nếu bạn đang dùng database cũ.
2. Nếu SQL Server của bạn không phải `.` thì sửa chuỗi kết nối trong `QuanLyCuaHangMayTinh/App.config`.
3. Mở thư mục project bằng VS Code.
4. Mở terminal trong VS Code và chạy:
   - `dotnet restore QuanLyCuaHangMayTinh.sln`
   - `dotnet build QuanLyCuaHangMayTinh.sln -c Debug`
5. Chạy file exe tại:
   - `QuanLyCuaHangMayTinh/bin/Debug/net481/QuanLyCuaHangMayTinh.exe`

## Gợi ý nếu gặp lỗi kết nối SQL
- Kiểm tra SQL Server service đã chạy.
- Kiểm tra database `QLCHMT` đã được tạo xong.
- Nếu dùng named instance, đổi `Data Source=.` thành tên instance thực tế.
- Nếu máy không dùng Integrated Security, sửa lại connection string theo user/password SQL Server.
