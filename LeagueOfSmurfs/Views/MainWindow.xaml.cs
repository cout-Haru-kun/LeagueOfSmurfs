using LeagueOfSmurfs.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LeagueOfSmurfs.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;
        private int cardAnimIndex;
        private const double WindowCornerRadius = 24;
        private const double ModeThumbTravel = 32;
        private static readonly SolidColorBrush LeagueThumbBrush = CreateFrozenBrush(0xBB, 0x86, 0xFC);
        private static readonly SolidColorBrush ValorantThumbBrush = CreateFrozenBrush(0x5B, 0xA4, 0xFF);
        private double smoothScrollTarget;
        private double smoothScrollCurrent;
        private bool smoothScrollLoopRunning;
        // Lower = slower glide; ~0.08 feels like a continuous flow.
        private const double SmoothScrollLerp = 0.08;
        // Wheel delta is typically ±120; scale down so each notch travels less.
        private const double SmoothScrollWheelScale = 0.28;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
            SyncApiBoxes();
            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainViewModel.ApiKeyText) && !ApiPlainBox.IsFocused && !ApiPasswordBox.IsFocused)
                    SyncApiBoxes();
                if (e.PropertyName == nameof(MainViewModel.ApiKeyVisible))
                    SyncApiBoxes();
                if (e.PropertyName == nameof(MainViewModel.IsValorant)
                    || e.PropertyName == nameof(MainViewModel.IsLeague))
                    AnimateModeThumb(viewModel.IsValorant, animateColor: true);
            };
            Closed += (s, e) =>
            {
                StopSmoothScrollLoop();
                viewModel.StopBackgroundAnimation();
            };
        }

        private static SolidColorBrush CreateFrozenBrush(byte r, byte g, byte b)
        {
            var brush = new SolidColorBrush(Color.FromRgb(r, g, b));
            brush.Freeze();
            return brush;
        }

        private void ModeLeague_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimateModeThumb(toValorant: false, animateColor: true);
        }

        private void ModeValorant_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimateModeThumb(toValorant: true, animateColor: true);
        }

        private void ModeToggle_MouseLeave(object sender, MouseEventArgs e)
        {
            // Snap preview back to the active mode.
            AnimateModeThumb(viewModel.IsValorant, animateColor: true);
        }

        private void ModeLeague_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (viewModel.SwitchLeagueCommand.CanExecute(null))
                viewModel.SwitchLeagueCommand.Execute(null);
            AnimateModeThumb(false, animateColor: true);
        }

        private void ModeValorant_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (viewModel.SwitchValorantCommand.CanExecute(null))
                viewModel.SwitchValorantCommand.Execute(null);
            AnimateModeThumb(true, animateColor: true);
        }

        private void AnimateModeThumb(bool toValorant, bool animateColor)
        {
            double targetX = toValorant ? ModeThumbTravel : 0;
            var move = new DoubleAnimation
            {
                To = targetX,
                Duration = TimeSpan.FromMilliseconds(180),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            ModeThumbTranslate.BeginAnimation(TranslateTransform.XProperty, move);

            if (animateColor)
                ModeThumb.Background = toValorant ? ValorantThumbBrush : LeagueThumbBrush;
        }

        private void AccountsScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (!smoothScrollLoopRunning)
                smoothScrollCurrent = AccountsScrollViewer.VerticalOffset;

            double step = -e.Delta * SmoothScrollWheelScale;
            double from = smoothScrollLoopRunning ? smoothScrollTarget : smoothScrollCurrent;
            smoothScrollTarget = Math.Max(0, Math.Min(AccountsScrollViewer.ScrollableHeight, from + step));

            if (!smoothScrollLoopRunning)
                StartSmoothScrollLoop();
        }

        private void StartSmoothScrollLoop()
        {
            if (smoothScrollLoopRunning)
                return;
            smoothScrollLoopRunning = true;
            CompositionTarget.Rendering += SmoothScroll_OnRendering;
        }

        private void StopSmoothScrollLoop()
        {
            if (!smoothScrollLoopRunning)
                return;
            CompositionTarget.Rendering -= SmoothScroll_OnRendering;
            smoothScrollLoopRunning = false;
        }

        private void SmoothScroll_OnRendering(object sender, EventArgs e)
        {
            double max = AccountsScrollViewer.ScrollableHeight;
            smoothScrollTarget = Math.Max(0, Math.Min(max, smoothScrollTarget));

            double diff = smoothScrollTarget - smoothScrollCurrent;
            if (Math.Abs(diff) < 0.35)
            {
                smoothScrollCurrent = smoothScrollTarget;
                AccountsScrollViewer.ScrollToVerticalOffset(smoothScrollCurrent);
                StopSmoothScrollLoop();
                return;
            }

            // Exponential ease toward target — continuous, no per-notch jumps.
            smoothScrollCurrent += diff * SmoothScrollLerp;
            AccountsScrollViewer.ScrollToVerticalOffset(smoothScrollCurrent);
        }

        private void RootBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // ClipToBounds alone does not honor CornerRadius — force rounded clip
            if (RootBorder.ActualWidth <= 0 || RootBorder.ActualHeight <= 0)
                return;
            RootBorder.Clip = new RectangleGeometry(
                new Rect(0, 0, RootBorder.ActualWidth, RootBorder.ActualHeight),
                WindowCornerRadius,
                WindowCornerRadius);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Opacity = 0;
            var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(280))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            var slide = new DoubleAnimation(12, 0, TimeSpan.FromMilliseconds(280))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            BeginAnimation(OpacityProperty, fade);
            RootTranslate.BeginAnimation(TranslateTransform.YProperty, slide);
            RootBorder_SizeChanged(RootBorder, null);
            AnimateModeThumb(viewModel.IsValorant, animateColor: true);
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void ApiPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!viewModel.ApiKeyVisible)
                viewModel.ApiKeyText = ApiPasswordBox.Password;
        }

        private void SyncApiBoxes()
        {
            string text = viewModel.ApiKeyText ?? string.Empty;
            if (ApiPasswordBox.Password != text)
                ApiPasswordBox.Password = text;
            if (viewModel.ApiKeyVisible)
                ApiPlainBox.Visibility = Visibility.Visible;
            else
                ApiPlainBox.Visibility = Visibility.Collapsed;
            ApiPasswordBox.Visibility = viewModel.ApiKeyVisible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void AccountCard_Loaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null)
                return;

            int delay = Math.Min(cardAnimIndex * 35, 350);
            cardAnimIndex++;

            element.Opacity = 0;
            var transform = new TranslateTransform(0, 10);
            element.RenderTransform = transform;

            var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(220))
            {
                BeginTime = TimeSpan.FromMilliseconds(delay),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            var slide = new DoubleAnimation(10, 0, TimeSpan.FromMilliseconds(220))
            {
                BeginTime = TimeSpan.FromMilliseconds(delay),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            element.BeginAnimation(OpacityProperty, fade);
            transform.BeginAnimation(TranslateTransform.YProperty, slide);
        }
    }
}
