using QuanLyCuaHangMayTinh.DAO;
using QuanLyCuaHangMayTinh.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCuaHangMayTinh
{
    public partial class fManager : Form
    {
        private readonly ThemePalette theme = AppTheme.Current;

        private TaiKhoan acc;
        private bool isReady = true;
        private Button currentNavButton;

        public fManager(TaiKhoan acc)
        {
            this.acc = acc;
            InitializeComponent();

            ApplyShellTheme();
            ConfigureNavigation();
            ShowHomeDashboard();

            if (!IsAdmin())
            {
                this.pnAdmin.Visible = false;
            }

            timer1.Stop();
        }

        private bool IsAdmin()
        {
            return string.Equals(acc.LoaiTaiKhoan, "admin", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(acc.LoaiTaiKhoan, "0", StringComparison.OrdinalIgnoreCase);
        }

        private string GetRoleDisplayName()
        {
            return IsAdmin() ? "Quản trị viên" : "Nhân viên";
        }

        private string GetDisplayName()
        {
            return string.IsNullOrWhiteSpace(acc.TenHienThi) ? acc.UserName : acc.TenHienThi;
        }

        private void ApplyShellTheme()
        {
            Font baseFont = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Font headingFont = new Font("Segoe UI Semibold", 17F, FontStyle.Bold, GraphicsUnit.Point, 0);

            BackColor = theme.ShellColor;
            ForeColor = theme.TextPrimary;
            Font = baseFont;
            Text = "TechZone Manager";
            AcceptButton = null;
            MinimumSize = new Size(1100, 700);

            pn.BackColor = theme.ShellColor;
            pn.Height = 62;

            pictureBox1.Size = new Size(58, 52);
            pictureBox1.Location = new Point(18, 5);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Click += HomeShortcut_Click;

            lbl.Text = "TECHZONE | Quản lý cửa hàng máy tính";
            lbl.Font = headingFont;
            lbl.ForeColor = theme.AccentColor;
            lbl.BackColor = theme.ShellColor;
            lbl.AutoSize = true;
            lbl.Location = new Point(94, 15);
            lbl.Cursor = Cursors.Hand;
            lbl.Click += HomeShortcut_Click;

            StyleWindowButton(btnMinimum, "—");
            StyleWindowButton(btnThoat, "✕");
            btnMinimum.Location = new Point(ClientSize.Width - 92, 10);
            btnThoat.Location = new Point(ClientSize.Width - 46, 10);
            btnMinimum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnThoat.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            panel1.BackColor = theme.NavigationColor;
            panel1.Width = 235;

            pnContent.Dock = DockStyle.Fill;
            pnContent.Location = new Point(panel1.Right, pn.Bottom);
            pnContent.Padding = new Padding(24);
            pnContent.BackColor = theme.PageColor;

            label1.Visible = false;
            panel2.Visible = false;
            lblChayChu.Visible = false;
        }

        private void StyleWindowButton(Button button, string text)
        {
            button.Text = text;
            button.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            button.ForeColor = theme.TextSecondary;
            button.BackColor = theme.NavigationColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Size = new Size(36, 36);

            button.MouseEnter += delegate
            {
                button.BackColor = button == btnThoat ? Color.FromArgb(180, 50, 50) : theme.NavigationHoverColor;
                button.ForeColor = theme.TextPrimary;
            };

            button.MouseLeave += delegate
            {
                button.BackColor = theme.NavigationColor;
                button.ForeColor = theme.TextSecondary;
            };
        }

        private void ConfigureNavigation()
        {
            btnDangXuat.Text = "Đăng xuất";
            btnThongTinCaNhan.Text = "Thông tin cá nhân";
            btnNhap.Text = "Nhập hàng";
            btnXuat.Text = "Xuất hàng";
            btnKhachHang.Text = "Khách hàng";
            btnNhaCungCap.Text = "Nhà cung cấp";
            btnMayTinh.Text = "Máy tính";
            txtDanhMuc.Text = "Danh mục";
            btnThongKe.Text = "Thống kê";
            btnTaiKhoan.Text = "Tài khoản";

            List<Button> mainButtons = new List<Button>
            {
                btnDangXuat,
                btnThongTinCaNhan,
                btnNhap,
                btnXuat,
                btnKhachHang,
                btnNhaCungCap,
                btnMayTinh,
                txtDanhMuc
            };

            for (int i = 0; i < mainButtons.Count; i++)
            {
                StyleNavButton(mainButtons[i], 20, 14 + (i * 56));
            }

            pnAdmin.Location = new Point(0, 14 + (mainButtons.Count * 56) + 12);
            pnAdmin.Size = new Size(panel1.Width, 120);
            pnAdmin.BackColor = theme.NavigationColor;
            StyleNavButton(btnThongKe, 20, 0);
            StyleNavButton(btnTaiKhoan, 20, 56);
        }

        private void StyleNavButton(Button button, int left, int top)
        {
            button.Left = left;
            button.Top = top;
            button.Width = panel1.Width - 40;
            button.Height = 46;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = theme.NavigationColor;
            button.ForeColor = theme.TextPrimary;
            button.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(18, 0, 0, 0);
            button.Cursor = Cursors.Hand;

            button.MouseEnter -= NavigationButton_MouseEnter;
            button.MouseLeave -= NavigationButton_MouseLeave;
            button.MouseEnter += NavigationButton_MouseEnter;
            button.MouseLeave += NavigationButton_MouseLeave;
        }

        private void NavigationButton_MouseEnter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button != currentNavButton)
            {
                button.BackColor = theme.NavigationHoverColor;
            }
        }

        private void NavigationButton_MouseLeave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button != currentNavButton)
            {
                button.BackColor = theme.NavigationColor;
            }
        }

        private void ActivateNavButton(Button button)
        {
            if (currentNavButton != null)
            {
                currentNavButton.BackColor = theme.NavigationColor;
                currentNavButton.ForeColor = theme.TextPrimary;
            }

            currentNavButton = button;

            if (currentNavButton != null)
            {
                currentNavButton.BackColor = theme.NavigationActiveColor;
                currentNavButton.ForeColor = theme.AccentColor;
            }
        }

        private void ShowHomeDashboard()
        {
            ActivateNavButton(null);
            pnContent.Controls.Clear();
            pnContent.BackColor = theme.PageColor;

            Panel dashboard = new Panel();
            dashboard.Dock = DockStyle.Fill;
            dashboard.BackColor = theme.PageColor;
            dashboard.Padding = new Padding(0);

            Panel heroCard = CreateCard(new Rectangle(8, 10, 736, 172));
            heroCard.Controls.Add(CreateLabel("TECHZONE COMPUTER STORE", 24, FontStyle.Bold, theme.AccentColor, new Point(28, 26), new Size(440, 38)));
            heroCard.Controls.Add(CreateLabel("Giải pháp quản lý bán hàng, kho hàng và chăm sóc khách hàng cho cửa hàng công nghệ.", 11.5F, FontStyle.Regular, theme.TextSecondary, new Point(28, 68), new Size(520, 52)));
            heroCard.Controls.Add(CreateLabel("• Quy trình nhập - xuất rõ ràng\r\n• Quản lý sản phẩm, danh mục, nhà cung cấp\r\n• Theo dõi khách hàng, tài khoản và báo cáo", 11F, FontStyle.Regular, theme.TextPrimary, new Point(28, 118), new Size(430, 60)));
            heroCard.Controls.Add(CreateBadge($"Xin chào, {GetDisplayName()}", new Point(530, 28), new Size(176, 38), theme.AccentColor));
            heroCard.Controls.Add(CreateBadge(GetRoleDisplayName(), new Point(530, 76), new Size(176, 34), theme.AccentSoftColor));
            heroCard.Controls.Add(CreateMiniStat("Sản phẩm", "Laptop • PC • Linh kiện", new Point(530, 120)));

            Panel introCard = CreateCard(new Rectangle(8, 200, 356, 305));
            introCard.Controls.Add(CreateLabel("Giới thiệu cửa hàng", 16F, FontStyle.Bold, theme.TextPrimary, new Point(22, 18), new Size(260, 28)));
            introCard.Controls.Add(CreateLabel(
                "TechZone hướng tới trải nghiệm bán lẻ công nghệ hiện đại: sản phẩm chính hãng, quy trình rõ ràng, dịch vụ tận tâm và số liệu minh bạch.\r\n\r\n" +
                "Hệ thống phù hợp để quản lý các nghiệp vụ hằng ngày như nhập hàng, bán hàng, tồn kho, khách hàng và báo cáo doanh thu.",
                11F,
                FontStyle.Regular,
                theme.TextSecondary,
                new Point(22, 58),
                new Size(310, 118)));
            introCard.Controls.Add(CreateSectionTitle("Điểm nổi bật", new Point(22, 184)));
            introCard.Controls.Add(CreateLabel(
                "• Giao diện gọn gàng, dễ thao tác\r\n" +
                "• Phân quyền tài khoản rõ ràng\r\n" +
                "• Báo cáo hỗ trợ theo dõi hoạt động bán hàng\r\n" +
                "• Dữ liệu lưu trữ trên SQL Server",
                10.8F,
                FontStyle.Regular,
                theme.TextPrimary,
                new Point(22, 216),
                new Size(310, 82)));

            Panel featureCard = CreateCard(new Rectangle(388, 200, 356, 305));
            featureCard.Controls.Add(CreateLabel("Chức năng chính", 16F, FontStyle.Bold, theme.TextPrimary, new Point(22, 18), new Size(220, 28)));
            featureCard.Controls.Add(CreateFeatureItem("Quản lý nhập hàng", "Lập hóa đơn nhập, cập nhật tồn kho, liên kết nhà cung cấp.", new Point(22, 60)));
            featureCard.Controls.Add(CreateFeatureItem("Quản lý xuất hàng", "Xử lý bán hàng, lưu lịch sử giao dịch và thông tin khách hàng.", new Point(22, 122)));
            featureCard.Controls.Add(CreateFeatureItem("Sản phẩm & danh mục", "Theo dõi máy tính, danh mục, giá nhập và giá bán.", new Point(22, 184)));
            featureCard.Controls.Add(CreateFeatureItem("Báo cáo & thống kê", "Hỗ trợ tổng hợp dữ liệu giúp theo dõi hoạt động kinh doanh.", new Point(22, 246)));

            Label footer = CreateLabel(
                "Mục tiêu của TechZone là vận hành cửa hàng nhanh hơn, trực quan hơn và chuyên nghiệp hơn trong từng giao dịch.",
                10.5F,
                FontStyle.Italic,
                theme.TextSecondary,
                new Point(12, 524),
                new Size(740, 22));
            footer.TextAlign = ContentAlignment.MiddleCenter;

            dashboard.Controls.Add(heroCard);
            dashboard.Controls.Add(introCard);
            dashboard.Controls.Add(featureCard);
            dashboard.Controls.Add(footer);
            pnContent.Controls.Add(dashboard);
        }

        private Panel CreateCard(Rectangle bounds)
        {
            Panel card = new Panel();
            card.BackColor = theme.CardColor;
            card.Location = bounds.Location;
            card.Size = bounds.Size;
            return card;
        }

        private Label CreateLabel(string text, float fontSize, FontStyle fontStyle, Color color, Point location, Size size)
        {
            Label label = new Label();
            label.Text = text;
            label.Font = new Font("Segoe UI", fontSize, fontStyle);
            label.ForeColor = color;
            label.Location = location;
            label.Size = size;
            label.BackColor = Color.Transparent;
            return label;
        }

        private Label CreateSectionTitle(string text, Point location)
        {
            return CreateLabel(text, 12.2F, FontStyle.Bold, theme.AccentSoftColor, location, new Size(180, 24));
        }

        private Panel CreateBadge(string text, Point location, Size size, Color color)
        {
            Panel badge = new Panel();
            badge.BackColor = AppTheme.Lighten(theme.NavigationColor, 0.18f);
            badge.Location = location;
            badge.Size = size;

            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            label.ForeColor = color;

            badge.Controls.Add(label);
            return badge;
        }

        private Panel CreateMiniStat(string title, string subtitle, Point location)
        {
            Panel panel = new Panel();
            panel.BackColor = AppTheme.Lighten(theme.NavigationColor, 0.18f);
            panel.Location = location;
            panel.Size = new Size(176, 38);

            Label titleLabel = CreateLabel(title, 9.5F, FontStyle.Bold, theme.AccentColor, new Point(12, 5), new Size(72, 18));
            Label subLabel = CreateLabel(subtitle, 9F, FontStyle.Regular, theme.TextSecondary, new Point(12, 19), new Size(150, 16));
            panel.Controls.Add(titleLabel);
            panel.Controls.Add(subLabel);
            return panel;
        }

        private Panel CreateFeatureItem(string title, string description, Point location)
        {
            Panel panel = new Panel();
            panel.BackColor = AppTheme.Lighten(theme.NavigationColor, 0.10f);
            panel.Location = location;
            panel.Size = new Size(310, 52);

            Label titleLabel = CreateLabel(title, 11F, FontStyle.Bold, theme.AccentColor, new Point(12, 6), new Size(200, 18));
            Label descLabel = CreateLabel(description, 9.7F, FontStyle.Regular, theme.TextSecondary, new Point(12, 24), new Size(286, 24));

            panel.Controls.Add(titleLabel);
            panel.Controls.Add(descLabel);
            return panel;
        }

        private void HomeShortcut_Click(object sender, EventArgs e)
        {
            if (!isReady)
            {
                MessageBox.Show("Vui lòng hoàn tất giao dịch hiện tại trước khi quay lại trang chủ.");
                return;
            }

            ShowHomeDashboard();
        }

        #region menu
        private bool isMouseDown = false;
        private int x, y;

        private void pn_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            x = e.X;
            y = e.Y;
        }

        private void pn_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = new Point(this.Location.X + e.X - x, this.Location.Y + e.Y - y);
            }
        }

        private void pn_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void lbl_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            x = e.X;
            y = e.Y;
        }

        private void lbl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = new Point(this.Location.X + e.X - x, this.Location.Y + e.Y - y);
            }
        }

        private void lbl_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void btnMinimum_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            if (!isReady)
            {
                MessageBox.Show("Vui lòng thanh toán trước khi thực hiện");
                return;
            }
            this.Close();
        }

        private void btnNhaCungCap_Click(object sender, EventArgs e)
        {
            if (!EnsureReady())
            {
                return;
            }

            ActivateNavButton(btnNhaCungCap);
            LoadChildForm(new fNhaCungCap());
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            if (!EnsureReady())
            {
                return;
            }

            ActivateNavButton(btnKhachHang);
            LoadChildForm(new fKhachHang());
        }

        private void txtDanhMuc_Click(object sender, EventArgs e)
        {
            if (!EnsureReady())
            {
                return;
            }

            ActivateNavButton(txtDanhMuc);
            LoadChildForm(new fDanhMuc());
        }

        private void btnMayTinh_Click(object sender, EventArgs e)
        {
            if (!EnsureReady())
            {
                return;
            }

            ActivateNavButton(btnMayTinh);
            LoadChildForm(new fMayTinh());
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (!EnsureReady())
            {
                return;
            }
            this.Close();
        }

        private void btnThongTinCaNhan_Click(object sender, EventArgs e)
        {
            if (!EnsureReady())
            {
                return;
            }

            ActivateNavButton(btnThongTinCaNhan);
            fThongTinCaNhan f = new fThongTinCaNhan(acc);
            LoadChildForm(f);
            f.CapNhatThongTin += f_CapNhatThongTin;
        }

        void f_CapNhatThongTin(object sender, EventArgs e)
        {
            acc = TaiKhoanDAO.Instance.GetByUsername(acc.UserName);
            ShowHomeDashboard();
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            if (!EnsureReady())
            {
                return;
            }

            ActivateNavButton(btnNhap);
            fNhap f = new fNhap(acc);
            LoadChildForm(f);
            f.MyEvent += btnNhaCungCap_Click;
            f.DangNhapHang += f_DangNhapHang;
            f.DaNhapHang += f_DaNhapHang;
        }

        void f_DaNhapHang(object sender, EventArgs e)
        {
            this.isReady = true;
        }

        void f_DangNhapHang(object sender, EventArgs e)
        {
            this.isReady = false;
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (!EnsureReady())
            {
                return;
            }

            ActivateNavButton(btnXuat);
            fXuat f = new fXuat(acc);
            LoadChildForm(f);
            f.MyEvent += btnKhachHang_Click;
            f.DangBanHang += f_DangBanHang;
            f.DaBanHang += f_DaBanHang;
        }

        void f_DaBanHang(object sender, EventArgs e)
        {
            this.isReady = true;
        }

        void f_DangBanHang(object sender, EventArgs e)
        {
            this.isReady = false;
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            if (!EnsureReady())
            {
                return;
            }

            ActivateNavButton(btnTaiKhoan);
            LoadChildForm(new fTaiKhoan(acc));
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (!EnsureReady())
            {
                return;
            }

            ActivateNavButton(btnThongKe);
            fThongKe f = new fThongKe();
            LoadChildForm(f);
        }

        private bool EnsureReady()
        {
            if (!isReady)
            {
                MessageBox.Show("Vui lòng thanh toán trước khi thực hiện");
                return false;
            }

            return true;
        }

        private void LoadChildForm(Form f)
        {
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.Dock = DockStyle.Fill;
            AppTheme.Apply(f);
            pnContent.Controls.Clear();
            pnContent.BackColor = theme.PageColor;
            pnContent.Controls.Add(f);
            f.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
    }
}
