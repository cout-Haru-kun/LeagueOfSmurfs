using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class LightVScrollBar : VScrollBar
    {
        public LightVScrollBar()
        {
            InitializeComponent();

            // Background
            this.ForeColor = Color.White;
            this.BackColor = Color.FromArgb(40, 40, 40);

            // Set style
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw
              | ControlStyles.Selectable | ControlStyles.AllPaintingInWmPaint
              | ControlStyles.UserPaint, true);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            CustomForms.Utils.SetRoundedRegion(this, 10, 10);
        }
    }
}
