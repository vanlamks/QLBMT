using QuanLyCuaHangMayTinh.DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace QuanLyCuaHangMayTinh.DAO
{
    public class ChiTietXuatDAO
    {
        private ChiTietXuatDAO() { }
        private static ChiTietXuatDAO instance;

        public static ChiTietXuatDAO Instance
        {
            get
            {
                if (instance == null) instance = new ChiTietXuatDAO();
                return ChiTietXuatDAO.instance;
            }
            private set { ChiTietXuatDAO.instance = value; }
        }

        public List<ChiTietXuat> GetByIdXuat(int idXuat)
        {
            string query = @"
                SELECT 
                    c.ID,
                    c.IdXuat,
                    c.MaMayTinh,
                    ISNULL(m.Ten, N'') AS TenMayTinh,
                    c.SoLuong
                FROM CTXuat c
                LEFT JOIN MayTinh m ON c.MaMayTinh = m.Ma
                WHERE c.IdXuat = @idXuat";

            DataTable data = DataProvider.Instance.executeQuery(query, new object[] { idXuat });
            List<ChiTietXuat> list = new List<ChiTietXuat>();

            foreach (DataRow item in data.Rows)
            {
                list.Add(new ChiTietXuat(item));
            }

            return list;
        }

        public DataTable GetDataTableByIdXuat(int idXuat)
        {
            string query = @"
                SELECT 
                    c.ID,
                    c.IdXuat,
                    c.MaMayTinh,
                    ISNULL(m.Ten, N'') AS TenMayTinh,
                    c.SoLuong
                FROM CTXuat c
                LEFT JOIN MayTinh m ON c.MaMayTinh = m.Ma
                WHERE c.IdXuat = @idXuat";

            return DataProvider.Instance.executeQuery(query, new object[] { idXuat });
        }

        public DataTable GetDataTableHoaDonByIdXuat(int idXuat)
        {
            string query = @"
                SELECT
                    ROW_NUMBER() OVER (ORDER BY c.ID) AS STT,
                    ISNULL(m.Ten, N'') AS TenHang,
                    c.SoLuong,
                    ISNULL(m.DonGiaXuat, 0) AS DonGia,
                    c.SoLuong * ISNULL(m.DonGiaXuat, 0) AS ThanhTien
                FROM CTXuat c
                LEFT JOIN MayTinh m ON c.MaMayTinh = m.Ma
                WHERE c.IdXuat = @idXuat
                ORDER BY c.ID";

            return DataProvider.Instance.executeQuery(query, new object[] { idXuat });
        }

        public bool Add(int idXuat, string maMayTinh, int soLuong)
        {
            try
            {
                int result = DataProvider.Instance.executeNonQuery(
                    "uspAddChiTietXuat @idXuat , @maMayTinh , @soLuong ",
                    new object[] { idXuat, maMayTinh, soLuong });

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Del(int idChiTietXuat)
        {
            try
            {
                int result = DataProvider.Instance.executeNonQuery(
                    "uspDelChiTietXuat @id ",
                    new object[] { idChiTietXuat });

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}