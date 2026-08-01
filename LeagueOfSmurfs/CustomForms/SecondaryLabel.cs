using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class SecondaryLabel : Label
    {
        public SecondaryLabel()
        {
            InitializeComponent();

            // Font
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.ForeColor = Color.FromArgb(80, 80, 80);
            this.Font = new Font("Segoe UI", 10);
            this.BackColor = Color.Transparent;
        }
    }
}
