using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    internal class Utils
    {

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        public static void SetRoundedRegion(Form form, int widthEllipse, int heightEllipse)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, form.Width, form.Height, widthEllipse, heightEllipse));
        }
        public static void SetRoundedRegion(Control control, int widthEllipse, int heightEllipse)
        {
            control.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, control.Width, control.Height, widthEllipse, heightEllipse));
        }
    }
}
