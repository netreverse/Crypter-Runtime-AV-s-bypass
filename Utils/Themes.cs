using System.Threading;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Drawing;
using System;
using System.Windows.Forms;
using static Helpers;
using System.IO;

static internal class Helpers
{

    public static Graphics G;

    private static SizeF TargetStringMeasure;
    public enum MouseState : byte
    {
        None = 0,
        Over = 1,
        Down = 2
    }

    public enum RoundingStyle : byte
    {
        All = 0,
        Top = 1,
        Bottom = 2,
        Left = 3,
        Right = 4,
        TopRight = 5,
        BottomRight = 6
    }
    public static Rectangle FullRectangle(Size S, bool Subtract)
    {
        Rectangle result;
        if (Subtract)
        {
            result = checked(new Rectangle(0, 0, S.Width - 1, S.Height - 1));
        }
        else
        {
            result = new Rectangle(0, 0, S.Width, S.Height);
        }
        return result;
    }
    public static Color ColorFromHex(string Hex)
    {
        return Color.FromArgb(Convert.ToInt32(long.Parse(string.Format("FFFFFFFFFF{0}", Hex.Substring(1)), System.Globalization.NumberStyles.HexNumber)));
    }

    public static Point MiddlePoint(string TargetText, Font TargetFont, Rectangle Rect)
    {
        TargetStringMeasure = G.MeasureString(TargetText, TargetFont);
        return new Point(Convert.ToInt32(Rect.Width / 2 - TargetStringMeasure.Width / 2), Convert.ToInt32(Rect.Height / 2 - TargetStringMeasure.Height / 2));
    }

    public static GraphicsPath RoundRect(Rectangle Rect, int Rounding, RoundingStyle Style = RoundingStyle.All)
    {

        GraphicsPath GP = new GraphicsPath();
        int AW = Rounding * 2;

        GP.StartFigure();

        if (Rounding == 0)
        {
            GP.AddRectangle(Rect);
            GP.CloseAllFigures();
            return GP;
        }

        switch (Style)
        {
            case RoundingStyle.All:
                GP.AddArc(new Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90);
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90);
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90);
                GP.AddArc(new Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90);
                break;
            case RoundingStyle.Top:
                GP.AddArc(new Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90);
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90);
                GP.AddLine(new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y + Rect.Height));
                break;
            case RoundingStyle.Bottom:
                GP.AddLine(new Point(Rect.X, Rect.Y), new Point(Rect.X + Rect.Width, Rect.Y));
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90);
                GP.AddArc(new Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90);
                break;
            case RoundingStyle.Left:
                GP.AddArc(new Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90);
                GP.AddLine(new Point(Rect.X + Rect.Width, Rect.Y), new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height));
                GP.AddArc(new Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90);
                break;
            case RoundingStyle.Right:
                GP.AddLine(new Point(Rect.X, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y));
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90);
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90);
                break;
            case RoundingStyle.TopRight:
                GP.AddLine(new Point(Rect.X, Rect.Y + 1), new Point(Rect.X, Rect.Y));
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90);
                GP.AddLine(new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height - 1), new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height));
                GP.AddLine(new Point(Rect.X + 1, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y + Rect.Height));
                break;
            case RoundingStyle.BottomRight:
                GP.AddLine(new Point(Rect.X, Rect.Y + 1), new Point(Rect.X, Rect.Y));
                GP.AddLine(new Point(Rect.X + Rect.Width - 1, Rect.Y), new Point(Rect.X + Rect.Width, Rect.Y));
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90);
                GP.AddLine(new Point(Rect.X + 1, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y + Rect.Height));
                break;
        }

        GP.CloseAllFigures();

        return GP;

    }

}

public class BoosterButton : Button
{

    private MouseState State;

