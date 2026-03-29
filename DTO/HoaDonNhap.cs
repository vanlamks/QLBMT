using System;
using System.Data;

namespace QuanLyCuaHangMayTinh.DTO
{
    public class HoaDonNhap
    {
        private int iD;
        private string maNhaCungCap;
        private string tenNhaCungCap;
        private string diaChiNhaCungCap;
        private DateTime ngayNhap;
        private string nguoiNhapHang;
        private int tongTien;

        public HoaDonNhap(DataRow row)
        {
            this.ID = int.Parse(row["ID"].ToString());

            if (row.Table.Columns.Contains("MaNhaCungCap"))
                this.MaNhaCungCap = row["MaNhaCungCap"].ToString();
            else
                this.MaNhaCungCap = "";

            if (row.Table.Columns.Contains("TenNhaCungCap"))
                this.TenNhaCungCap = row["TenNhaCungCap"].ToString();
            else
                this.TenNhaCungCap = "";

            if (row.Table.Columns.Contains("DiaChiNhaCungCap"))
                this.DiaChiNhaCungCap = row["DiaChiNhaCungCap"].ToString();
            else
                this.DiaChiNhaCungCap = "";

            if (row.Table.Columns.Contains("NgayNhap"))
                this.NgayNhap = Convert.ToDateTime(row["NgayNhap"]);
            else
                this.NgayNhap = DateTime.Now;

            if (row.Table.Columns.Contains("NguoiNhapHang"))
                this.NguoiNhapHang = row["NguoiNhapHang"].ToString();
            else
                this.NguoiNhapHang = "";

            if (row.Table.Columns.Contains("TongTien") && row["TongTien"].ToString() != "")
                this.TongTien = int.Parse(row["TongTien"].ToString());
            else
                this.TongTien = 0;
        }

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public string MaNhaCungCap
        {
            get { return maNhaCungCap; }
            set { maNhaCungCap = value; }
        }

        public string TenNhaCungCap
        {
            get { return tenNhaCungCap; }
            set { tenNhaCungCap = value; }
        }

        public string DiaChiNhaCungCap
        {
            get { return diaChiNhaCungCap; }
            set { diaChiNhaCungCap = value; }
        }

        public DateTime NgayNhap
        {
            get { return ngayNhap; }
            set { ngayNhap = value; }
        }

        public string NguoiNhapHang
        {
            get { return nguoiNhapHang; }
            set { nguoiNhapHang = value; }
        }

        public int TongTien
        {
            get { return tongTien; }
            set { tongTien = value; }
        }

        public string ThoiGianNhapText
        {
            get { return NgayNhap.ToString("dd/MM/yyyy HH:mm:ss"); }
        }
    }
}