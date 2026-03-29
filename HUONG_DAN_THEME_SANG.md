# Hướng dẫn tùy chỉnh giao diện sáng

Project đã được chuyển sang cơ chế theme sáng dùng chung.

## Cách đổi màu nhanh
Mở file:

`QuanLyCuaHangMayTinh/App.config`

Tại phần `<appSettings>`, bạn có thể đổi các mã màu theo nhu cầu:

- `Theme.ShellColor`: nền thanh trên cùng
- `Theme.NavigationColor`: nền menu bên trái
- `Theme.NavigationHoverColor`: màu rê chuột menu
- `Theme.NavigationActiveColor`: màu menu đang chọn
- `Theme.PageColor`: nền vùng nội dung chính
- `Theme.CardColor`: nền các khối/thẻ và panel
- `Theme.InputColor`: nền ô nhập liệu
- `Theme.AccentColor`: màu nhấn chính cho nút chính/tiêu đề
- `Theme.AccentSoftColor`: màu nhấn phụ
- `Theme.TextPrimary`: màu chữ chính
- `Theme.TextSecondary`: màu chữ phụ
- `Theme.BorderColor`: màu viền control
- `Theme.GridAlternateRowColor`: màu dòng xen kẽ trong bảng

## Định dạng màu hỗ trợ
- `#RRGGBB` ví dụ `#F8FAFC`
- `R,G,B` ví dụ `248,250,252`

## Gợi ý nếu muốn sáng hơn nữa
- `Theme.PageColor = #FFFFFF`
- `Theme.NavigationColor = #F1F5F9`
- `Theme.CardColor = #FFFFFF`
- `Theme.AccentColor = #0F62FE`
