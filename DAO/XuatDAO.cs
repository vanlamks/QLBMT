using QuanLyCuaHangMayTinh.DTO;
using System;
using System.Data;

namespace QuanLyCuaHangMayTinh.DAO
{
    public class XuatDAO
    {
        private XuatDAO() { }
        private static XuatDAO instance;

        public static XuatDAO Instance
        {
            get
            {
                if (instance == null) instance = new XuatDAO();
                return XuatDAO.instance;
            }
            private set { XuatDAO.instance = value; }
        }

        public int Add(string cmtnd, string username)
        {
            string query = "uspInsertXuat @idKhachHang , @nguoiXuat ";
            try
            {
                DataTable data = DataProvider.Instance.executeQuery(query, new object[] { cmtnd, username });

                if (data.Rows.Count == 0)
                    return 0;

                return int.Parse(data.Rows[0]["ID"].ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public HoaDonXuat GetHoaDonById(int id)
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

            try
            {
                DataTable data = DataProvider.Instance.executeQuery(query, new object[] { id });

                if (data.Rows.Count == 0)
                    return null;

                return new HoaDonXuat(data.Rows[0]);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetDataHoaDonById(int id)
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

            try
            {
                return DataProvider.Instance.executeQuery(query, new object[] { id });
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public int GetMaxID()
        {
            string query = "SELECT MAX(ID) AS ID FROM Xuat";
            DataTable data = new DataTable();

            try
            {
                data = DataProvider.Instance.executeQuery(query);

                if (data.Rows.Count == 1 && data.Rows[0]["ID"].ToString() != "")
                    return int.Parse(data.Rows[0]["ID"].ToString());
            }
            catch (Exception)
            {
            }

            return 0;
        }

        public bool DelByTime(DateTime from, DateTime to)
        {
            try
            {
                string query = @"
                    DELETE FROM Xuat
                    WHERE NgayXuat >= @fromDay
                      AND NgayXuat < DATEADD(DAY, 1, CAST(@toDay AS DATE))";

                int result = DataProvider.Instance.executeNonQuery(query, new object[] { from, to });
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Del(int idXuat)
        {
            try
            {
                string query = "DELETE FROM Xuat WHERE ID = @id";
                int result = DataProvider.Instance.executeNonQuery(query, new object[] { idXuat });

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetTotalPriceById(int id)
        {
            try
            {
                string query = "uspGetTotalPriceByIDXuat @id";
                DataTable data = DataProvider.Instance.executeQuery(query, new object[] { id });

                if (data.Rows.Count > 0 && data.Rows[0]["TotalPrice"].ToString() != "")
                    return int.Parse(data.Rows[0]["TotalPrice"].ToString());
            }
            catch (Exception)
            {
            }

            return 0;
        }
    }
}