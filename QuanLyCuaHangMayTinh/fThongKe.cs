using QuanLyCuaHangMayTinh.Code;
using QuanLyCuaHangMayTinh.DAO;
using System;
using System.Windows.Forms;

namespace QuanLyCuaHangMayTinh
{
    public partial class fThongKe : Form
    {
        private BindingSource bds = new BindingSource();

        public fThongKe()
        {
            InitializeComponent();
            AppTheme.Apply(this);

            dtgv.DataSource = bds;

            dtpkFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
            dtpkTo.Value = DateTime.Today;

            cbxType.SelectedIndex = 1; // 0 = Hóa đơn nhập, 1 = Hóa đơn xuất
            LoadDtgv();
            ChangeHeader();
        }

        public void ChangeHeader()
        {
            if (dtgv.Columns["ID"] != null)
                dtgv.Columns["ID"].HeaderText = "ID";

            if (cbxType.SelectedIndex == 1) // Hóa đơn xuất
            {
                if (dtgv.Columns["KhachHang"] != null)
                    dtgv.Columns["KhachHang"].HeaderText = "Khách hàng";

                if (dtgv.Columns["DiaChiKhachHang"] != null)
                    dtgv.Columns["DiaChiKhachHang"].HeaderText = "Địa chỉ khách hàng";

                if (dtgv.Columns["NgayXuat"] != null)
                    dtgv.Columns["NgayXuat"].HeaderText = "Ngày xuất";

                if (dtgv.Columns["NguoiBanHang"] != null)
                    dtgv.Columns["NguoiBanHang"].HeaderText = "Người bán hàng";

                if (dtgv.Columns["TongTien"] != null)
                    dtgv.Columns["TongTien"].HeaderText = "Tổng tiền";

                if (dtgv.Columns["ThoiGianXuatText"] != null)
                    dtgv.Columns["ThoiGianXuatText"].Visible = false;
            }
            else // Hóa đơn nhập
            {
                if (dtgv.Columns["MaNhaCungCap"] != null)
                    dtgv.Columns["MaNhaCungCap"].HeaderText = "Mã nhà cung cấp";

                if (dtgv.Columns["TenNhaCungCap"] != null)
                    dtgv.Columns["TenNhaCungCap"].HeaderText = "Tên nhà cung cấp";

                if (dtgv.Columns["DiaChiNhaCungCap"] != null)
                    dtgv.Columns["DiaChiNhaCungCap"].HeaderText = "Địa chỉ NCC";

                if (dtgv.Columns["NgayNhap"] != null)
                    dtgv.Columns["NgayNhap"].HeaderText = "Ngày nhập";

                if (dtgv.Columns["NguoiNhapHang"] != null)
                    dtgv.Columns["NguoiNhapHang"].HeaderText = "Người nhập hàng";

                if (dtgv.Columns["TongTien"] != null)
                    dtgv.Columns["TongTien"].HeaderText = "Tổng tiền";

                if (dtgv.Columns["ThoiGianNhapText"] != null)
                    dtgv.Columns["ThoiGianNhapText"].Visible = false;
            }
        }

        public void LoadDtgv()
        {
            if (cbxType.SelectedIndex == 1)
            {
                bds.DataSource = HoaDonXuatDAO.Instance.GetByTime(dtpkFrom.Value, dtpkTo.Value);
                txtTongTien.Text = HoaDonXuatDAO.Tong.ToString();
            }
            else
            {
                bds.DataSource = HoaDonNhapDAO.Instance.GetByTime(dtpkFrom.Value, dtpkTo.Value);
                txtTongTien.Text = HoaDonNhapDAO.Tong.ToString();
            }
        }

        private void cbxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDtgv();
            ChangeHeader();
        }

        private void dtpkFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadDtgv();
            ChangeHeader();
        }

        private void dtpkTo_ValueChanged(object sender, EventArgs e)
        {
            LoadDtgv();
            ChangeHeader();
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            if (cbxType.SelectedIndex == 1)
            {
                bds.DataSource = HoaDonXuatDAO.Instance.GetAll();
                txtTongTien.Text = HoaDonXuatDAO.Tong.ToString();
            }
            else
            {
                bds.DataSource = HoaDonNhapDAO.Instance.GetAll();
                txtTongTien.Text = HoaDonNhapDAO.Tong.ToString();
            }

            ChangeHeader();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            bool result;

            if (cbxType.SelectedIndex == 1)
                result = XuatDAO.Instance.DelByTime(dtpkFrom.Value, dtpkTo.Value);
            else
                result = NhapDAO.Instance.DelByTime(dtpkFrom.Value, dtpkTo.Value);

            if (result)
            {
                MessageBox.Show("Xóa dữ liệu thành công");
                LoadDtgv();
                ChangeHeader();
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xóa hoặc có lỗi xảy ra");
            }
        }

        private void btnShowReport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF files (*.pdf)|*.pdf";

            if (cbxType.SelectedIndex == 1)
                sfd.FileName = "BaoCaoHoaDonXuat_" + dtpkFrom.Value.ToString("ddMMyyyy") + "_" + dtpkTo.Value.ToString("ddMMyyyy") + ".pdf";
            else
                sfd.FileName = "BaoCaoHoaDonNhap_" + dtpkFrom.Value.ToString("ddMMyyyy") + "_" + dtpkTo.Value.ToString("ddMMyyyy") + ".pdf";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string pdfPath;

                if (cbxType.SelectedIndex == 1)
                {
                    pdfPath = ThongKePdfExporter.ExportHoaDonXuatReport(
                        dtpkFrom.Value,
                        dtpkTo.Value,
                        sfd.FileName);
                }
                else
                {
                    pdfPath = ThongKePdfExporter.ExportHoaDonNhapReport(
                        dtpkFrom.Value,
                        dtpkTo.Value,
                        sfd.FileName);
                }

                using (fXemPdf frm = new fXemPdf(pdfPath))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xuất PDF thất bại: " + ex.Message);
            }
        }
    }
}