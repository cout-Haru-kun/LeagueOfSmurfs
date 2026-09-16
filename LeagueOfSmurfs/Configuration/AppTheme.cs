using System.Drawing;

namespace LeagueOfSmurfs.Configuration
{
    public enum GameMode
    {
        League,
        Valorant
    }

    public static class AppTheme
    {
        public static GameMode Current { get; private set; } = GameMode.League;

        public static void SetMode(GameMode mode)
        {
            Current = mode;
        }

        public static bool IsValorant
        {
            get { return Current == GameMode.Valorant; }
        }

        // Accents
        public static readonly Color LeagueAccent = Color.FromArgb(187, 134, 252);
        public static readonly Color ValorantAccent = Color.FromArgb(91, 164, 255); // #5BA4FF

        public static Color Accent
        {
            get
            {
                return IsValorant ? ValorantAccent : LeagueAccent;
            }
        }

        public static Color AccentDark
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(59, 125, 216) // #3B7DD8
                    : Color.FromArgb(140, 90, 200);
            }
        }

        // Surfaces
        public static Color Window
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(10, 16, 28) // #0A101C
                    : Color.FromArgb(30, 30, 30);
            }
        }

        public static Color TitleBar
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(17, 24, 39) // #111827
                    : Color.FromArgb(40, 40, 40);
            }
        }

        public static Color Panel
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(19, 27, 44) // #131B2C
                    : Color.FromArgb(40, 40, 40);
            }
        }

        public static Color PanelAlt
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(16, 26, 44)
                    : Color.FromArgb(35, 35, 35);
            }
        }

        public static Color Input
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(27, 37, 56) // #1B2538
                    : Color.FromArgb(50, 50, 50);
            }
        }

        public static Color Card
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(22, 31, 51) // #161F33
                    : Color.FromArgb(40, 40, 40);
            }
        }

        public static Color Hover
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(36, 49, 72) // #243148
                    : Color.FromArgb(40, 40, 40);
            }
        }

        public static Color SecondaryText
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(155, 180, 212) // #9BB4D4
                    : Color.FromArgb(80, 80, 80);
            }
        }
    }
}