    private LinearGradientBrush Gradient;
    public BoosterButton()
    {
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 9);
        ForeColor = Helpers.ColorFromHex("#B6B6B6");
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
    }


    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.G = e.Graphics;
        Helpers.G.SmoothingMode = SmoothingMode.HighQuality;
        Helpers.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        base.OnPaint(e);

        Helpers.G.Clear(Parent.BackColor);


        if (Enabled)
        {
            switch (State)
            {
                case MouseState.None:
                    Gradient = new LinearGradientBrush(new Rectangle(0, 0, Width - 1, Height - 1), Helpers.ColorFromHex("#606060"), Helpers.ColorFromHex("#4E4E4E"), LinearGradientMode.Vertical);

                    break;
                case MouseState.Over:
                    Gradient = new LinearGradientBrush(new Rectangle(0, 0, Width - 1, Height - 1), Helpers.ColorFromHex("#6A6A6A"), Helpers.ColorFromHex("#585858"), LinearGradientMode.Vertical);

                    break;
                case MouseState.Down:
                    Gradient = new LinearGradientBrush(new Rectangle(0, 0, Width - 1, Height - 1), Helpers.ColorFromHex("#565656"), Helpers.ColorFromHex("#444444"), LinearGradientMode.Vertical);

                    break;
            }

            Helpers.G.FillPath(Gradient, Helpers.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));

            using (Pen Border = new Pen(Helpers.ColorFromHex("#323232")))
            {
                Helpers.G.DrawPath(Border, Helpers.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
            }

            //// Top Line
            switch (State)
            {

                case MouseState.None:

                    using (Pen TopLine = new Pen(Helpers.ColorFromHex("#737373")))
                    {
                        Helpers.G.DrawLine(TopLine, 4, 1, Width - 4, 1);
                    }


                    break;
                case MouseState.Over:

                    using (Pen TopLine = new Pen(Helpers.ColorFromHex("#7D7D7D")))
                    {
                        Helpers.G.DrawLine(TopLine, 4, 1, Width - 4, 1);
                    }


                    break;
                case MouseState.Down:

                    using (Pen TopLine = new Pen(Helpers.ColorFromHex("#696969")))
                    {
                        Helpers.G.DrawLine(TopLine, 4, 1, Width - 4, 1);
                    }


                    break;
            }

            using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#F5F5F5")))
            {
                using (Font TextFont = new Font("Segoe UI", 9))
                {
                    Helpers.G.DrawString(Text, TextFont, TextBrush, Helpers.MiddlePoint(Text, TextFont, new Rectangle(0, 0, Width + 2, Height)));
                }
            }


        }
        else
        {
            Gradient = new LinearGradientBrush(new Rectangle(0, 0, Width - 1, Height - 1), Helpers.ColorFromHex("#4C4C4C"), Helpers.ColorFromHex("#3A3A3A"), LinearGradientMode.Vertical);

            Helpers.G.FillPath(Gradient, Helpers.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));

            using (Pen Border = new Pen(Helpers.ColorFromHex("#323232")))
            {
                Helpers.G.DrawPath(Border, Helpers.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
            }

            using (Pen TopLine = new Pen(Helpers.ColorFromHex("#5F5F5F")))
            {
                Helpers.G.DrawLine(TopLine, 4, 1, Width - 4, 1);
            }

            using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#818181")))
            {
                using (Font TextFont = new Font("Segoe UI", 9))
                {
                    Helpers.G.DrawString(Text, TextFont, TextBrush, Helpers.MiddlePoint(Text, TextFont, new Rectangle(0, 0, Width + 2, Height)));
                }
            }

        }

    }

    protected override void OnMouseEnter(EventArgs e)
    {
        State = MouseState.Over;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        State = MouseState.None;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        State = MouseState.Down;
        Invalidate();
        base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        State = MouseState.Over;
        Invalidate();
        base.OnMouseUp(e);
    }

}

public class BoosterHeader : Control
{


    private SizeF TextMeasure;
    public BoosterHeader()
    {
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 10);
        ForeColor = Helpers.ColorFromHex("#C0C0C0");
    }


    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.G = e.Graphics;
        Helpers.G.SmoothingMode = SmoothingMode.HighQuality;
        Helpers.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        Helpers.G.Clear(Parent.BackColor);

        using (Pen Line = new Pen(Helpers.ColorFromHex("#5C5C5C")))
        {
            Helpers.G.DrawLine(Line, 0, 6, Width - 1, 6);
        }

        using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#D4D4D4")))
        {
            using (Font TextFont = new Font("Segoe UI", 10))
            {
                using (SolidBrush ParentFill = new SolidBrush(Parent.BackColor))
                {
                    TextMeasure = Helpers.G.MeasureString(Text, TextFont);
                    Helpers.G.FillRectangle(ParentFill, new Rectangle(14, -4, Convert.ToInt32(TextMeasure.Width + 8), Convert.ToInt32(TextMeasure.Height + 4)));
                    Helpers.G.DrawString(Text, TextFont, TextBrush, new Point(20, -4));
                }
            }
        }

        base.OnPaint(e);
    }

    protected override void OnResize(EventArgs e)
    {
        Size = new Size(Width, 14);
        base.OnResize(e);
    }

}

public class BoosterToolTip : ToolTip
{

    public BoosterToolTip()
    {
        OwnerDraw = true;
        BackColor = Helpers.ColorFromHex("#242424");
        Draw += OnDraw;
    }


    private void OnDraw(object sender, DrawToolTipEventArgs e)
    {
        Helpers.G = e.Graphics;
        Helpers.G.SmoothingMode = SmoothingMode.HighQuality;
        Helpers.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        Helpers.G.Clear(Helpers.ColorFromHex("#242424"));

        using (Pen Border = new Pen(Helpers.ColorFromHex("#343434")))
        {
            Helpers.G.DrawRectangle(Border, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));
        }


