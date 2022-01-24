using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleCard
{
    public class ResizeableControl
    {
        private enum EdgeType
        {
            None,
            Right,
            Left,
            Top,
            Bottom,
            TopLeft,
            RightBottom
        }

        private Control c;
        private EdgeType edgeType = EdgeType.None;
        private int thickness = 4;
        private bool isMouseDown = false;
        private bool isOutlineDraw = false;

        public ResizeableControl(Control control)
        {
            c = control;
        }

        public void Enable()
        {
            c.MouseDown += Control_MouseDown;
            c.MouseUp += Control_MouseUp;
            c.MouseMove += Control_MouseMove;
            c.MouseLeave += Control_MouseLeave;
        }

        public void Disable()
        {
            c.MouseDown -= Control_MouseDown;
            c.MouseUp -= Control_MouseUp;
            c.MouseMove -= Control_MouseMove;
            c.MouseLeave -= Control_MouseLeave;
        }

        private void Control_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
            }
        }

        private void Control_MouseUp(object? sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Control_MouseMove(object? sender, MouseEventArgs e)
        {
            #region 绘制边框
            using var g = c.CreateGraphics();
            switch (edgeType)
            {
                case EdgeType.None:
                    if (isOutlineDraw)
                    {
                        c.Refresh();
                        isOutlineDraw = false;
                    }
                    break;
                case EdgeType.Right:
                    c.Refresh();
                    g.FillRectangle(Brushes.LawnGreen, c.Width - thickness, 0, c.Width, c.Height);
                    isOutlineDraw = true;
                    break;
                case EdgeType.Left:
                    c.Refresh();
                    g.FillRectangle(Brushes.LawnGreen, 0, 0, thickness, c.Height);
                    isOutlineDraw = true;
                    break;
                case EdgeType.Top:
                    c.Refresh();
                    g.FillRectangle(Brushes.LawnGreen, 0, 0, c.Width, thickness);
                    isOutlineDraw = true;
                    break;
                case EdgeType.Bottom:
                    c.Refresh();
                    g.FillRectangle(Brushes.LawnGreen, 0, c.Height - thickness, c.Width, thickness);
                    isOutlineDraw = true;
                    break;
                case EdgeType.TopLeft:
                    c.Refresh();
                    g.FillRectangle(Brushes.LawnGreen, 0, 0, thickness * 4, thickness * 4);
                    isOutlineDraw = true;
                    break;
                case EdgeType.RightBottom:
                    c.Refresh();
                    g.FillRectangle(Brushes.LawnGreen, c.Width - thickness * 4, c.Height - thickness * 4, thickness * 4, thickness * 4);
                    isOutlineDraw = true;
                    break;
                default:
                    break;
            }
            #endregion

            if (isMouseDown && (edgeType != EdgeType.None))
            {
                c.SuspendLayout();
                switch (edgeType)
                {
                    case EdgeType.Right:
                        c.SetBounds(c.Left, c.Top, c.Width - (c.Width - e.X), c.Height);
                        break;
                    case EdgeType.Left:
                        c.SetBounds(c.Left + e.X, c.Top, c.Width - e.X, c.Height);
                        break;
                    case EdgeType.Top:
                        c.SetBounds(c.Left, c.Top + e.Y, c.Width, c.Height - e.Y);
                        break;
                    case EdgeType.Bottom:
                        c.SetBounds(c.Left, c.Top, c.Width, c.Height - (c.Height - e.Y));
                        break;
                    case EdgeType.TopLeft:
                        c.SetBounds(c.Left + e.X, c.Top + e.Y, c.Width, c.Height);
                        break;
                    case EdgeType.RightBottom:
                        c.SetBounds(c.Left, c.Top, c.Width - (c.Width - e.X), c.Height - (c.Height - e.Y));
                        break;
                    default:
                        break;
                }
                c.SuspendLayout();
            }
            else
            {
                // 左上选区
                if (e.X <= thickness * 4 && e.Y <= thickness * 4)
                {
                    c.Cursor = Cursors.SizeAll;
                    edgeType = EdgeType.TopLeft;
                }
                // 右下选区
                else if (e.X > c.Width - (thickness * 4) && e.Y > c.Height - (thickness * 4))
                {
                    c.Cursor = Cursors.SizeNWSE;
                    edgeType = EdgeType.RightBottom;
                }
                // 左选区
                else if (e.X <= thickness)
                {
                    c.Cursor = Cursors.SizeWE;
                    edgeType = EdgeType.Left;
                }
                // 右选区
                else if (e.X > c.Width - (thickness + 1))
                {
                    c.Cursor = Cursors.SizeWE;
                    edgeType = EdgeType.Right;
                }
                // 上选区
                else if (e.Y <= thickness)
                {
                    c.Cursor = Cursors.SizeNS;
                    edgeType = EdgeType.Top;
                }
                // 下选区
                else if (e.Y > c.Height - (thickness + 1))
                {
                    c.Cursor = Cursors.SizeNS;
                    edgeType = EdgeType.Bottom;
                }
                else
                {
                    c.Cursor = Cursors.Default;
                    edgeType = EdgeType.None;
                }
            }
        }

        private void Control_MouseLeave(object? sender, EventArgs e)
        {
            edgeType = EdgeType.None;
            c.Refresh();
        }
    }
}
