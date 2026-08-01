using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class LightTextBox : TextBox
    {
        public LightTextBox()
        {
            InitializeComponent();

            // Background
            this.ForeColor = Color.White;
            this.BackColor = Color.FromArgb(50, 50, 50);

            // Border
            this.BorderStyle = BorderStyle.None;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            CustomForms.Utils.SetRoundedRegion(this, 10, 10);
        }
    }
}
