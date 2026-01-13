using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public static class UIStyler
{
    public static void StyleControl(Control control, int radius = 12)
    {
        if (control == null) return;

        ApplyRoundedCorners(control, radius);

        switch (control)
        {
            case DataGridView dgv:
                StyleDataGridView(dgv);
                break;

            case TextBox txt:
                StyleTextBox(txt);
                break;

            //case Label lbl:
            //    StyleLabel(lbl);
            //    break;

            //case TableLayoutPanel tlp:
            //    StyleTableLayoutPanel(tlp, radius);
            //    break;

            case Panel pnl:
                StylePanel(pnl);
                break;
        }

        // 🔥 RECURSIVE: Style ALL child controls
        foreach (Control child in control.Controls)
        {
            StyleControl(child, radius);
        }
    }

    // ================= ROUNDED CORE =================

    private static void ApplyRoundedCorners(Control control, int radius)
    {
        if (control is TableLayoutPanel)
            return;

        control.Margin = new Padding(6);

        void apply(object s, EventArgs e)
        {
            Rectangle rect = control.ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using (GraphicsPath path = GetRoundedPath(rect, radius))
            {
                control.Region = new Region(path);
            }
        }

        control.Resize += apply;
        control.HandleCreated += apply;
    }

    private static GraphicsPath GetRoundedPath(Rectangle rect, int radius)
    {
        int d = radius * 2;
        GraphicsPath path = new GraphicsPath();

        path.StartFigure();
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();

        return path;
    }
    private static void StyleTextBox(TextBox txt)
    {
        txt.BorderStyle = BorderStyle.None;
        txt.Font = new Font("Segoe UI", 10F);
        txt.BackColor = Color.White;
    }

    private static void StyleLabel(Label lbl)
    {
        lbl.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lbl.ForeColor = Color.FromArgb(60, 60, 60);
    }

    private static void StylePanel(Panel pnl)
    {
        pnl.BackColor = Color.White;
        pnl.Padding = new Padding(10);
    }

    private static void StyleTableLayoutPanel(TableLayoutPanel tlp, int radius)
    {
        tlp.BackColor = Color.WhiteSmoke;
        tlp.Padding = new Padding(8);
        tlp.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

        // 🔥 CRITICAL FIX: disable clipping
        tlp.Resize += (s, e) =>
        {
            foreach (Control c in tlp.Controls)
            {
                c.Invalidate();
            }
        };
    }

    private static void StyleDataGridView(DataGridView dgv)
    {
        dgv.BorderStyle = BorderStyle.None;
        dgv.BackgroundColor = System.Drawing.SystemColors.Control;
        dgv.GridColor = System.Drawing.SystemColors.Control;
        dgv.EnableHeadersVisualStyles = false;

        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 94, 168);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 94, 168);
        dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

        dgv.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F);
        dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;

        dgv.RowHeadersVisible = false;
        dgv.RowTemplate.Height = 30;
    }
}