        if (ToolTipIcon == ToolTipIcon.None)
        {
            using (Font TextFont = new Font("Segoe UI", 9))
            {
                using (SolidBrush TextBrush = new SolidBrush(Helpers.ColorFromHex("#B6B6B6")))
                {
                    Helpers.G.DrawString(e.ToolTipText, TextFont, TextBrush, new PointF(e.Bounds.X + 4, e.Bounds.Y + 1));
                }
            }


        }
        else
        {
            switch (ToolTipIcon)
            {

                case ToolTipIcon.Info:

                    using (Font TextFont = new Font("Segoe UI", 9, FontStyle.Bold))
                    {
                        using (SolidBrush TextBrush = new SolidBrush(Helpers.ColorFromHex("#7FD88B")))
                        {
                            Helpers.G.DrawString("Information", TextFont, TextBrush, new PointF(e.Bounds.X + 4, e.Bounds.Y + 2));
                        }
                    }


                    break;
                case ToolTipIcon.Warning:

                    using (Font TextFont = new Font("Segoe UI", 9, FontStyle.Bold))
                    {
                        using (SolidBrush TextBrush = new SolidBrush(Helpers.ColorFromHex("#D8C67F")))
                        {
                            Helpers.G.DrawString("Warning", TextFont, TextBrush, new PointF(e.Bounds.X + 4, e.Bounds.Y + 2));
                        }
                    }


                    break;
                case ToolTipIcon.Error:

                    using (Font TextFont = new Font("Segoe UI", 9, FontStyle.Bold))
                    {
                        using (SolidBrush TextBrush = new SolidBrush(Helpers.ColorFromHex("#D87F7F")))
                        {
                            Helpers.G.DrawString("Error", TextFont, TextBrush, new PointF(e.Bounds.X + 4, e.Bounds.Y + 2));
                        }
                    }


                    break;
            }

            using (Font TextFont = new Font("Segoe UI", 9))
            {
                using (SolidBrush TextBrush = new SolidBrush(Helpers.ColorFromHex("#B6B6B6")))
                {
                    Helpers.G.DrawString(e.ToolTipText, TextFont, TextBrush, new PointF(e.Bounds.X + 4, e.Bounds.Y + 15));
                }
            }

        }

    }

}
public class XylosNotice : TextBox
{
    private Graphics G;

    private string B64;

    public XylosNotice()
    {
        this.B64 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAABL0lEQVQ4T5VT0VGDQBB9e2cBdGBSgTIDEr9MCw7pI0kFtgB9yFiC+KWMmREqMOnAAuDWOfAiudzhyA/svtvH7Xu7BOv5eH2atVKtwbwk0LWGGVyDqLzoRB7e3u/HJTQOdm+PGYjWNuk4ZkIW36RbkzsS7KqiBnB1Usw49DHh8oQEXMfJKhwgAM4/Mw7RIp0NeLG3ScCcR4vVhnTPnVCf9rUZeImTdKnz71VREnBnn5FKzMnX95jA2V6vLufkBQFESTq0WBXsEla7owmcoC6QJMKW2oCUePY5M0lAjK0iBAQ8TBGc2/d7+uvnM/AQNF4Rp4bpiGkRfTb2Gigx12+XzQb3D9JfBGaQzHWm7HS000RJ2i/av5fJjPDZMplErwl1GxDpMTbL1YC5lCwze52/AQFekh7wKBpGAAAAAElFTkSuQmCC";
        this.DoubleBuffered = true;
        base.Enabled = false;
        base.ReadOnly = true;
        base.BorderStyle = BorderStyle.None;
        this.Multiline = true;
        this.Cursor = Cursors.Default;
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        base.SetStyle(ControlStyles.UserPaint, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        this.G = e.Graphics;
        this.G.SmoothingMode = SmoothingMode.HighQuality;
        this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        base.OnPaint(e);
        this.G.Clear(Color.White);
        using (SolidBrush solidBrush = new SolidBrush(Helpers.ColorFromHex("#FFFDE8")))
        {
            using (Pen pen = new Pen(Helpers.ColorFromHex("#F2F3F7")))
            {
                using (SolidBrush solidBrush2 = new SolidBrush(Helpers.ColorFromHex("#B9B595")))
                {
                    using (Font font = new Font("Segoe UI", 9f))
                    {
                        this.G.FillPath(solidBrush, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 3, Helpers.RoundingStyle.All));
                        this.G.DrawPath(pen, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 3, Helpers.RoundingStyle.All));
                        this.G.DrawString(this.Text, font, solidBrush2, new Point(30, 6));
                    }
                }
            }
        }
        using (Image image = Image.FromStream(new MemoryStream(Convert.FromBase64String(this.B64))))
        {
            this.G.DrawImage(image, new Rectangle(8, checked((int)Math.Round(unchecked((double)base.Height / 2.0 - 8.0))), 16, 16));
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
    }
}
[DefaultEvent("TextChanged")]
public class BoosterTextBox : Control
{

    private TextBox withEventsField_T;

    private TextBox T
    {
        get { return withEventsField_T; }
        set
        {
            withEventsField_T = value;
            T.Text = withEventsField_T.Text;
        }
    }

