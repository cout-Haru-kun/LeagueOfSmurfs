using LeagueOfSmurfs.Configuration;
using System;
using System.Linq;
using System.Windows;

namespace LeagueOfSmurfs.Services
{
    public static class ThemeService
    {
        private const string LeagueTheme = "Themes/LeagueTheme.xaml";
        private const string ValorantTheme = "Themes/ValorantTheme.xaml";

        public static void Apply(GameMode mode)
        {
            AppTheme.SetMode(mode);
            var app = Application.Current;
            if (app == null)
                return;

            var dictionaries = app.Resources.MergedDictionaries;
            var theme = dictionaries.FirstOrDefault(d =>
                d.Source != null &&
                (d.Source.OriginalString.IndexOf("LeagueTheme", StringComparison.OrdinalIgnoreCase) >= 0
                 || d.Source.OriginalString.IndexOf("ValorantTheme", StringComparison.OrdinalIgnoreCase) >= 0));

            if (theme != null)
                dictionaries.Remove(theme);

            string path = mode == GameMode.Valorant ? ValorantTheme : LeagueTheme;
            dictionaries.Add(new ResourceDictionary
            {
                Source = new Uri(path, UriKind.Relative)
            });
        }
    }
}
