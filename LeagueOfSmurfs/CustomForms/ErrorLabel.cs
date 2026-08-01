using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class ErrorLabel : Label
    {
        public ErrorLabel()
        {
            InitializeComponent();

            // Font
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.ForeColor = Color.Red;
            this.Font = new Font("Segoe UI", 8);

            // Disabled at runtime
            this.Visible = false;
            this.Enabled = false;
        }
    }
}
