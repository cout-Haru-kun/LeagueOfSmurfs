using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LeagueOfSmurfs.Views
{
    public partial class AccountCard : UserControl
    {
        private const double CardCornerRadius = 12;

        public AccountCard()
        {
            InitializeComponent();
        }

        private void AccountCard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Clip children (including rank bar) to rounded card shape
            CardBorder.Clip = new RectangleGeometry(
                new Rect(0, 0, CardBorder.ActualWidth, CardBorder.ActualHeight),
                CardCornerRadius,
                CardCornerRadius);
        }
    }
}
