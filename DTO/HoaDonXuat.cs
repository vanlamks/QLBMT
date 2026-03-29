using System;
using System.Data;

namespace QuanLyCuaHangMayTinh.DTO
{
    public class HoaDonXuat
    {
        private int iD;
        private string khachHang;
        private string diaChiKhachHang;
        private DateTime ngayXuat;
        private string nguoiBanHang;
        private int tongTien;

        public HoaDonXuat(DataRow row)
        {
            this.ID = int.Parse(row["ID"].ToString());

            if (row.Table.Columns.Contains("Ten"))
                this.KhachHang = row["Ten"].ToString();
            else
                this.KhachHang = "";

            if (row.Table.Columns.Contains("DiaChiKhachHang"))
                this.DiaChiKhachHang = row["DiaChiKhachHang"].ToString();
            else
                this.DiaChiKhachHang = "";

            if (row.Table.Columns.Contains("NgayXuat"))
                this.NgayXuat = Convert.ToDateTime(row["NgayXuat"]);
            else
                this.NgayXuat = DateTime.Now;

            if (row.Table.Columns.Contains("NguoiBanHang"))
                this.NguoiBanHang = row["NguoiBanHang"].ToString();
            else
                this.NguoiBanHang = "";

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

        public string KhachHang
        {
            get { return khachHang; }
            set { khachHang = value; }
        }

        public string DiaChiKhachHang
        {
            get { return diaChiKhachHang; }
            set { diaChiKhachHang = value; }
        }

        public DateTime NgayXuat
        {
            get { return ngayXuat; }
            set { ngayXuat = value; }
        }

        public string NguoiBanHang
        {
            get { return nguoiBanHang; }
            set { nguoiBanHang = value; }
        }

        public int TongTien
        {
            get { return tongTien; }
            set { tongTien = value; }
        }

        public string ThoiGianXuatText
        {
            get { return NgayXuat.ToString("dd/MM/yyyy HH:mm:ss"); }
        }
    }
}