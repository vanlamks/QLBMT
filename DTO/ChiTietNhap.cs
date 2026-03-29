using System;
using System.Data;

namespace QuanLyCuaHangMayTinh.DTO
{
    public class ChiTietNhap
    {
        private int iD;
        private int iDNhap;
        private string maMayTinh;
        private string tenMayTinh;
        private int soLuong;

        public ChiTietNhap(DataRow row)
        {
            this.ID = int.Parse(row["ID"].ToString());
            this.IDNhap = int.Parse(row["IdNhap"].ToString());
            this.MaMayTinh = row["MaMayTinh"].ToString();
            this.SoLuong = int.Parse(row["SoLuong"].ToString());

            if (row.Table.Columns.Contains("TenMayTinh"))
                this.TenMayTinh = row["TenMayTinh"].ToString();
            else
                this.TenMayTinh = "";
        }

        public int SoLuong
        {
            get { return soLuong; }
            set { soLuong = value; }
        }

        public string MaMayTinh
        {
            get { return maMayTinh; }
            set { maMayTinh = value; }
        }

        public string TenMayTinh
        {
            get { return tenMayTinh; }
            set { tenMayTinh = value; }
        }

        public int IDNhap
        {
            get { return iDNhap; }
            set { iDNhap = value; }
        }

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
    }
}