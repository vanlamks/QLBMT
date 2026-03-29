using iTextSharp.text;
using iTextSharp.text.pdf;
using QuanLyCuaHangMayTinh.DAO;
using QuanLyCuaHangMayTinh.DTO;
using System;
using System.Data;
using System.IO;

namespace QuanLyCuaHangMayTinh.Code
{
    public class HoaDonNhapPdfExporter
    {
        private const string TenCuaHang = "TECHZONE";
        private const string DiaChiCuaHang = "108/37 Miếu Bình Đông";
        private const string DienThoaiCuaHang = "0355335544";

        public static string ExportHoaDonPdf(int idNhap, string filePath)
        {
            HoaDonNhap hoaDon = HoaDonNhapDAO.Instance.GetById(idNhap);
            DataTable dtChiTiet = ChiTietNhapDAO.Instance.GetDataTableHoaDonByIDNhap(idNhap);

            if (hoaDon == null)
                throw new Exception("Không tìm thấy hóa đơn nhập.");

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            Font titleFont = new Font(baseFont, 18, Font.BOLD);
            Font headerFont = new Font(baseFont, 13, Font.BOLD);
            Font normalFont = new Font(baseFont, 12, Font.NORMAL);
            Font boldFont = new Font(baseFont, 12, Font.BOLD);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document(PageSize.A4, 25, 25, 20, 20);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                PdfPTable top = new PdfPTable(2);
                top.WidthPercentage = 100;
                top.SetWidths(new float[] { 40, 60 });

                PdfPCell left = new PdfPCell();
                left.Border = Rectangle.NO_BORDER;
                left.AddElement(new Paragraph(TenCuaHang, headerFont));
                left.AddElement(new Paragraph("Địa chỉ: " + DiaChiCuaHang, normalFont));
                left.AddElement(new Paragraph("ĐT: " + DienThoaiCuaHang, normalFont));

                PdfPCell right = new PdfPCell();
                right.Border = Rectangle.NO_BORDER;
                right.HorizontalAlignment = Element.ALIGN_CENTER;
                right.AddElement(new Paragraph("HÓA ĐƠN NHẬP HÀNG", titleFont));
                right.AddElement(new Paragraph("Mặt hàng nhập: Máy tính", headerFont));

                top.AddCell(left);
                top.AddCell(right);
                doc.Add(top);

                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph("Mã hóa đơn: " + hoaDon.ID, normalFont));
                doc.Add(new Paragraph("Mã nhà cung cấp: " + hoaDon.MaNhaCungCap, normalFont));
                doc.Add(new Paragraph("Tên nhà cung cấp: " + hoaDon.TenNhaCungCap, normalFont));
                doc.Add(new Paragraph("Địa chỉ: " + hoaDon.DiaChiNhaCungCap, normalFont));
                doc.Add(new Paragraph("Người nhập hàng: " + hoaDon.NguoiNhapHang, normalFont));
                doc.Add(new Paragraph("Thời gian: " + hoaDon.ThoiGianNhapText, normalFont));
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 10, 35, 15, 20, 20 });

                AddHeaderCell(table, "STT", boldFont);
                AddHeaderCell(table, "TÊN HÀNG", boldFont);
                AddHeaderCell(table, "SỐ LƯỢNG", boldFont);
                AddHeaderCell(table, "ĐƠN GIÁ NHẬP", boldFont);
                AddHeaderCell(table, "THÀNH TIỀN", boldFont);

                foreach (DataRow row in dtChiTiet.Rows)
                {
                    AddBodyCell(table, row["STT"].ToString(), normalFont);
                    AddBodyCell(table, row["TenHang"].ToString(), normalFont);
                    AddBodyCell(table, row["SoLuong"].ToString(), normalFont);
                    AddBodyCell(table, FormatMoney(row["DonGia"]), normalFont);
                    AddBodyCell(table, FormatMoney(row["ThanhTien"]), normalFont);
                }

                doc.Add(table);
                doc.Add(new Paragraph(" "));

                Paragraph tongTien = new Paragraph("Thành tiền: " + FormatMoney(hoaDon.TongTien) + " VNĐ", boldFont);
                tongTien.Alignment = Element.ALIGN_LEFT;
                doc.Add(tongTien);

                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph("Ngày " + hoaDon.NgayNhap.Day +
                                      " tháng " + hoaDon.NgayNhap.Month +
                                      " năm " + hoaDon.NgayNhap.Year,
                                      normalFont)
                { Alignment = Element.ALIGN_RIGHT });

                doc.Add(new Paragraph(" "));

                PdfPTable sign = new PdfPTable(2);
                sign.WidthPercentage = 100;
                sign.SetWidths(new float[] { 50, 50 });

                PdfPCell nccCell = new PdfPCell();
                nccCell.Border = Rectangle.NO_BORDER;
                nccCell.HorizontalAlignment = Element.ALIGN_CENTER;
                nccCell.AddElement(new Paragraph("NHÀ CUNG CẤP", boldFont));
                nccCell.AddElement(new Paragraph("\n\n\n"));
                nccCell.AddElement(new Paragraph(hoaDon.TenNhaCungCap, normalFont));

                PdfPCell nvCell = new PdfPCell();
                nvCell.Border = Rectangle.NO_BORDER;
                nvCell.HorizontalAlignment = Element.ALIGN_CENTER;
                nvCell.AddElement(new Paragraph("NGƯỜI NHẬP HÀNG", boldFont));
                nvCell.AddElement(new Paragraph("\n\n\n"));
                nvCell.AddElement(new Paragraph(hoaDon.NguoiNhapHang, normalFont));

                sign.AddCell(nccCell);
                sign.AddCell(nvCell);

                doc.Add(sign);
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
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 6;
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