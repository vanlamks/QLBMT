using QuanLyCuaHangMayTinh.Code;
using QuanLyCuaHangMayTinh.DAO;
using QuanLyCuaHangMayTinh.DTO;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCuaHangMayTinh
{
    public partial class fXuat : Form
    {
        private KhachHang currKh;
        private BindingSource bds = new BindingSource();
        private TaiKhoan acc;
        private int currIDXuat = 0;

        public fXuat(TaiKhoan acc)
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

            if (dtgv.Columns["IdXuat"] != null)
                dtgv.Columns["IdXuat"].HeaderText = "Số hóa đơn xuất";

            if (dtgv.Columns["ID"] != null)
                dtgv.Columns["ID"].Visible = false;
        }

        private event EventHandler myEvent;
        public event EventHandler MyEvent
        {
            add { myEvent += value; }
            remove { myEvent -= value; }
        }

        private event EventHandler dangBanHang;
        public event EventHandler DangBanHang
        {
            add { dangBanHang += value; }
            remove { dangBanHang -= value; }
        }

        private event EventHandler daBanHang;
        public event EventHandler DaBanHang
        {
            add { daBanHang += value; }
            remove { daBanHang -= value; }
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

                if (XuatDAO.Instance.Del(currIDXuat))
                {
                    AppTheme.StylePrimaryButton(btnConfirm);
                    btnConfirm.Text = "Bắt đầu mua hàng";
                    pnChiTietXuat.Visible = false;
                    currIDXuat = 0;
                    bds.DataSource = null;
                    txtThanhTien.Text = "0";
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra. Vui lòng liên hệ admin");
                }
                return;
            }

            if (txtCmtnd.Text.Trim() == "")
            {
                MessageBox.Show("Mã không được để trống");
                return;
            }

            currKh = KhachHangDAO.Instance.Check(txtCmtnd.Text.Trim());
            if (currKh == null)
            {
                MessageBox.Show("Vui lòng nhập vào thông tin khách hàng!!!");

                if (myEvent != null)
                    myEvent(this, new EventArgs());

                return;
            }

            if (MessageBox.Show(
                "Bạn có chắc chắn đang xuất hàng cho khách hàng có mã: " + currKh.Cmtnd +
                " tên là: " + currKh.Ten,
                "Xác nhận",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                currIDXuat = XuatDAO.Instance.Add(txtCmtnd.Text.Trim(), acc.UserName);

                if (currIDXuat == 0)
                {
                    MessageBox.Show("Có lỗi xảy ra. Liên hệ admin");
                    return;
                }

                AppTheme.StyleSecondaryButton(btnConfirm);
                btnConfirm.Text = "Hủy bỏ";
                pnChiTietXuat.Visible = true;

                LoadDtgv();
                dtgv.DataSource = bds;
                ChangeHeader();
                AddDataBinding();
                LoadTxtThanhTien();

                if (dangBanHang != null)
                    dangBanHang(this, e);
            }
        }

        public void LoadTxtThanhTien()
        {
            txtThanhTien.Text = XuatDAO.Instance.GetTotalPriceById(currIDXuat).ToString();
        }

        public void LoadDtgv()
        {
            bds.DataSource = ChiTietXuatDAO.Instance.GetByIdXuat(currIDXuat);
        }

        public void AddDataBinding()
        {
            txtMaMayTinh.DataBindings.Clear();
            nbudSoLuong.DataBindings.Clear();

            txtMaMayTinh.DataBindings.Add("Text", dtgv.DataSource, "MaMayTinh", true, DataSourceUpdateMode.Never);
            nbudSoLuong.DataBindings.Add("Value", dtgv.DataSource, "SoLuong", true, DataSourceUpdateMode.Never);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (currIDXuat == 0)
            {
                MessageBox.Show("Bạn chưa bắt đầu hóa đơn xuất");
                return;
            }

            if (txtMaMayTinh.Text.Trim() == "")
            {
                MessageBox.Show("Không được bỏ trống mã máy tính");
                return;
            }

            if (ChiTietXuatDAO.Instance.Add(currIDXuat, txtMaMayTinh.Text.Trim(), (int)nbudSoLuong.Value))
            {
                MessageBox.Show("Thêm thành công");
                LoadDtgv();
                LoadTxtThanhTien();
                ChangeHeader();
            }
            else
            {
                MessageBox.Show("Hết hàng hoặc mã máy tính không tồn tại");
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            if (dtgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần hủy");
                return;
            }

            int idChiTietXuat = int.Parse(dtgv.SelectedRows[0].Cells["ID"].Value.ToString());

            if (ChiTietXuatDAO.Instance.Del(idChiTietXuat))
            {
                MessageBox.Show("Hủy thành công");
                LoadDtgv();
                LoadTxtThanhTien();
                ChangeHeader();
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra");
            }
        }


        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (currIDXuat == 0)
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
                sfd.FileName = "HoaDonXuat_" + currIDXuat + ".pdf";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    string pdfPath = Code.HoaDonPdfExporter.ExportHoaDonPdf(currIDXuat, sfd.FileName);

                    if (daBanHang != null)
                        daBanHang(this, e);

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