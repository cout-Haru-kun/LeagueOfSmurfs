using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs.CustomForms
{
    internal class TitleBar
    {
        // Settings
        private const int height = 30;

        // Runtime variable
        private Form parent;
        private bool drag = false;
        private Point baseLoc = new Point(0, 0);

        public TitleBar(Form parentForm)
        {
            this.parent = parentForm;
        }




        public void OnPaint(PaintEventArgs e)
        {
            Brush brush = new SolidBrush(Color.FromArgb(40, 40, 40));
            Rectangle rect = new Rectangle(0, 0, parent.Width, height);

            // Draw bar
            e.Graphics.FillRectangle(brush, rect);
        }



        public void MouseDown(object sender, MouseEventArgs e)
        {
            // Check if Y is on top of window then moove
            if (e.Location.Y < height)
            {
                this.baseLoc = e.Location;
                drag = true;
            }
        }
        public void MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (this.drag)
            {
                Point p1 = new Point(e.X, e.Y);
                Point p2 = this.parent.PointToScreen(p1);
                Point p3 = new Point(p2.X - this.baseLoc.X,
                                     p2.Y - this.baseLoc.Y);
                this.parent.Location = p3;
            }
        }
    }
}
