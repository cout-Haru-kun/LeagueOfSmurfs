using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class LightPanel : Panel
    {
        public LightPanel() : base()
        {
            InitializeComponent();

            // Backcolor
            this.BackColor = Color.FromArgb(40, 40, 40);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            CustomForms.Utils.SetRoundedRegion(this, 20, 20);
        }
    }
}
