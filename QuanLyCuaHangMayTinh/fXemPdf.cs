using Microsoft.Web.WebView2.WinForms;
using System;
using System.Windows.Forms;

namespace QuanLyCuaHangMayTinh
{
    public partial class fXemPdf : Form
    {
        private readonly string pdfPath;
        private WebView2 webView;

        public fXemPdf(string pdfPath)
        {
            this.pdfPath = pdfPath;
            InitViewer();
        }

        private async void InitViewer()
        {
            this.Text = "Xem hóa đơn PDF";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;

            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            this.Controls.Add(webView);

            await webView.EnsureCoreWebView2Async(null);
            webView.Source = new Uri(pdfPath);
        }
    }
}