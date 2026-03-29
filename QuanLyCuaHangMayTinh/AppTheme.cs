using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace QuanLyCuaHangMayTinh
{
    internal sealed class ThemePalette
    {
        public Color ShellColor { get; private set; }
        public Color NavigationColor { get; private set; }
        public Color NavigationHoverColor { get; private set; }
        public Color NavigationActiveColor { get; private set; }
        public Color PageColor { get; private set; }
        public Color CardColor { get; private set; }
        public Color InputColor { get; private set; }
        public Color AccentColor { get; private set; }
        public Color AccentSoftColor { get; private set; }
        public Color TextPrimary { get; private set; }
        public Color TextSecondary { get; private set; }
        public Color BorderColor { get; private set; }
        public Color GridAlternateRowColor { get; private set; }

        private ThemePalette()
        {
        }

        public static ThemePalette CreateFromConfiguration()
        {
            return new ThemePalette
            {
                ShellColor = ReadColor("Theme.ShellColor", Color.FromArgb(244, 247, 251)),
                NavigationColor = ReadColor("Theme.NavigationColor", Color.FromArgb(232, 238, 247)),
                NavigationHoverColor = ReadColor("Theme.NavigationHoverColor", Color.FromArgb(220, 228, 240)),
                NavigationActiveColor = ReadColor("Theme.NavigationActiveColor", Color.FromArgb(209, 221, 238)),
                PageColor = ReadColor("Theme.PageColor", Color.FromArgb(248, 250, 252)),
                CardColor = ReadColor("Theme.CardColor", Color.White),
                InputColor = ReadColor("Theme.InputColor", Color.White),
                AccentColor = ReadColor("Theme.AccentColor", Color.FromArgb(37, 99, 235)),
                AccentSoftColor = ReadColor("Theme.AccentSoftColor", Color.FromArgb(8, 145, 178)),
                TextPrimary = ReadColor("Theme.TextPrimary", Color.FromArgb(30, 41, 59)),
                TextSecondary = ReadColor("Theme.TextSecondary", Color.FromArgb(100, 116, 139)),
                BorderColor = ReadColor("Theme.BorderColor", Color.FromArgb(214, 223, 235)),
                GridAlternateRowColor = ReadColor("Theme.GridAlternateRowColor", Color.FromArgb(243, 247, 252))
            };
        }

        private static Color ReadColor(string key, Color fallback)
        {
            try
            {
                return ParseColor(ConfigurationManager.AppSettings[key], fallback);
            }
            catch
            {
                return fallback;
            }
        }

        private static Color ParseColor(string rawValue, Color fallback)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return fallback;
            }

            string value = rawValue.Trim();

            try
            {
                if (value.StartsWith("#", StringComparison.Ordinal))
                {
                    return ColorTranslator.FromHtml(value);
                }

                if (value.IndexOf(',') >= 0)
                {
                    string[] parts = value.Split(',');
                    if (parts.Length == 3)
                    {
                        return Color.FromArgb(
                            int.Parse(parts[0].Trim(), CultureInfo.InvariantCulture),
                            int.Parse(parts[1].Trim(), CultureInfo.InvariantCulture),
                            int.Parse(parts[2].Trim(), CultureInfo.InvariantCulture));
                    }

                    if (parts.Length == 4)
                    {
                        return Color.FromArgb(
                            int.Parse(parts[0].Trim(), CultureInfo.InvariantCulture),
                            int.Parse(parts[1].Trim(), CultureInfo.InvariantCulture),
                            int.Parse(parts[2].Trim(), CultureInfo.InvariantCulture),
                            int.Parse(parts[3].Trim(), CultureInfo.InvariantCulture));
                    }
                }

                if (value.Length == 6 || value.Length == 8)
                {
                    return ColorTranslator.FromHtml("#" + value);
                }

                Color named = Color.FromName(value);
                if (named.A > 0 || string.Equals(value, nameof(Color.Transparent), StringComparison.OrdinalIgnoreCase))
                {
                    return named;
                }
            }
            catch
            {
            }

            return fallback;
        }
    }

    internal static class AppTheme
    {
        public static ThemePalette Current { get; } = ThemePalette.CreateFromConfiguration();

        public static void Apply(Control root)
        {
            if (root == null)
            {
                return;
            }

            ApplyRecursive(root, root);
            AttachToControlTree(root, root);
        }

        public static void StylePrimaryButton(Button button)
        {
            if (button == null)
            {
                return;
            }

            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Current.AccentColor;
            button.ForeColor = Color.White;
            button.FlatAppearance.MouseOverBackColor = Lighten(Current.AccentColor, 0.12f);
            button.FlatAppearance.MouseDownBackColor = Darken(Current.AccentColor, 0.08f);
            button.Cursor = Cursors.Hand;
        }

        public static void StyleSecondaryButton(Button button)
        {
            if (button == null)
            {
                return;
            }

            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Current.BorderColor;
            button.FlatAppearance.MouseOverBackColor = Lighten(Current.NavigationHoverColor, 0.08f);
            button.FlatAppearance.MouseDownBackColor = Current.NavigationHoverColor;
            button.BackColor = Current.CardColor;
            button.ForeColor = Current.TextPrimary;
            button.Cursor = Cursors.Hand;
        }

        private static void AttachToControlTree(Control control, Control root)
        {
            control.ControlAdded -= OnControlAdded;
            control.ControlAdded += OnControlAdded;

            foreach (Control child in control.Controls)
            {
                AttachToControlTree(child, root);
            }
        }

        private static void OnControlAdded(object sender, ControlEventArgs e)
        {
            Control senderControl = sender as Control;
            Control root = senderControl != null ? (senderControl.FindForm() ?? senderControl) : e.Control;

            ApplyRecursive(e.Control, root ?? e.Control);
            AttachToControlTree(e.Control, root ?? e.Control);
        }

        private static void ApplyRecursive(Control control, Control root)
        {
            if (control == null)
            {
                return;
            }

            if (control is Form form)
            {
                form.BackColor = Current.PageColor;
                form.ForeColor = Current.TextPrimary;
            }
            else if (control is DataGridView grid)
            {
                StyleDataGridView(grid);
            }
            else if (control is Button button)
            {
                if (!IsManagedExternally(button, root))
                {
                    if (IsPrimaryButton(button))
                    {
                        StylePrimaryButton(button);
                    }
                    else
                    {
                        StyleSecondaryButton(button);
                    }
                }
            }
            else if (control is ComboBox comboBox)
            {
                StyleComboBox(comboBox);
            }
            else if (control is NumericUpDown numericUpDown)
            {
                StyleNumericUpDown(numericUpDown);
            }
            else if (control is DateTimePicker dateTimePicker)
            {
                StyleDateTimePicker(dateTimePicker);
            }
            else if (control is TextBoxBase textBox)
            {
                StyleTextBox(textBox);
            }
            else if (control is Label label)
            {
                StyleLabel(label);
            }
            else if (control is Panel panel)
            {
                StylePanel(panel);
            }
            else
            {
                StyleGenericControl(control);
            }

            foreach (Control child in control.Controls)
            {
                ApplyRecursive(child, root);
            }
        }

        private static void StylePanel(Panel panel)
        {
            if (panel.Parent is Form && panel.Dock == DockStyle.Fill)
            {
                panel.BackColor = Current.PageColor;
            }
            else
            {
                panel.BackColor = Current.CardColor;
            }
        }

        private static void StyleGenericControl(Control control)
        {
            if (control is PictureBox)
            {
                return;
            }

            if (string.Equals(control.GetType().Name, "ReportViewer", StringComparison.Ordinal))
            {
                return;
            }

            if (!(control is UserControl))
            {
                control.BackColor = Current.PageColor;
            }

            control.ForeColor = Current.TextPrimary;
        }

        private static void StyleLabel(Label label)
        {
            if (label == null)
            {
                return;
            }

            label.ForeColor = Current.TextPrimary;
            if (label.BackColor != Color.Transparent)
            {
                label.BackColor = Color.Transparent;
            }
        }

        private static void StyleTextBox(TextBoxBase textBox)
        {
            if (textBox == null)
            {
                return;
            }

            textBox.BackColor = Current.InputColor;
            textBox.ForeColor = Current.TextPrimary;
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void StyleComboBox(ComboBox comboBox)
        {
            if (comboBox == null)
            {
                return;
            }

            comboBox.BackColor = Current.InputColor;
            comboBox.ForeColor = Current.TextPrimary;
            comboBox.FlatStyle = FlatStyle.Flat;
        }

        private static void StyleNumericUpDown(NumericUpDown numericUpDown)
        {
            if (numericUpDown == null)
            {
                return;
            }

            numericUpDown.BackColor = Current.InputColor;
            numericUpDown.ForeColor = Current.TextPrimary;
            numericUpDown.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void StyleDateTimePicker(DateTimePicker dateTimePicker)
        {
            if (dateTimePicker == null)
            {
                return;
            }

            dateTimePicker.CalendarMonthBackground = Current.InputColor;
            dateTimePicker.CalendarForeColor = Current.TextPrimary;
            dateTimePicker.CalendarTitleBackColor = Current.AccentColor;
            dateTimePicker.CalendarTitleForeColor = Color.White;
            dateTimePicker.CalendarTrailingForeColor = Current.TextSecondary;
            dateTimePicker.Font = new Font(dateTimePicker.Font.FontFamily, dateTimePicker.Font.Size, dateTimePicker.Font.Style);
        }

        private static void StyleDataGridView(DataGridView grid)
        {
            if (grid == null)
            {
                return;
            }

            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = Current.PageColor;
            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = Current.BorderColor;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.BackColor = Current.CardColor;
            grid.DefaultCellStyle.ForeColor = Current.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = Lighten(Current.AccentColor, 0.78f);
            grid.DefaultCellStyle.SelectionForeColor = Current.TextPrimary;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Current.GridAlternateRowColor;
            grid.AlternatingRowsDefaultCellStyle.ForeColor = Current.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Current.NavigationColor;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Current.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Current.NavigationActiveColor;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Current.TextPrimary;
            grid.RowHeadersDefaultCellStyle.BackColor = Current.NavigationColor;
            grid.RowHeadersDefaultCellStyle.ForeColor = Current.TextPrimary;
            grid.RowHeadersDefaultCellStyle.SelectionBackColor = Current.NavigationActiveColor;
            grid.RowHeadersDefaultCellStyle.SelectionForeColor = Current.TextPrimary;
            grid.RowsDefaultCellStyle.BackColor = Current.CardColor;
            grid.RowsDefaultCellStyle.ForeColor = Current.TextPrimary;
            grid.RowsDefaultCellStyle.SelectionBackColor = Lighten(Current.AccentColor, 0.78f);
            grid.RowsDefaultCellStyle.SelectionForeColor = Current.TextPrimary;
            grid.RowTemplate.DefaultCellStyle.BackColor = Current.CardColor;
            grid.RowTemplate.DefaultCellStyle.ForeColor = Current.TextPrimary;
            grid.RowTemplate.DefaultCellStyle.SelectionBackColor = Lighten(Current.AccentColor, 0.78f);
            grid.RowTemplate.DefaultCellStyle.SelectionForeColor = Current.TextPrimary;
        }

        private static bool IsManagedExternally(Button button, Control root)
        {
            if (button == null)
            {
                return true;
            }

            if (root is fManager)
            {
                return true;
            }

            return false;
        }

        private static bool IsPrimaryButton(Button button)
        {
            if (button == null)
            {
                return false;
            }

            string name = button.Name ?? string.Empty;
            string text = button.Text ?? string.Empty;
            Color currentColor = button.BackColor;

            if (name.IndexOf("Login", StringComparison.OrdinalIgnoreCase) >= 0 ||
                name.IndexOf("Confirm", StringComparison.OrdinalIgnoreCase) >= 0 ||
                name.IndexOf("ThanhToan", StringComparison.OrdinalIgnoreCase) >= 0 ||
                name.IndexOf("Update", StringComparison.OrdinalIgnoreCase) >= 0 ||
                name.IndexOf("ShowReport", StringComparison.OrdinalIgnoreCase) >= 0 ||
                text.IndexOf("thanh toán", StringComparison.OrdinalIgnoreCase) >= 0 ||
                text.IndexOf("xác", StringComparison.OrdinalIgnoreCase) >= 0 ||
                text.IndexOf("báo cáo", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            return currentColor.R >= 180 && currentColor.G >= 90 && currentColor.B <= 140;
        }

        public static Color Lighten(Color color, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            return Color.FromArgb(
                color.A,
                color.R + (int)Math.Round((255 - color.R) * amount),
                color.G + (int)Math.Round((255 - color.G) * amount),
                color.B + (int)Math.Round((255 - color.B) * amount));
        }

        public static Color Darken(Color color, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            return Color.FromArgb(
                color.A,
                (int)Math.Round(color.R * (1f - amount)),
                (int)Math.Round(color.G * (1f - amount)),
                (int)Math.Round(color.B * (1f - amount)));
        }
    }
}
