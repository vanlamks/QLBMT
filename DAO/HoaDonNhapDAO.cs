using QuanLyCuaHangMayTinh.DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace QuanLyCuaHangMayTinh.DAO
{
    public class HoaDonNhapDAO
    {
        private HoaDonNhapDAO() { }

        private static HoaDonNhapDAO instance;

        public static HoaDonNhapDAO Instance
        {
            get
            {
                if (instance == null) instance = new HoaDonNhapDAO();
                return instance;
            }
            private set { instance = value; }
        }

        private static int tong;

        public static int Tong
        {
            get { return tong; }
            set { tong = value; }
        }

        public List<HoaDonNhap> GetByTime(DateTime from, DateTime to)
        {
            DataTable data = new DataTable();
            List<HoaDonNhap> list = new List<HoaDonNhap>();

            string query = @"
                SELECT 
                    n.ID,
                    ISNULL(n.MaNhaCungCap, N'') AS MaNhaCungCap,
                    ISNULL(ncc.Ten, N'') AS TenNhaCungCap,
                    ISNULL(ncc.DiaChi, N'') AS DiaChiNhaCungCap,
                    n.NgayNhap,
                    ISNULL(n.NguoiNhapHang, N'') AS NguoiNhapHang,
                    ISNULL(tam.TongTien, 0) AS TongTien
                FROM Nhap n
                LEFT JOIN NhaCungCap ncc ON n.MaNhaCungCap = ncc.Ma
                LEFT JOIN
                (
                    SELECT 
                        ct.IdNhap,
                        SUM(ct.SoLuong * mt.DonGiaNhap) AS TongTien
                    FROM CTNhap ct
                    INNER JOIN MayTinh mt ON ct.MaMayTinh = mt.Ma
                    GROUP BY ct.IdNhap
                ) tam ON n.ID = tam.IdNhap
                WHERE n.NgayNhap >= @fromDay
                  AND n.NgayNhap < DATEADD(DAY, 1, CAST(@toDay AS DATE))
                ORDER BY n.NgayNhap DESC, n.ID DESC";

            data = DataProvider.Instance.executeQuery(query, new object[] { from, to });

            tong = 0;
            foreach (DataRow item in data.Rows)
            {
                list.Add(new HoaDonNhap(item));
                tong += int.Parse(item["TongTien"].ToString());
            }

            return list;
        }

        public List<HoaDonNhap> GetAll()
        {
            DataTable data = new DataTable();
            List<HoaDonNhap> list = new List<HoaDonNhap>();

            string query = @"
                SELECT 
                    n.ID,
                    ISNULL(n.MaNhaCungCap, N'') AS MaNhaCungCap,
                    ISNULL(ncc.Ten, N'') AS TenNhaCungCap,
                    ISNULL(ncc.DiaChi, N'') AS DiaChiNhaCungCap,
                    n.NgayNhap,
                    ISNULL(n.NguoiNhapHang, N'') AS NguoiNhapHang,
                    ISNULL(tam.TongTien, 0) AS TongTien
                FROM Nhap n
                LEFT JOIN NhaCungCap ncc ON n.MaNhaCungCap = ncc.Ma
                LEFT JOIN
                (
                    SELECT 
                        ct.IdNhap,
                        SUM(ct.SoLuong * mt.DonGiaNhap) AS TongTien
                    FROM CTNhap ct
                    INNER JOIN MayTinh mt ON ct.MaMayTinh = mt.Ma
                    GROUP BY ct.IdNhap
                ) tam ON n.ID = tam.IdNhap
                ORDER BY n.NgayNhap DESC, n.ID DESC";

            data = DataProvider.Instance.executeQuery(query);

            tong = 0;
            foreach (DataRow item in data.Rows)
            {
                list.Add(new HoaDonNhap(item));
                tong += int.Parse(item["TongTien"].ToString());
            }

            return list;
        }

        public HoaDonNhap GetById(int id)
        {
            string query = @"
                SELECT 
                    n.ID,
                    ISNULL(n.MaNhaCungCap, N'') AS MaNhaCungCap,
                    ISNULL(ncc.Ten, N'') AS TenNhaCungCap,
                    ISNULL(ncc.DiaChi, N'') AS DiaChiNhaCungCap,
                    n.NgayNhap,
                    ISNULL(n.NguoiNhapHang, N'') AS NguoiNhapHang,
                    ISNULL(tam.TongTien, 0) AS TongTien
                FROM Nhap n
                LEFT JOIN NhaCungCap ncc ON n.MaNhaCungCap = ncc.Ma
                LEFT JOIN
                (
                    SELECT 
                        ct.IdNhap,
                        SUM(ct.SoLuong * mt.DonGiaNhap) AS TongTien
                    FROM CTNhap ct
                    INNER JOIN MayTinh mt ON ct.MaMayTinh = mt.Ma
                    GROUP BY ct.IdNhap
                ) tam ON n.ID = tam.IdNhap
                WHERE n.ID = @id";

            DataTable data = DataProvider.Instance.executeQuery(query, new object[] { id });

            if (data.Rows.Count == 0)
                return null;

            return new HoaDonNhap(data.Rows[0]);
        }
    }
}