    private MouseState State;
    public new string Text
    {
        get { return T.Text; }
        set
        {
            base.Text = value;
            T.Text = value;
            Invalidate();
        }
    }

    public new bool Enabled
    {
        get { return T.Enabled; }
        set
        {
            T.Enabled = value;
            Invalidate();
        }
    }

    public bool UseSystemPasswordChar
    {
        get { return T.UseSystemPasswordChar; }
        set
        {
            T.UseSystemPasswordChar = value;
            Invalidate();
        }
    }

    public bool MultiLine
    {
        get { return T.Multiline; }
        set
        {
            T.Multiline = value;
            Size = new Size(T.Width + 2, T.Height + 2);
            Invalidate();
        }
    }

    public new bool ReadOnly
    {
        get { return T.ReadOnly; }
        set
        {
            T.ReadOnly = value;
            Invalidate();
        }
    }

    public BoosterTextBox()
    {
        DoubleBuffered = true;

        T = new TextBox
        {
            BorderStyle = BorderStyle.None,
            BackColor = Helpers.ColorFromHex("#242424"),
            ForeColor = Helpers.ColorFromHex("#B6B6B6"),
            Location = new Point(1, 1),
            Multiline = true
        };

        Controls.Add(T);
    }


    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.G = e.Graphics;
        Helpers.G.SmoothingMode = SmoothingMode.HighQuality;
        Helpers.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        if (Enabled)
        {
            T.BackColor = Helpers.ColorFromHex("#242424");

            switch (State)
            {

                case MouseState.Down:

                    using (Pen Border = new Pen(Helpers.ColorFromHex("#C8C8C8")))
                    {
                        Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, Width - 1, Height - 1));
                    }


                    break;
                default:

                    using (Pen Border = new Pen(Helpers.ColorFromHex("#5C5C5C")))
                    {
                        Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, Width - 1, Height - 1));
                    }


                    break;
            }


        }
        else
        {
            T.BackColor = Helpers.ColorFromHex("#282828");

            using (Pen Border = new Pen(Helpers.ColorFromHex("#484848")))
            {
                Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, Width - 1, Height - 1));
            }

        }

        base.OnPaint(e);

    }

    protected override void OnEnter(EventArgs e)
    {
        State = MouseState.Down;
        Invalidate();
        base.OnEnter(e);
    }

    protected override void OnLeave(EventArgs e)
    {
        State = MouseState.None;
        Invalidate();
        base.OnLeave(e);
    }

    protected override void OnResize(EventArgs e)
    {
        if (MultiLine)
        {
            T.Size = new Size(Width - 2, Height - 2);
            Invalidate();
        }
        else
        {
            T.Size = new Size(Width - 2, T.Height);
            Size = new Size(Width, T.Height + 2);
        }
        base.OnResize(e);
    }

    private void TTextChanged()
    {
        base.OnTextChanged(EventArgs.Empty);
    }

}

public class BoosterComboBox : ComboBox
{

    private MouseState State;

    private Rectangle Rect;
    private string ItemString = string.Empty;

    private string FirstItem = string.Empty;
    public BoosterComboBox()
    {
        ItemHeight = 20;
        DoubleBuffered = true;
        BackColor = Color.FromArgb(36, 36, 36);
        DropDownStyle = ComboBoxStyle.DropDownList;
        DrawMode = DrawMode.OwnerDrawFixed;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
    }


    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.G = e.Graphics;
        Helpers.G.SmoothingMode = SmoothingMode.HighQuality;
        Helpers.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        base.OnPaint(e);

        Helpers.G.Clear(Parent.BackColor);


        if (Enabled)
        {
            using (SolidBrush Fill = new SolidBrush(Helpers.ColorFromHex("#242424")))
            {
                Helpers.G.FillRectangle(Fill, new Rectangle(0, 0, Width - 1, Height - 1));
            }

            switch (State)
            {

                case MouseState.None:

                    using (Pen Border = new Pen(Helpers.ColorFromHex("#5C5C5C")))
                    {
                        Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, Width - 1, Height - 1));
                    }


                    break;
                case MouseState.Over:

