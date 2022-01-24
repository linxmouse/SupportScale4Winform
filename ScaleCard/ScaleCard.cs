namespace ScaleCard
{
    public partial class ScaleCard : UserControl
    {
        // 当前窗口的宽度和高度
        private float w, h;

        public ScaleCard()
        {
            InitializeComponent();

            w = this.Width;
            h = this.Height;
            SetTag(this);
            new ResizeableControl(this).Enable();
        }

        private void SetTag(Control control)
        {
            foreach (Control item in control.Controls)
            {
                item.Tag = item.Width + ";" + item.Height + ";" + item.Left + ";" + item.Top + ";" + item.Font.Size;
                if (item.Controls.Count > 0)
                {
                    SetTag(item);
                }
            }
        }

        private void ScaleCard_Resize(object sender, EventArgs e)
        {
            float scalew = this.Width / w;
            float scaleh = this.Height / h;
            ScaleControls(scalew, scaleh, this);
        }

        private void ScaleControls(float scalew, float scaleh, Control control)
        {
            foreach (Control item in control.Controls)
            {
                if (item.Tag != null)
                {
                    string[] tag = item.Tag.ToString().Split(new char[] { ';' });
                    // 根据窗体缩放的比例确定控件的值
                    item.Width = Convert.ToInt32(Convert.ToSingle(tag[0]) * scalew);
                    item.Height = Convert.ToInt32(Convert.ToSingle(tag[1]) * scaleh);
                    item.Left = Convert.ToInt32(Convert.ToSingle(tag[2]) * scalew);
                    item.Top = Convert.ToInt32(Convert.ToSingle(tag[3]) * scaleh);
                    Single size = Convert.ToSingle(tag[4]) * scaleh;
                    item.Font = new Font(item.Font.Name, size, item.Font.Style, item.Font.Unit);

                    if (item.Controls.Count > 0)
                    {
                        ScaleControls(scalew, scaleh, item);
                    }
                }
            }
        }
    }
}