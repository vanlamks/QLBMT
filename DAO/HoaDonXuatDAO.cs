using QuanLyCuaHangMayTinh.DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace QuanLyCuaHangMayTinh.DAO
{
    public class HoaDonXuatDAO
    {
        private HoaDonXuatDAO() { }

        private static HoaDonXuatDAO instance;

        public static HoaDonXuatDAO Instance
        {
            get
            {
                if (instance == null) instance = new HoaDonXuatDAO();
                return HoaDonXuatDAO.instance;
            }
            private set { HoaDonXuatDAO.instance = value; }
        }

        private static int tong;

        public static int Tong
        {
            get { return HoaDonXuatDAO.tong; }
            set { HoaDonXuatDAO.tong = value; }
        }

        public List<HoaDonXuat> GetByTime(DateTime from, DateTime to)
        {
            DataTable data = new DataTable();
            List<HoaDonXuat> list = new List<HoaDonXuat>();

            string query = @"
                SELECT 
                    x.ID,
                    ISNULL(kh.Ten, N'') AS Ten,
                    ISNULL(kh.DiaChi, N'') AS DiaChiKhachHang,
                    x.NgayXuat,
                    ISNULL(x.NguoiBanHang, N'') AS NguoiBanHang,
                    ISNULL(tam.TongTien, 0) AS TongTien
                FROM Xuat x
                LEFT JOIN KhachHang kh ON x.IdKhachHang = kh.Cmtnd
                LEFT JOIN
                (
                    SELECT 
                        ct.IdXuat,
                        SUM(ct.SoLuong * mt.DonGiaXuat) AS TongTien
                    FROM CTXuat ct
                    INNER JOIN MayTinh mt ON ct.MaMayTinh = mt.Ma
                    GROUP BY ct.IdXuat
                ) tam ON x.ID = tam.IdXuat
                WHERE x.NgayXuat >= @fromDay
                  AND x.NgayXuat < DATEADD(DAY, 1, CAST(@toDay AS DATE))
                ORDER BY x.NgayXuat DESC, x.ID DESC";

            data = DataProvider.Instance.executeQuery(query, new object[] { from, to });

            tong = 0;
            foreach (DataRow item in data.Rows)
            {
                list.Add(new HoaDonXuat(item));
                tong += int.Parse(item["TongTien"].ToString());
            }

            return list;
        }

        public List<HoaDonXuat> GetAll()
        {
            DataTable data = new DataTable();
            List<HoaDonXuat> list = new List<HoaDonXuat>();

            string query = @"
                SELECT 
                    x.ID,
                    ISNULL(kh.Ten, N'') AS Ten,
                    ISNULL(kh.DiaChi, N'') AS DiaChiKhachHang,
                    x.NgayXuat,
                    ISNULL(x.NguoiBanHang, N'') AS NguoiBanHang,
                    ISNULL(tam.TongTien, 0) AS TongTien
                FROM Xuat x
                LEFT JOIN KhachHang kh ON x.IdKhachHang = kh.Cmtnd
                LEFT JOIN
                (
                    SELECT 
                        ct.IdXuat,
                        SUM(ct.SoLuong * mt.DonGiaXuat) AS TongTien
                    FROM CTXuat ct
                    INNER JOIN MayTinh mt ON ct.MaMayTinh = mt.Ma
                    GROUP BY ct.IdXuat
                ) tam ON x.ID = tam.IdXuat
                ORDER BY x.NgayXuat DESC, x.ID DESC";

            data = DataProvider.Instance.executeQuery(query);

            tong = 0;
            foreach (DataRow item in data.Rows)
            {
                list.Add(new HoaDonXuat(item));
                tong += int.Parse(item["TongTien"].ToString());
            }

            return list;
        }

        public HoaDonXuat GetById(int id)
        {
            string query = @"
                SELECT 
                    x.ID,
                    ISNULL(kh.Ten, N'') AS Ten,
                    ISNULL(kh.DiaChi, N'') AS DiaChiKhachHang,
                    x.NgayXuat,
                    ISNULL(x.NguoiBanHang, N'') AS NguoiBanHang,
                    ISNULL(tam.TongTien, 0) AS TongTien
                FROM Xuat x
                LEFT JOIN KhachHang kh ON x.IdKhachHang = kh.Cmtnd
                LEFT JOIN
                (
                    SELECT 
                        ct.IdXuat,
                        SUM(ct.SoLuong * mt.DonGiaXuat) AS TongTien
                    FROM CTXuat ct
                    INNER JOIN MayTinh mt ON ct.MaMayTinh = mt.Ma
                    GROUP BY ct.IdXuat
                ) tam ON x.ID = tam.IdXuat
                WHERE x.ID = @id";

            DataTable data = DataProvider.Instance.executeQuery(query, new object[] { id });

            if (data.Rows.Count == 0)
                return null;

            return new HoaDonXuat(data.Rows[0]);
        }

        public DataTable GetDataTableById(int id)
        {
            string query = @"
                SELECT 
                    x.ID,
                    ISNULL(kh.Ten, N'') AS Ten,
                    ISNULL(kh.DiaChi, N'') AS DiaChiKhachHang,
                    x.NgayXuat,
                    ISNULL(x.NguoiBanHang, N'') AS NguoiBanHang,
                    ISNULL(tam.TongTien, 0) AS TongTien
                FROM Xuat x
                LEFT JOIN KhachHang kh ON x.IdKhachHang = kh.Cmtnd
                LEFT JOIN
                (
                    SELECT 
                        ct.IdXuat,
                        SUM(ct.SoLuong * mt.DonGiaXuat) AS TongTien
                    FROM CTXuat ct
                    INNER JOIN MayTinh mt ON ct.MaMayTinh = mt.Ma
                    GROUP BY ct.IdXuat
                ) tam ON x.ID = tam.IdXuat
                WHERE x.ID = @id";

            return DataProvider.Instance.executeQuery(query, new object[] { id });
        }
    }
}