                    using (Pen Border = new Pen(Helpers.ColorFromHex("#C8C8C8")))
                    {
                        Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, Width - 1, Height - 1));
                    }


                    break;
            }

            using (Font ArrowFont = new Font("Marlett", 12))
            {
                using (SolidBrush ArrowBrush = new SolidBrush(Helpers.ColorFromHex("#909090")))
                {
                    Helpers.G.DrawString("6", ArrowFont, ArrowBrush, new Point(Width - 20, 5));
                }
            }


            if ((Items != null))
            {
                try
                {
                    FirstItem = GetItemText(Items[0]);
                }
                catch
                {
                }


                if (!(SelectedIndex == -1))
                {
                    using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#B6B6B6")))
                    {
                        using (Font TextFont = new Font("Segoe UI", 9))
                        {
                            Helpers.G.DrawString(ItemString, TextFont, TextBrush, new Point(4, 4));
                        }
                    }


                }
                else
                {
                    using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#B6B6B6")))
                    {
                        using (Font TextFont = new Font("Segoe UI", 9))
                        {
                            Helpers.G.DrawString(FirstItem, TextFont, TextBrush, new Point(4, 4));
                        }
                    }

                }


            }


        }
        else
        {
            using (SolidBrush Fill = new SolidBrush(ColorFromHex("#282828")))
            {
                using (Pen Border = new Pen(Helpers.ColorFromHex("#484848")))
                {
                    Helpers.G.FillRectangle(Fill, new Rectangle(0, 0, Width - 1, Height - 1));
                    Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, Width - 1, Height - 1));
                }
            }

            using (Font ArrowFont = new Font("Marlett", 12))
            {
                using (SolidBrush ArrowBrush = new SolidBrush(Helpers.ColorFromHex("#707070")))
                {
                    Helpers.G.DrawString("6", ArrowFont, ArrowBrush, new Point(Width - 20, 5));
                }
            }


            if ((Items != null))
            {
                try
                {
                    FirstItem = GetItemText(Items[0]);
                }
                catch
                {
                }

                using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#818181")))
                {
                    using (Font TextFont = new Font("Segoe UI", 9))
                    {
                        Helpers.G.DrawString(FirstItem, TextFont, TextBrush, new Point(4, 4));
                    }
                }

            }

        }

    }


    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        Helpers.G = e.Graphics;
        Helpers.G.SmoothingMode = SmoothingMode.HighQuality;
        Helpers.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        Rect = e.Bounds;

        using (SolidBrush Back = new SolidBrush(Helpers.ColorFromHex("#242424")))
        {
            Helpers.G.FillRectangle(Back, new Rectangle(e.Bounds.X - 4, e.Bounds.Y - 1, e.Bounds.Width + 4, e.Bounds.Height - 1));
        }

        if (!(e.Index == -1))
        {
            ItemString = GetItemText(Items[e.Index]);
        }

        using (Font ItemsFont = new Font("Segoe UI", 9))
        {
            using (Pen Border = new Pen(Helpers.ColorFromHex("#D0D5D9")))
            {


                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    using (SolidBrush HoverItemBrush = new SolidBrush(Helpers.ColorFromHex("#F5F5F5")))
                    {
                        Helpers.G.DrawString(ItemString, new Font("Segoe UI", 9), HoverItemBrush, new Point(Rect.X + 5, Rect.Y + 1));
                    }


                }
                else
                {
                    using (SolidBrush DefaultItemBrush = new SolidBrush(Helpers.ColorFromHex("#C0C0C0")))
                    {
                        Helpers.G.DrawString(ItemString, new Font("Segoe UI", 9), DefaultItemBrush, new Point(Rect.X + 5, Rect.Y + 1));
                    }

                }

            }
        }

        e.DrawFocusRectangle();

        base.OnDrawItem(e);

    }

    protected override void OnSelectedItemChanged(EventArgs e)
    {
        Invalidate();
        base.OnSelectedItemChanged(e);
    }

    protected override void OnSelectedIndexChanged(EventArgs e)
    {
        State = MouseState.None;
        Invalidate();
        base.OnSelectedIndexChanged(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        State = MouseState.Over;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        State = MouseState.None;
        Invalidate();
        base.OnMouseLeave(e);
    }

}

public class BoosterCheckBox : CheckBox
{

    private MouseState State;

    private bool Block;
    private Thread CheckThread;
    private Thread UncheckThread;

