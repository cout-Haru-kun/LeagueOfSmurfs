using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class LightListBox : ListBox
    {
        public LightListBox() : base()
        {
            InitializeComponent();

            this.BackColor = Color.FromArgb(40, 40, 40);
            this.BorderStyle = BorderStyle.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
