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
        public static readonly Color ValorantAccent = Color.FromArgb(0, 148, 255);

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
                    ? Color.FromArgb(0, 100, 180)
                    : Color.FromArgb(140, 90, 200);
            }
        }

        // Surfaces
        public static Color Window
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(18, 28, 48)
                    : Color.FromArgb(30, 30, 30);
            }
        }

        public static Color TitleBar
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(24, 40, 68)
                    : Color.FromArgb(40, 40, 40);
            }
        }

        public static Color Panel
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(24, 40, 68)
                    : Color.FromArgb(40, 40, 40);
            }
        }

        public static Color PanelAlt
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(20, 34, 58)
                    : Color.FromArgb(35, 35, 35);
            }
        }

        public static Color Input
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(32, 52, 86)
                    : Color.FromArgb(50, 50, 50);
            }
        }

        public static Color Card
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(22, 36, 62)
                    : Color.FromArgb(40, 40, 40);
            }
        }

        public static Color Hover
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(28, 48, 80)
                    : Color.FromArgb(40, 40, 40);
            }
        }

        public static Color SecondaryText
        {
            get
            {
                return IsValorant
                    ? Color.FromArgb(140, 170, 210)
                    : Color.FromArgb(80, 80, 80);
            }
        }
    }
}
