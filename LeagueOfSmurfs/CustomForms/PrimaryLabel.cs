using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class PrimaryLabel : Label
    {
        public PrimaryLabel()
        {
            InitializeComponent();

            // Font
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 15, FontStyle.Bold);
        }

    }
}
