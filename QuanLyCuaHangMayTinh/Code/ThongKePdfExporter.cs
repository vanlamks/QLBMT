using iTextSharp.text;
using iTextSharp.text.pdf;
using QuanLyCuaHangMayTinh.DAO;
using QuanLyCuaHangMayTinh.DTO;
using System;
using System.Collections.Generic;
using System.IO;

namespace QuanLyCuaHangMayTinh.Code
{
    public class ThongKePdfExporter
    {
        private const string TenCuaHang = "TECHZONE";
        private const string DiaChiCuaHang = "108/37 Miếu Bình Đông";
        private const string DienThoaiCuaHang = "0355335544";

        public static string ExportHoaDonXuatReport(DateTime from, DateTime to, string filePath)
        {
            List<HoaDonXuat> list = HoaDonXuatDAO.Instance.GetByTime(from, to);
            int tongTien = HoaDonXuatDAO.Tong;

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            Font titleFont = new Font(baseFont, 18, Font.BOLD);
            Font headerFont = new Font(baseFont, 11, Font.BOLD);
            Font normalFont = new Font(baseFont, 10, Font.NORMAL);
            Font boldFont = new Font(baseFont, 11, Font.BOLD);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                doc.Add(new Paragraph(TenCuaHang, boldFont));
                doc.Add(new Paragraph("Địa chỉ: " + DiaChiCuaHang, normalFont));
                doc.Add(new Paragraph("ĐT: " + DienThoaiCuaHang, normalFont));
                doc.Add(new Paragraph(" "));

                Paragraph title = new Paragraph("BÁO CÁO HÓA ĐƠN XUẤT", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);

                Paragraph time = new Paragraph(
                    "Từ ngày: " + from.ToString("dd/MM/yyyy") + "   Đến ngày: " + to.ToString("dd/MM/yyyy"),
                    normalFont);
                time.Alignment = Element.ALIGN_CENTER;
                doc.Add(time);
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 8, 20, 24, 18, 16, 14 });

                AddHeaderCell(table, "ID", headerFont);
                AddHeaderCell(table, "Khách hàng", headerFont);
                AddHeaderCell(table, "Địa chỉ khách hàng", headerFont);
                AddHeaderCell(table, "Ngày xuất", headerFont);
                AddHeaderCell(table, "Người bán hàng", headerFont);
                AddHeaderCell(table, "Tổng tiền", headerFont);

                foreach (HoaDonXuat item in list)
                {
                    AddBodyCell(table, item.ID.ToString(), normalFont);
                    AddBodyCell(table, item.KhachHang, normalFont);
                    AddBodyCell(table, item.DiaChiKhachHang, normalFont);
                    AddBodyCell(table, item.NgayXuat.ToString("dd/MM/yyyy HH:mm"), normalFont);
                    AddBodyCell(table, item.NguoiBanHang, normalFont);
                    AddBodyCell(table, FormatMoney(item.TongTien), normalFont);
                }

                doc.Add(table);
                doc.Add(new Paragraph(" "));

                Paragraph tong = new Paragraph("Tổng tiền: " + FormatMoney(tongTien) + " VNĐ", boldFont);
                tong.Alignment = Element.ALIGN_RIGHT;
                doc.Add(tong);

                doc.Close();
            }

            return filePath;
        }

        public static string ExportHoaDonNhapReport(DateTime from, DateTime to, string filePath)
        {
            List<HoaDonNhap> list = HoaDonNhapDAO.Instance.GetByTime(from, to);
            int tongTien = HoaDonNhapDAO.Tong;

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            Font titleFont = new Font(baseFont, 18, Font.BOLD);
            Font headerFont = new Font(baseFont, 11, Font.BOLD);
            Font normalFont = new Font(baseFont, 10, Font.NORMAL);
            Font boldFont = new Font(baseFont, 11, Font.BOLD);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                doc.Add(new Paragraph(TenCuaHang, boldFont));
                doc.Add(new Paragraph("Địa chỉ: " + DiaChiCuaHang, normalFont));
                doc.Add(new Paragraph("ĐT: " + DienThoaiCuaHang, normalFont));
                doc.Add(new Paragraph(" "));

                Paragraph title = new Paragraph("BÁO CÁO HÓA ĐƠN NHẬP", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);

                Paragraph time = new Paragraph(
                    "Từ ngày: " + from.ToString("dd/MM/yyyy") + "   Đến ngày: " + to.ToString("dd/MM/yyyy"),
                    normalFont);
                time.Alignment = Element.ALIGN_CENTER;
                doc.Add(time);
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 8, 14, 20, 24, 18, 16 });

                AddHeaderCell(table, "ID", headerFont);
                AddHeaderCell(table, "Mã NCC", headerFont);
                AddHeaderCell(table, "Tên nhà cung cấp", headerFont);
                AddHeaderCell(table, "Địa chỉ NCC", headerFont);
                AddHeaderCell(table, "Ngày nhập", headerFont);
                AddHeaderCell(table, "Tổng tiền", headerFont);

                foreach (HoaDonNhap item in list)
                {
                    AddBodyCell(table, item.ID.ToString(), normalFont);
                    AddBodyCell(table, item.MaNhaCungCap, normalFont);
                    AddBodyCell(table, item.TenNhaCungCap, normalFont);
                    AddBodyCell(table, item.DiaChiNhaCungCap, normalFont);
                    AddBodyCell(table, item.NgayNhap.ToString("dd/MM/yyyy HH:mm"), normalFont);
                    AddBodyCell(table, FormatMoney(item.TongTien), normalFont);
                }

                doc.Add(table);
                doc.Add(new Paragraph(" "));

                Paragraph tong = new Paragraph("Tổng tiền: " + FormatMoney(tongTien) + " VNĐ", boldFont);
                tong.Alignment = Element.ALIGN_RIGHT;
                doc.Add(tong);

                doc.Close();
            }

            return filePath;
        }

        private static void AddHeaderCell(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 6;
            table.AddCell(cell);
        }

        private static void AddBodyCell(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text ?? "", font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            table.AddCell(cell);
        }

        private static string FormatMoney(object value)
        {
            decimal money = 0;
            decimal.TryParse(value.ToString(), out money);
            return string.Format("{0:N0}", money);
        }
    }
}