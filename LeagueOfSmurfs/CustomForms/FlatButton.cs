using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class FlatButton : Button
    {
        public override string Text { get => ""; set => base.Text = value; }

        public FlatButton() : base()
        {
            InitializeComponent();

            // Flat style
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatAppearance.BorderSize = 0;

            // Image
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Background
            this.BackColor = Color.FromArgb(30, 30, 30);
        }
    }
}