    private Rectangle OverFillRect = new Rectangle(1, 1, 14, 14);
    public BoosterCheckBox()
    {
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 9);
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
    }


    private void CheckAnimation()
    {
        Block = true;

        int X = 1;
        int Rectw = 15;

        while (!(OverFillRect.Width == 0))
        {
            X += 1;
            Rectw -= 1;
            OverFillRect = new Rectangle(X, OverFillRect.Y, Rectw, OverFillRect.Height);
            Invalidate();
            Thread.Sleep(30);
        }

        Block = false;

    }


    private void UncheckAnimation()
    {
        Block = true;

        int X = 15;
        int Rectw = 0;

        while (!(OverFillRect.Width == 14))
        {
            X -= 1;
            Rectw += 1;
            OverFillRect = new Rectangle(X, OverFillRect.Y, Rectw, OverFillRect.Height);
            Invalidate();
            Thread.Sleep(30);
        }

        Block = false;

    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.G = e.Graphics;
        Helpers.G.SmoothingMode = SmoothingMode.HighQuality;
        Helpers.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        base.OnPaint(e);

        Helpers.G.Clear(Parent.BackColor);


        if (Enabled)
        {
            using (SolidBrush Fill = new SolidBrush(ColorFromHex("#242424")))
            {
                using (Pen Border = new Pen(Helpers.ColorFromHex("#5C5C5C")))
                {
                    Helpers.G.FillRectangle(Fill, new Rectangle(0, 0, 16, 16));
                    Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, 16, 16));
                }
            }

            switch (State)
            {

                case MouseState.None:

                    using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#B6B6B6")))
                    {
                        using (Font TextFont = new Font("Segoe UI", 9))
                        {
                            Helpers.G.DrawString(Text, TextFont, TextBrush, new Point(25, -1));
                        }
                    }


                    break;
                case MouseState.Over:

                    using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#F5F5F5")))
                    {
                        using (Font TextFont = new Font("Segoe UI", 9))
                        {
                            Helpers.G.DrawString(Text, TextFont, TextBrush, new Point(25, -1));
                        }
                    }


                    break;
            }

            using (Font CheckFont = new Font("Marlett", 12))
            {
                using (SolidBrush CheckBrush = new SolidBrush(Color.FromArgb(144, 144, 144)))
                {
                    Helpers.G.DrawString("b", CheckFont, CheckBrush, new Point(-2, 1));
                }
            }

            using (SolidBrush Fill = new SolidBrush(Helpers.ColorFromHex("#242424")))
            {
                Helpers.G.SmoothingMode = SmoothingMode.None;
                Helpers.G.FillRectangle(Fill, OverFillRect);
            }


        }
        else
        {
            using (SolidBrush Fill = new SolidBrush(ColorFromHex("#282828")))
            {
                using (Pen Border = new Pen(Helpers.ColorFromHex("#484848")))
                {
                    Helpers.G.FillRectangle(Fill, new Rectangle(0, 0, 16, 16));
                    Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, 16, 16));
                }
            }

            using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#818181")))
            {
                using (Font TextFont = new Font("Segoe UI", 9))
                {
                    Helpers.G.DrawString(Text, TextFont, TextBrush, new Point(25, -1));
                }
            }

            using (Font CheckFont = new Font("Marlett", 12))
            {
                using (SolidBrush CheckBrush = new SolidBrush(Helpers.ColorFromHex("#707070")))
                {
                    Helpers.G.DrawString("b", CheckFont, CheckBrush, new Point(-2, 1));
                }
            }

            using (SolidBrush Fill = new SolidBrush(Helpers.ColorFromHex("#282828")))
            {
                Helpers.G.SmoothingMode = SmoothingMode.None;
                Helpers.G.FillRectangle(Fill, OverFillRect);
            }

        }

    }

    protected override void OnMouseEnter(EventArgs e)
    {
        State = MouseState.Over;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        State = MouseState.None;
        Invalidate();
        base.OnMouseLeave(e);
    }


    protected override void OnCheckedChanged(EventArgs e)
    {
        if (Checked)
        {
            CheckThread = new Thread(CheckAnimation) { IsBackground = true };
            CheckThread.Start();
        }
        else
        {
            UncheckThread = new Thread(UncheckAnimation) { IsBackground = true };
            UncheckThread.Start();
        }

        if (!Block)
        {
            base.OnCheckedChanged(e);
        }

    }

}

public class BoosterTabControl : TabControl
{

    private Rectangle MainRect;

    private Rectangle OverRect;

    private int SubOverIndex = -1;
    private bool Hovering
    {
        get { return !(OverIndex == -1); }
    }

    private int OverIndex
    {
        get { return SubOverIndex; }
        set
        {
            SubOverIndex = value;
            if (!(SubOverIndex == -1))
            {
                OverRect = GetTabRect(OverIndex);
            }
            Invalidate();
        }
    }

    public BoosterTabControl()
    {
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 10);
        ForeColor = Helpers.ColorFromHex("#78797B");
        ItemSize = new Size(40, 170);
        SizeMode = TabSizeMode.Fixed;
        Alignment = TabAlignment.Left;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override void CreateHandle()
    {
        foreach (TabPage Tab in TabPages)
        {
            Tab.BackColor = Helpers.ColorFromHex("#424242");
            Tab.ForeColor = Helpers.ColorFromHex("#B6B6B6");
            Tab.Font = new Font("Segoe UI", 9);
        }
        base.CreateHandle();
    }


    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.G = e.Graphics;
        Helpers.G.SmoothingMode = SmoothingMode.HighQuality;
        Helpers.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        Helpers.G.Clear(Helpers.ColorFromHex("#333333"));

        using (Pen Border = new Pen(Helpers.ColorFromHex("#292929")))
        {
            Helpers.G.SmoothingMode = SmoothingMode.None;
            Helpers.G.DrawLine(Border, ItemSize.Height + 3, 4, ItemSize.Height + 3, Height - 5);
        }


