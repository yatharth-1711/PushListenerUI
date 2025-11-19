using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

public class RoundedComboBox : ComboBox
{
    public Color BorderColor { get; set; } = Color.DodgerBlue;
    public Color IconColor { get; set; } = Color.DodgerBlue;
    public int BorderSize { get; set; } = 1;
    public int BorderRadius { get; set; } = 8;

    public RoundedComboBox()
    {
        DrawMode = DrawMode.OwnerDrawFixed;
        DropDownStyle = ComboBoxStyle.DropDownList;
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index < 0) return;

        e.DrawBackground();
        bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
        Color bgColor = selected ? Color.FromArgb(230, 230, 230) : BackColor;
        Color textColor = selected ? Color.Black : ForeColor;

        using (SolidBrush bgBrush = new SolidBrush(bgColor))
            e.Graphics.FillRectangle(bgBrush, e.Bounds);

        using (SolidBrush textBrush = new SolidBrush(textColor))
            e.Graphics.DrawString(Items[e.Index].ToString(), Font, textBrush, e.Bounds.X + 2, e.Bounds.Y + 3);

        e.DrawFocusRectangle();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using (GraphicsPath path = GetPath(rect, BorderRadius))
        using (Pen pen = new Pen(BorderColor, BorderSize))
        {
            g.DrawPath(pen, path);
        }
    }

    private GraphicsPath GetPath(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        float curve = radius * 2f;

        path.AddArc(rect.X, rect.Y, curve, curve, 180, 90);
        path.AddArc(rect.Right - curve, rect.Y, curve, curve, 270, 90);
        path.AddArc(rect.Right - curve, rect.Bottom - curve, curve, curve, 0, 90);
        path.AddArc(rect.X, rect.Bottom - curve, curve, curve, 90, 90);
        path.CloseFigure();

        return path;
    }


    public class SmartComboBox : ComboBox
    {
        private string _placeholder = "Select value...";
        private bool _isPlaceholder = true;
        private List<string> originalItems = new List<string>();

        [Category("Custom")]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; Invalidate(); }
        }

        public Color BorderColor { get; set; } = Color.DodgerBlue;
        public int BorderRadius { get; set; } = 8;
        public int BorderSize { get; set; } = 1;

        public SmartComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDown;
            this.TextChanged += HandleSearch;
            this.GotFocus += RemovePlaceholder;
            this.LostFocus += SetPlaceholder;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            SetPlaceholder(null, null);
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (_isPlaceholder)
            {
                Text = "";
                ForeColor = Color.Black;
                _isPlaceholder = false;
            }
        }

        private void SetPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                _isPlaceholder = true;
                Text = _placeholder;
                ForeColor = Color.Gray;
            }
        }

        private void HandleSearch(object sender, EventArgs e)
        {
            if (_isPlaceholder) return;

            string filter = Text.ToLower();
            if (originalItems.Count == 0)
                originalItems = Items.Cast<string>().ToList();

            var filtered = originalItems.Where(i => i.ToLower().Contains(filter)).ToArray();
            Items.Clear();
            Items.AddRange(filtered);
            DroppedDown = true;
            Cursor.Current = Cursors.Default;
            SelectionStart = Text.Length;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();
            Color textColor = (_isPlaceholder ? Color.Gray : Color.Black);
            using (SolidBrush brush = new SolidBrush(textColor))
                e.Graphics.DrawString(Items[e.Index].ToString(), Font, brush, e.Bounds);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            using (GraphicsPath path = GetRoundPath(rect, BorderRadius))
            using (Pen pen = new Pen(BorderColor, BorderSize))
            {
                g.DrawPath(pen, path);
            }
        }

        private GraphicsPath GetRoundPath(Rectangle r, int radius)
        {
            float curve = radius * 2f;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(r.X, r.Y, curve, curve, 180, 90);
            path.AddArc(r.Right - curve, r.Y, curve, curve, 270, 90);
            path.AddArc(r.Right - curve, r.Bottom - curve, curve, curve, 0, 90);
            path.AddArc(r.X, r.Bottom - curve, curve, curve, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
