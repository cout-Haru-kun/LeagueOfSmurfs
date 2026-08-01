using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    public partial class LightComboBox : ComboBox
    {
        public LightComboBox() : base()
        {
            InitializeComponent();

            this.BackColor = Color.FromArgb(40, 40, 40);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