        for (int I = 0; I <= TabPages.Count - 1; I++)
        {
            MainRect = GetTabRect(I);


            if (SelectedIndex == I)
            {
                using (SolidBrush Selection = new SolidBrush(Helpers.ColorFromHex("#424242")))
                {
                    Helpers.G.FillRectangle(Selection, new Rectangle(MainRect.X - 6, MainRect.Y + 2, MainRect.Width + 8, MainRect.Height - 1));
                }

                using (SolidBrush SelectionLeft = new SolidBrush(Helpers.ColorFromHex("#F63333")))
                {
                    Helpers.G.FillRectangle(SelectionLeft, new Rectangle(MainRect.X - 2, MainRect.Y + 2, 3, MainRect.Height - 1));
                }

                using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#F5F5F5")))
                {
                    using (Font TextFont = new Font("Segoe UI", 10))
                    {
                        Helpers.G.DrawString(TabPages[I].Text, TextFont, TextBrush, new Point(MainRect.X + 25, MainRect.Y + 11));
                    }
                }


            }
            else
            {
                using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#C0C0C0")))
                {
                    using (Font TextFont = new Font("Segoe UI", 10))
                    {
                        Helpers.G.DrawString(TabPages[I].Text, TextFont, TextBrush, new Point(MainRect.X + 25, MainRect.Y + 11));
                    }
                }

            }


            if (Hovering)
            {
                using (SolidBrush Selection = new SolidBrush(Helpers.ColorFromHex("#383838")))
                {
                    Helpers.G.FillRectangle(Selection, new Rectangle(OverRect.X - 6, OverRect.Y + 2, OverRect.Width + 8, OverRect.Height - 1));
                }

                using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#C0C0C0")))
                {
                    using (Font TextFont = new Font("Segoe UI", 10))
                    {
                        Helpers.G.DrawString(TabPages[OverIndex].Text, TextFont, TextBrush, new Point(OverRect.X + 25, OverRect.Y + 11));
                    }
                }

            }

        }

        base.OnPaint(e);

    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        for (int I = 0; I <= TabPages.Count - 1; I++)
        {
            if (GetTabRect(I).Contains(e.Location) & !(SelectedIndex == I))
            {
                OverIndex = I;
                break; // TODO: might not be correct. Was : Exit For
            }
            else
            {
                OverIndex = -1;
            }
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        OverIndex = -1;
        base.OnMouseLeave(e);
    }

}

public class BoosterRadioButton : RadioButton
{


    private MouseState State;
    private Thread CheckThread;
    private Thread UncheckThread;

    private Rectangle EllipseRect = new Rectangle(5, 5, 6, 6);
    public BoosterRadioButton()
    {
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 9);
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
    }


    private void CheckAnimation()
    {
        int X = 1;
        int Y = 1;
        int EllipseW = 14;
        int EllipseH = 14;


        while (!(EllipseH == 8))
        {
            if (X < 4)
            {
                X += 1;
                Y += 1;
            }

            EllipseW -= 1;
            EllipseH -= 1;
            EllipseRect = new Rectangle(X, Y, EllipseW, EllipseH);
            Invalidate();
            Thread.Sleep(30);
        }

    }


    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.G = e.Graphics;
        Helpers.G.SmoothingMode = SmoothingMode.HighQuality;
        Helpers.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        base.OnPaint(e);

        Helpers.G.Clear(Parent.BackColor);


        if (Enabled)
        {
            using (SolidBrush Fill = new SolidBrush(ColorFromHex("#242424")))
            {
                using (Pen Border = new Pen(Helpers.ColorFromHex("#5C5C5C")))
                {
                    Helpers.G.FillEllipse(Fill, new Rectangle(0, 0, 16, 16));
                    Helpers.G.DrawEllipse(Border, new Rectangle(0, 0, 16, 16));
                }
            }

            switch (State)
            {

                case MouseState.None:

                    using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#B6B6B6")))
                    {
                        using (Font TextFont = new Font("Segoe UI", 9))
                        {
                            Helpers.G.DrawString(Text, TextFont, TextBrush, new Point(25, -1));
                        }
                    }


                    break;
                case MouseState.Over:

                    using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#F5F5F5")))
                    {
                        using (Font TextFont = new Font("Segoe UI", 9))
                        {
                            Helpers.G.DrawString(Text, TextFont, TextBrush, new Point(25, -1));
                        }
                    }


                    break;
            }


            if (Checked)
            {
                using (SolidBrush CheckBrush = new SolidBrush(Helpers.ColorFromHex("#909090")))
                {
                    Helpers.G.FillEllipse(CheckBrush, EllipseRect);
                }

            }


        }
        else
        {
            using (SolidBrush Fill = new SolidBrush(ColorFromHex("#282828")))
            {
                using (Pen Border = new Pen(Helpers.ColorFromHex("#484848")))
                {
                    Helpers.G.FillEllipse(Fill, new Rectangle(0, 0, 16, 16));
                    Helpers.G.DrawEllipse(Border, new Rectangle(0, 0, 16, 16));
                }
            }

            using (SolidBrush TextBrush = new SolidBrush(ColorFromHex("#818181")))
            {
                using (Font TextFont = new Font("Segoe UI", 9))
                {
                    Helpers.G.DrawString(Text, TextFont, TextBrush, new Point(25, -1));
                }
            }


            if (Checked)
            {
                using (SolidBrush CheckBrush = new SolidBrush(Helpers.ColorFromHex("#707070")))
                {
                    Helpers.G.FillEllipse(CheckBrush, EllipseRect);
                }

            }

        }

    }

    protected override void OnMouseEnter(EventArgs e)
    {
        State = MouseState.Over;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        State = MouseState.None;
        Invalidate();
        base.OnMouseLeave(e);
    }


    protected override void OnCheckedChanged(EventArgs e)
    {
        if (Checked)
        {
            CheckThread = new Thread(CheckAnimation) { IsBackground = true };
            CheckThread.Start();
        }

        base.OnCheckedChanged(e);
    }

}

