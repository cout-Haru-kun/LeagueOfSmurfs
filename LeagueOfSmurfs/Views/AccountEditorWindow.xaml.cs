using LeagueOfSmurfs.Configurations;
using LeagueOfSmurfs.ViewModels;
using RiotSharp;
using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LeagueOfSmurfs.Views
{
    public partial class AccountEditorWindow : Window
    {
        private readonly AccountEditorViewModel viewModel;
        private const double WindowCornerRadius = 20;

        public AccountEditorWindow(
            ConfigurationManager confManager,
            RiotApi api,
            SmurfsConfiguration existing,
            Action onSaved)
        {
            Resources.Add("BoolToVis", new BooleanToVisibilityConverter());
            InitializeComponent();
            viewModel = new AccountEditorViewModel(confManager, api, existing, () =>
            {
                onSaved?.Invoke();
                CloseWithAnimation();
            });
            DataContext = viewModel;
            Opacity = 0;
            SyncPasswordBoxes();
            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(AccountEditorViewModel.Password)
                    && !PasswordPlainBox.IsFocused && !PasswordBox.IsFocused)
                    SyncPasswordBoxes();
                if (e.PropertyName == nameof(AccountEditorViewModel.PasswordVisible))
                    SyncPasswordBoxes();
            };
        }

        private void RootBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (RootBorder.ActualWidth <= 0 || RootBorder.ActualHeight <= 0)
                return;
            RootBorder.Clip = new RectangleGeometry(
                new Rect(0, 0, RootBorder.ActualWidth, RootBorder.ActualHeight),
                WindowCornerRadius,
                WindowCornerRadius);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(220))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            var slide = new DoubleAnimation(10, 0, TimeSpan.FromMilliseconds(220))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            BeginAnimation(OpacityProperty, fade);
            RootTranslate.BeginAnimation(TranslateTransform.YProperty, slide);
            RootBorder_SizeChanged(RootBorder, null);
            SyncPasswordBoxes();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CloseWithAnimation();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Command handles save; Close happens via onSaved callback when successful
        }

        private void CloseWithAnimation()
        {
            var fade = new DoubleAnimation(Opacity, 0, TimeSpan.FromMilliseconds(160))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            fade.Completed += (s, e) => Close();
            BeginAnimation(OpacityProperty, fade);
        }

        private void GameNameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Text))
                return;

            if (e.Text.IndexOf('#') >= 0)
            {
                e.Handled = true;
                FocusTagBox();
                return;
            }

            e.Handled = !IsAllowedGameNameText(e.Text);
        }

        private void GameNameBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Some layouts send # via Oem3 / D3+Shift without PreviewTextInput reliability.
            if (e.Key == Key.Oem3 || (e.Key == Key.D3 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
            {
                e.Handled = true;
                FocusTagBox();
            }
        }

        private void GameNameBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.DataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }

            string paste = Convert.ToString(e.DataObject.GetData(DataFormats.Text), CultureInfo.InvariantCulture) ?? string.Empty;
            if (paste.IndexOf('#') >= 0)
            {
                e.CancelCommand();
                string beforeHash = paste.Substring(0, paste.IndexOf('#'));
                string afterHash = paste.Substring(paste.IndexOf('#') + 1);
                string cleanedName = SanitizeGameName(beforeHash);
                string cleanedTag = SanitizeTag(afterHash);

                var box = (TextBox)sender;
                int start = box.SelectionStart;
                string current = box.Text ?? string.Empty;
                string merged = current.Substring(0, start)
                    + cleanedName
                    + current.Substring(start + box.SelectionLength);
                if (merged.Length > box.MaxLength)
                    merged = merged.Substring(0, box.MaxLength);
                box.Text = merged;
                box.CaretIndex = Math.Min(start + cleanedName.Length, box.Text.Length);

                if (!string.IsNullOrEmpty(cleanedTag))
                {
                    TagBox.Text = cleanedTag.Length > TagBox.MaxLength
                        ? cleanedTag.Substring(0, TagBox.MaxLength)
                        : cleanedTag;
                    viewModel.Tag = TagBox.Text;
                }
                FocusTagBox();
                return;
            }

            string sanitized = SanitizeGameName(paste);
            if (sanitized != paste)
            {
                e.CancelCommand();
                var box = (TextBox)sender;
                int start = box.SelectionStart;
                string current = box.Text ?? string.Empty;
                string merged = current.Substring(0, start)
                    + sanitized
                    + current.Substring(start + box.SelectionLength);
                if (merged.Length > box.MaxLength)
                    merged = merged.Substring(0, box.MaxLength);
                box.Text = merged;
                box.CaretIndex = Math.Min(start + sanitized.Length, box.Text.Length);
            }
        }

        private void TagBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsAllowedTagText(e.Text);
        }

        private void TagBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.DataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }

            string paste = Convert.ToString(e.DataObject.GetData(DataFormats.Text), CultureInfo.InvariantCulture) ?? string.Empty;
            string sanitized = SanitizeTag(paste);
            if (sanitized == paste)
                return;

            e.CancelCommand();
            var box = (TextBox)sender;
            int start = box.SelectionStart;
            string current = box.Text ?? string.Empty;
            string merged = current.Substring(0, start)
                + sanitized
                + current.Substring(start + box.SelectionLength);
            if (merged.Length > box.MaxLength)
                merged = merged.Substring(0, box.MaxLength);
            box.Text = merged;
            box.CaretIndex = Math.Min(start + sanitized.Length, box.Text.Length);
        }

        private void FocusTagBox()
        {
            TagBox.Focus();
            TagBox.CaretIndex = TagBox.Text?.Length ?? 0;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!viewModel.PasswordVisible)
                viewModel.Password = PasswordBox.Password;
        }

        private void SyncPasswordBoxes()
        {
            string text = viewModel.Password ?? string.Empty;
            if (PasswordBox.Password != text)
                PasswordBox.Password = text;
            PasswordPlainBox.Visibility = viewModel.PasswordVisible ? Visibility.Visible : Visibility.Collapsed;
            PasswordBox.Visibility = viewModel.PasswordVisible ? Visibility.Collapsed : Visibility.Visible;
        }

        // Riot game name: letters (any Unicode), digits, spaces — no '#'.
        private static bool IsAllowedGameNameText(string text)
        {
            foreach (char c in text)
            {
                if (c == '#')
                    return false;
                if (!(char.IsLetter(c) || char.IsDigit(c) || c == ' '))
                    return false;
            }
            return true;
        }

        // Riot tagline: alphanumeric / Unicode letters only (3–5).
        private static bool IsAllowedTagText(string text)
        {
            foreach (char c in text)
            {
                if (!(char.IsLetter(c) || char.IsDigit(c)))
                    return false;
            }
            return true;
        }

        private static string SanitizeGameName(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            var sb = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                if (c == '#')
                    break;
                if (char.IsLetter(c) || char.IsDigit(c) || c == ' ')
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private static string SanitizeTag(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            var sb = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                if (char.IsLetter(c) || char.IsDigit(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
