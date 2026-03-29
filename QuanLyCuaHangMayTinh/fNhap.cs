using QuanLyCuaHangMayTinh.Code;
using QuanLyCuaHangMayTinh.DAO;
using QuanLyCuaHangMayTinh.DTO;
using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyCuaHangMayTinh
{
    public partial class fNhap : Form
    {
        private TaiKhoan acc;
        private NhaCungCap currNCC;
        private int currIdNhap = 0;
        private BindingSource bds = new BindingSource();

        public fNhap(TaiKhoan acc)
        {
            InitializeComponent();
            AppTheme.Apply(this);
            this.acc = acc;
        }

        public void ChangeHeader()
        {
            if (dtgv.Columns["MaMayTinh"] != null)
                dtgv.Columns["MaMayTinh"].HeaderText = "Mã máy tính";

            if (dtgv.Columns["TenMayTinh"] != null)
                dtgv.Columns["TenMayTinh"].HeaderText = "Tên máy tính";

            if (dtgv.Columns["SoLuong"] != null)
                dtgv.Columns["SoLuong"].HeaderText = "Số lượng";

            if (dtgv.Columns["IDNhap"] != null)
                dtgv.Columns["IDNhap"].HeaderText = "Số hóa đơn nhập";

            if (dtgv.Columns["ID"] != null)
                dtgv.Columns["ID"].Visible = false;
        }

        public void LoadDtgv()
        {
            bds.DataSource = ChiTietNhapDAO.Instance.GetByIDNhap(currIdNhap);
            dtgv.DataSource = bds;
            ChangeHeader();
            AddDataBinding();
        }

        public void LoadTxtThanhTien()
        {
            txtThanhTien.Text = NhapDAO.Instance.GetTotalPriceById(currIdNhap).ToString();
        }

        public void AddDataBinding()
        {
            txtMaMayTinh.DataBindings.Clear();
            nbudSoLuong.DataBindings.Clear();

            txtMaMayTinh.DataBindings.Add("Text", dtgv.DataSource, "MaMayTinh", true, DataSourceUpdateMode.Never);
            nbudSoLuong.DataBindings.Add("Value", dtgv.DataSource, "SoLuong", true, DataSourceUpdateMode.Never);
        }

        private event EventHandler myEvent;
        public event EventHandler MyEvent
        {
            add { myEvent += value; }
            remove { myEvent -= value; }
        }

        private event EventHandler dangNhapHang;
        public event EventHandler DangNhapHang
        {
            add { dangNhapHang += value; }
            remove { dangNhapHang -= value; }
        }

        private event EventHandler daNhapHang;
        public event EventHandler DaNhapHang
        {
            add { daNhapHang += value; }
            remove { daNhapHang -= value; }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (btnConfirm.Text == "Hủy bỏ")
            {
                if (txtThanhTien.Text != "0")
                {
                    MessageBox.Show("Bạn cần trả lại hết hàng");
                    return;
                }

                if (NhapDAO.Instance.Del(currIdNhap))
                {
                    AppTheme.StylePrimaryButton(btnConfirm);
                    btnConfirm.Text = "Xác nhận mã nhà cung cấp";
                    pnChiTietNhap.Visible = false;
                    currIdNhap = 0;
                    currNCC = null;
                    bds.DataSource = null;
                    txtThanhTien.Text = "0";
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra. Vui lòng liên hệ admin");
                }
                return;
            }

            if (txtMaNhaCC.Text.Trim() == "")
            {
                MessageBox.Show("Mã nhà cung cấp không được để trống");
                return;
            }

            currNCC = NhaCungCapDAO.Instance.Check(txtMaNhaCC.Text.Trim());
            if (currNCC == null)
            {
                MessageBox.Show("Mã nhà cung cấp này không tồn tại. Nhập vào thông tin nhà cung cấp!!!");

                if (myEvent != null)
                    myEvent(this, new EventArgs());

                return;
            }

            if (MessageBox.Show(
                "Bạn có chắc chắn đang nhập hàng của nhà cung cấp có mã: " + currNCC.Ma +
                " tên là: " + currNCC.Ten,
                "Xác nhận",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                currIdNhap = NhapDAO.Instance.Add(txtMaNhaCC.Text.Trim(), acc.UserName);

                if (currIdNhap == 0)
                {
                    MessageBox.Show("Có lỗi xảy ra. Liên hệ admin");
                    return;
                }

                AppTheme.StyleSecondaryButton(btnConfirm);
                btnConfirm.Text = "Hủy bỏ";
                pnChiTietNhap.Visible = true;

                LoadDtgv();
                LoadTxtThanhTien();

                if (dangNhapHang != null)
                    dangNhapHang(this, e);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (currIdNhap == 0)
            {
                MessageBox.Show("Bạn chưa bắt đầu hóa đơn nhập");
                return;
            }

            if (!MyRegular.CheckRequired(txtMaMayTinh.Text, "Bắt buộc nhập vào mã máy tính"))
                return;

            if (txtMaMayTinh.Text.Trim() == "")
            {
                MessageBox.Show("Bắt buộc nhập trường này");
                return;
            }

            if (ChiTietNhapDAO.Instance.Add(currIdNhap, txtMaMayTinh.Text.Trim(), (int)nbudSoLuong.Value))
            {
                MessageBox.Show("Thêm thành công");
                LoadDtgv();
                LoadTxtThanhTien();
                ChangeHeader();
            }
            else
            {
                MessageBox.Show("Mã máy tính không đúng");
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            if (dtgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần hủy");
                return;
            }

            int idChiTietNhap = int.Parse(dtgv.SelectedRows[0].Cells["ID"].Value.ToString());

            if (ChiTietNhapDAO.Instance.Del(idChiTietNhap))
            {
                MessageBox.Show("Hủy thành công");
                LoadDtgv();
                LoadTxtThanhTien();
                ChangeHeader();
            }
            else
            {
                MessageBox.Show("Không thể hoàn tác. Có lỗi xảy ra");
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (currIdNhap == 0)
            {
                MessageBox.Show("Chưa có hóa đơn để thanh toán");
                return;
            }

            if (MessageBox.Show(
                "Thành tiền: " + txtThanhTien.Text + "\r\nBạn có muốn thanh toán không?",
                "Xác nhận",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF files (*.pdf)|*.pdf";
                sfd.FileName = "HoaDonNhap_" + currIdNhap + ".pdf";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    string pdfPath = HoaDonNhapPdfExporter.ExportHoaDonPdf(currIdNhap, sfd.FileName);

                    if (daNhapHang != null)
                        daNhapHang(this, e);

                    this.Hide();

                    using (fXemPdf frm = new fXemPdf(pdfPath))
                    {
                        frm.ShowDialog();
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xuất PDF thất bại: " + ex.Message);
                }
            }
        }
    }
}