public class BoosterNumericUpDown : NumericUpDown
{

    private MouseState State;
    public string AfterValue { get; set; }

    private Thread ValueChangedThread;
    private Point TextPoint = new Point(2, 2);

    private Font TextFont = new Font("Segoe UI", 10);
    public BoosterNumericUpDown()
    {
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 10);
        Controls[0].Hide();
        Controls[1].Hide();
        ForeColor = Helpers.ColorFromHex("#B6B6B6");
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
    }


    private void ValueChangedAnimation()
    {
        int TextSize = 5;

        while (!(TextSize == 10))
        {
            TextSize += 1;
            TextFont = new Font("Segoe UI", TextSize);
            Invalidate();
            Thread.Sleep(30);
        }

    }


    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.G = e.Graphics;
        Helpers.G.SmoothingMode = SmoothingMode.HighQuality;
        Helpers.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        Helpers.G.Clear(Parent.BackColor);

        base.OnPaint(e);


        if (Enabled)
        {
            using (SolidBrush Fill = new SolidBrush(Helpers.ColorFromHex("#242424")))
            {
                Helpers.G.FillRectangle(Fill, new Rectangle(0, 0, Width - 1, Height - 1));
            }

            switch (State)
            {

                case MouseState.None:

                    using (Pen Border = new Pen(Helpers.ColorFromHex("#5C5C5C")))
                    {
                        Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, Width - 1, Height - 1));
                    }


                    break;
                case MouseState.Over:

                    using (Pen Border = new Pen(Helpers.ColorFromHex("#C8C8C8")))
                    {
                        Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, Width - 1, Height - 1));
                    }


                    break;
            }

            using (SolidBrush TextBrush = new SolidBrush(Helpers.ColorFromHex("#B6B6B6")))
            {
                Helpers.G.DrawString(Value + AfterValue, TextFont, TextBrush, TextPoint);
            }

            using (Font ArrowFont = new Font("Marlett", 10))
            {
                using (SolidBrush ArrowBrush = new SolidBrush(Helpers.ColorFromHex("#909090")))
                {
                    Helpers.G.DrawString("5", ArrowFont, ArrowBrush, new Point(Width - 18, 2));
                    Helpers.G.DrawString("6", ArrowFont, ArrowBrush, new Point(Width - 18, 10));
                }
            }


        }
        else
        {
            using (SolidBrush Fill = new SolidBrush(ColorFromHex("#282828")))
            {
                using (Pen Border = new Pen(Helpers.ColorFromHex("#484848")))
                {
                    Helpers.G.FillRectangle(Fill, new Rectangle(0, 0, Width - 1, Height - 1));
                    Helpers.G.DrawRectangle(Border, new Rectangle(0, 0, Width - 1, Height - 1));
                }
            }

            using (SolidBrush TextBrush = new SolidBrush(Helpers.ColorFromHex("#818181")))
            {
                Helpers.G.DrawString(Value + AfterValue, TextFont, TextBrush, TextPoint);
            }

            using (Font ArrowFont = new Font("Marlett", 10))
            {
                using (SolidBrush ArrowBrush = new SolidBrush(Helpers.ColorFromHex("#707070")))
                {
                    Helpers.G.DrawString("5", ArrowFont, ArrowBrush, new Point(Width - 18, 2));
                    Helpers.G.DrawString("6", ArrowFont, ArrowBrush, new Point(Width - 18, 10));
                }
            }

        }

    }


    protected override void OnMouseUp(MouseEventArgs e)
    {

        if (e.X > Width - 16 && e.Y < 11)
        {
            if (!(Value + Increment > Maximum))
            {
                Value += Increment;
            }
            else
            {
                Value = Maximum;
            }

        }
        else if (e.X > Width - 16 && e.Y > 13)
        {
            if (!(Value - Increment < Minimum))
            {
                Value -= Increment;
            }
            else
            {
                Value = Minimum;
            }
        }

        Invalidate();

        base.OnMouseUp(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        State = MouseState.Over;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        State = MouseState.None;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnValueChanged(EventArgs e)
    {
        ValueChangedThread = new Thread(ValueChangedAnimation) { IsBackground = true };
        ValueChangedThread.Start();
        base.OnValueChanged(e);
    }

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
