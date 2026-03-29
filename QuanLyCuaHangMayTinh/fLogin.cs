using QuanLyCuaHangMayTinh.DAO;
using QuanLyCuaHangMayTinh.DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCuaHangMayTinh
{
    public partial class fLogin : Form
    {
        private readonly ThemePalette theme = AppTheme.Current;

        public fLogin()
        {
            InitializeComponent();
            ApplyLoginTheme();
        }

        private void ApplyLoginTheme()
        {
            BackColor = theme.ShellColor;
            ForeColor = theme.TextPrimary;
            Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            Text = "Đăng nhập - TechZone";
            ClientSize = new Size(500, 310);

            pn.BackColor = theme.NavigationColor;
            pn.Height = 48;

            pictureBox1.Location = new Point(10, 6);
            pictureBox1.Size = new Size(42, 36);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            lbl.Text = "Đăng nhập hệ thống";
            lbl.Font = new Font("Segoe UI Semibold", 13.5F, FontStyle.Bold);
            lbl.ForeColor = theme.AccentColor;
            lbl.BackColor = theme.NavigationColor;
            lbl.Location = new Point(58, 10);
            lbl.AutoSize = true;

            btnMinimum.Text = "—";
            btnMinimum.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnMinimum.Location = new Point(ClientSize.Width - 90, 6);
            btnMinimum.Size = new Size(36, 34);
            btnMinimum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            StyleWindowButton(btnMinimum, false);

            btnThoat.Text = "✕";
            btnThoat.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnThoat.Location = new Point(ClientSize.Width - 46, 6);
            btnThoat.Size = new Size(36, 34);
            btnThoat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            StyleWindowButton(btnThoat, true);

            Panel loginCard = new Panel();
            loginCard.BackColor = theme.CardColor;
            loginCard.Location = new Point(18, 58);
            loginCard.Size = new Size(464, 232);
            Controls.Add(loginCard);
            loginCard.SendToBack();

            Label subtitle = new Label();
            subtitle.Text = "Vui lòng đăng nhập để quản lý bán hàng, kho hàng và báo cáo cửa hàng.";
            subtitle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular);
            subtitle.ForeColor = theme.TextSecondary;
            subtitle.BackColor = theme.CardColor;
            subtitle.Location = new Point(40, 70);
            subtitle.Size = new Size(420, 38);
            Controls.Add(subtitle);
            subtitle.BringToFront();

            label1.Text = "Tên đăng nhập";
            label1.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            label1.ForeColor = theme.TextPrimary;
            label1.BackColor = theme.CardColor;
            label1.Location = new Point(40, 122);
            label1.AutoSize = true;

            label2.Text = "Mật khẩu";
            label2.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            label2.ForeColor = theme.TextPrimary;
            label2.BackColor = theme.CardColor;
            label2.Location = new Point(40, 172);
            label2.AutoSize = true;

            StyleTextBox(txtUserName, new Point(170, 116));
            StyleTextBox(txtPassword, new Point(170, 166));
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;

            btnLogin.Text = "Đăng nhập";
            btnLogin.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            AppTheme.StylePrimaryButton(btnLogin);
            btnLogin.Location = new Point(170, 222);
            btnLogin.Size = new Size(210, 42);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.ForeColor = Color.White;

            Label footer = new Label();
            footer.Text = "TechZone • Quản lý bán hàng, kho hàng và báo cáo";
            footer.Font = new Font("Segoe UI", 9.3F, FontStyle.Italic);
            footer.ForeColor = theme.TextSecondary;
            footer.BackColor = theme.ShellColor;
            footer.Location = new Point(110, 277);
            footer.Size = new Size(290, 20);
            Controls.Add(footer);
            footer.BringToFront();

            AcceptButton = btnLogin;
        }

        private void StyleTextBox(TextBox textBox, Point location)
        {
            textBox.Location = location;
            textBox.Size = new Size(210, 32);
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = theme.InputColor;
            textBox.ForeColor = theme.TextPrimary;
            textBox.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
        }

        private void StyleWindowButton(Button button, bool isCloseButton)
        {
            button.BackColor = theme.NavigationColor;
            button.ForeColor = theme.TextSecondary;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.MouseEnter += delegate
            {
                button.BackColor = isCloseButton ? Color.FromArgb(180, 50, 50) : AppTheme.Lighten(theme.NavigationColor, 0.08f);
                button.ForeColor = theme.TextPrimary;
            };
            button.MouseLeave += delegate
            {
                button.BackColor = theme.NavigationColor;
                button.ForeColor = theme.TextSecondary;
            };
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

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (TaiKhoanDAO.Instance.CheckLogin(txtUserName.Text, txtPassword.Text))
                {
                    this.Hide();
                    TaiKhoan acc = TaiKhoanDAO.Instance.GetByUsername(txtUserName.Text);
                    fManager f = new fManager(acc);
                    f.ShowDialog();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Kết nối không thành công.\r\nKiểm tra lại cấu hình kết nối hoặc liên hệ quản trị viên.");
            }
        }
    }
}
