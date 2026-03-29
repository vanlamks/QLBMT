# Nâng cấp giao diện TechZone

Các thay đổi chính:
- Làm mới màn hình đăng nhập theo phong cách hiện đại hơn.
- Làm mới form quản lý chính với tông màu đồng bộ, menu trái rõ ràng hơn.
- Trang chủ được thay bằng dashboard giới thiệu cửa hàng chuyên nghiệp hơn.
- Loại bỏ hoàn toàn các nội dung cũ mang tính đồ án sinh viên như tên sinh viên, giáo viên hướng dẫn, bài tập lớn.
- Thống nhất font chữ theo Segoe UI và phối màu tối hiện đại.
- Bổ sung cơ chế tô sáng menu đang được chọn.
- Cho phép bấm logo hoặc tiêu đề để quay lại trang chủ.

Các file đã chỉnh:
- `QuanLyCuaHangMayTinh/fManager.cs`
- `QuanLyCuaHangMayTinh/fManager.Designer.cs`
- `QuanLyCuaHangMayTinh/fLogin.cs`
- `QuanLyCuaHangMayTinh/fLogin.Designer.cs`


## Cập nhật mới về theme sáng
- Chuyển toàn bộ ứng dụng sang cơ chế theme sáng dùng chung thay vì giữ màu tối cố định trong từng form.
- Cho phép tùy chỉnh màu nền, màu menu, màu thẻ, màu chữ và màu nhấn ngay trong `QuanLyCuaHangMayTinh/App.config`.
- Các form con như danh mục, khách hàng, máy tính, nhà cung cấp, nhập/xuất, tài khoản, thống kê và báo cáo sẽ tự nhận theme khi mở.
- Chi tiết cách đổi màu được mô tả trong file `HUONG_DAN_THEME_SANG.md`.
