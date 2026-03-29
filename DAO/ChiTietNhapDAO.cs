using QuanLyCuaHangMayTinh.DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace QuanLyCuaHangMayTinh.DAO
{
    public class ChiTietNhapDAO
    {
        private ChiTietNhapDAO() { }
        private static ChiTietNhapDAO instance;

        public static ChiTietNhapDAO Instance
        {
            get { if (instance == null) instance = new ChiTietNhapDAO(); return instance; }
            private set { instance = value; }
        }

        public List<ChiTietNhap> GetByIDNhap(int idNhap)
        {
            string query = @"
                SELECT 
                    c.ID,
                    c.IdNhap,
                    c.MaMayTinh,
                    ISNULL(m.Ten, N'') AS TenMayTinh,
                    c.SoLuong
                FROM CTNhap c
                LEFT JOIN MayTinh m ON c.MaMayTinh = m.Ma
                WHERE c.IdNhap = @idNhap";

            DataTable data = DataProvider.Instance.executeQuery(query, new object[] { idNhap });
            List<ChiTietNhap> list = new List<ChiTietNhap>();

            foreach (DataRow row in data.Rows)
            {
                list.Add(new ChiTietNhap(row));
            }

            return list;
        }

        public DataTable GetDataTableByIDNhap(int idNhap)
        {
            string query = @"
                SELECT 
                    c.ID,
                    c.IdNhap,
                    c.MaMayTinh,
                    ISNULL(m.Ten, N'') AS TenMayTinh,
                    c.SoLuong
                FROM CTNhap c
                LEFT JOIN MayTinh m ON c.MaMayTinh = m.Ma
                WHERE c.IdNhap = @idNhap";

            return DataProvider.Instance.executeQuery(query, new object[] { idNhap });
        }

        public DataTable GetDataTableHoaDonByIDNhap(int idNhap)
        {
            string query = @"
                SELECT
                    ROW_NUMBER() OVER (ORDER BY c.ID) AS STT,
                    ISNULL(m.Ten, N'') AS TenHang,
                    c.SoLuong,
                    ISNULL(m.DonGiaNhap, 0) AS DonGia,
                    c.SoLuong * ISNULL(m.DonGiaNhap, 0) AS ThanhTien
                FROM CTNhap c
                LEFT JOIN MayTinh m ON c.MaMayTinh = m.Ma
                WHERE c.IdNhap = @idNhap
                ORDER BY c.ID";

            return DataProvider.Instance.executeQuery(query, new object[] { idNhap });
        }

        public bool Add(int idNhap, string maMayTinh, int soLuong)
        {
            try
            {
                int result = DataProvider.Instance.executeNonQuery(
                    "uspAddChiTietNhap @idNhap , @maMayTinh , @soLuong ",
                    new object[] { idNhap, maMayTinh, soLuong });

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Del(int idChiTietNhap)
        {
            try
            {
                int result = DataProvider.Instance.executeNonQuery(
                    "uspDelChiTietNhap @id ",
                    new object[] { idChiTietNhap });

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}