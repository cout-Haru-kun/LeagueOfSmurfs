using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class FlatButtonRaw : Button
    {
        public FlatButtonRaw()
        {
            InitializeComponent();

            // Flat style
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.BorderColor = Color.FromArgb(40, 40, 40);

            // Background
            this.BackColor = Color.FromArgb(187, 134, 252);

            // Font
            this.ForeColor = Color.Black;
            this.Font = new Font("Segoe UI", 10);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            CustomForms.Utils.SetRoundedRegion(this, 10, 10);
        }
    }
}
