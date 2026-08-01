using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class FlatProgressBar : ProgressBar
    {
        private SolidBrush brush = null;
        private int targetValue;

        public FlatProgressBar()
        {
            InitializeComponent();

            // Set style for color
            this.SetStyle(ControlStyles.UserPaint, true);

            // Color
            this.ForeColor = Color.FromArgb(187, 134, 252);
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            // Paint
            if (brush == null || brush.Color != this.ForeColor)
                brush = new SolidBrush(this.ForeColor);

            Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);
            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            rec.Height = rec.Height - 4;
            e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);
        }

        public void update()
        {
            // Update value
            if (this.targetValue != this.Value)
            {
                this.Value += (int)this.Maximum / 50;
                if (this.Value > this.targetValue)
                {
                    this.Value = this.targetValue;
                }
            }

            // Recall paint
            this.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
        }

         public void SetValue(int value)
         {
            this.targetValue = value;
            if (this.Value > this.targetValue)
            {
                this.Value = this.targetValue;
            }
        }
    }
}
