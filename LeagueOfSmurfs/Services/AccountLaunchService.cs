using LeagueOfSmurfs.Configurations;
using LeagueOfSmurfs.Utils;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LeagueOfSmurfs.Services
{
    /// <summary>
    /// Riot login automation (keyboard / clipboard). Keeps WinForms SendKeys for compatibility.
    /// </summary>
    public static class AccountLaunchService
    {
        public static async Task LaunchAsync(
            SmurfsConfiguration conf,
            IProgress<int> progress,
            Action minimizeHost,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (conf == null)
                throw new ArgumentNullException(nameof(conf));

            DateTime deadline = DateTime.UtcNow.AddSeconds(30);
            CancellationTokenSource focusCts = null;
            Task focusLoop = null;

            try
            {
                Report(progress, 0);
                RiotUtils.closeRiot();
                RiotUtils.launchClient();
                Report(progress, 20);

                Process riotProcess = null;
                while (riotProcess == null)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    ThrowIfTimedOut(deadline);
                    riotProcess = WindowUtils.FindRiotClientProcess();
                    if (riotProcess == null)
                        await Task.Delay(100, cancellationToken).ConfigureAwait(true);
                }

                try { riotProcess.WaitForInputIdle(5000); } catch { /* ignore */ }
                Report(progress, 40);

                focusCts = new CancellationTokenSource();
                focusLoop = WindowUtils.KeepFocusedLoopAsync(deadline, focusCts.Token);

                await Task.Delay(1500, cancellationToken).ConfigureAwait(true);
                ClearClipboard();
                while (!string.Equals(GetClipboardText(), "longtestforinput", StringComparison.Ordinal))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    ThrowIfTimedOut(deadline);
                    if (!await WindowUtils.SendKeysWhenFocusedAsync("longtestforinput", deadline).ConfigureAwait(true))
                        throw new TimeoutException("Could not focus Riot client for input probe");
                    await Task.Delay(100, cancellationToken).ConfigureAwait(true);
                    if (!await WindowUtils.SendKeysWhenFocusedAsync("^{a}^{x}", deadline).ConfigureAwait(true))
                        throw new TimeoutException("Could not focus Riot client for input probe clear");
                }

                Report(progress, 50);

                SetClipboardText(conf.username ?? string.Empty);
                if (!await WindowUtils.SendKeysWhenFocusedAsync("^{v}", deadline).ConfigureAwait(true))
                    throw new TimeoutException("Could not paste username");
                Report(progress, 65);

                if (!await WindowUtils.SendKeysWhenFocusedAsync("{TAB}", deadline).ConfigureAwait(true))
                    throw new TimeoutException("Could not tab to password");
                await Task.Delay(200, cancellationToken).ConfigureAwait(true);

                SetClipboardText(conf.password ?? string.Empty);
                if (!await WindowUtils.SendKeysWhenFocusedAsync("^{v}", deadline).ConfigureAwait(true))
                    throw new TimeoutException("Could not paste password");
                await Task.Delay(50, cancellationToken).ConfigureAwait(true);
                ClearClipboard();
                Report(progress, 80);

                for (int i = 0; i < 7; i++)
                {
                    ThrowIfTimedOut(deadline);
                    if (!await WindowUtils.SendKeysWhenFocusedAsync("{TAB}", deadline).ConfigureAwait(true))
                        throw new TimeoutException("Could not tab through login form");
                    await Task.Delay(50, cancellationToken).ConfigureAwait(true);
                }
                if (!await WindowUtils.SendKeysWhenFocusedAsync("{ENTER}", deadline).ConfigureAwait(true))
                    throw new TimeoutException("Could not confirm login");

                int waitMs = Math.Min(3000, Math.Max(0, (int)(deadline - DateTime.UtcNow).TotalMilliseconds));
                if (waitMs > 0)
                    await Task.Delay(waitMs, cancellationToken).ConfigureAwait(true);

                RiotUtils.launchCurrentGame();
                Report(progress, 100);

                focusCts.Cancel();

                string gameProcess = RiotUtils.getGameProcessName();
                DateTime gameDeadline = DateTime.UtcNow.AddSeconds(60);
                Process[] game = null;
                while (game == null || game.Length == 0)
                {
                    if (DateTime.UtcNow >= gameDeadline)
                        break;
                    game = Process.GetProcessesByName(gameProcess);
                    await Task.Delay(100, cancellationToken).ConfigureAwait(true);
                }

                if (game != null && game.Length > 0 && minimizeHost != null)
                    minimizeHost();

                await Task.Delay(1000, cancellationToken).ConfigureAwait(true);
                Report(progress, 0);
            }
            finally
            {
                if (focusCts != null)
                {
                    focusCts.Cancel();
                    if (focusLoop != null)
                    {
                        try { await focusLoop.ConfigureAwait(true); } catch { /* ignore */ }
                    }
                    focusCts.Dispose();
                }
                try { ClearClipboard(); } catch { /* ignore */ }
            }
        }

        private static void Report(IProgress<int> progress, int value)
        {
            progress?.Report(value);
        }

        private static void ThrowIfTimedOut(DateTime deadlineUtc)
        {
            if (DateTime.UtcNow >= deadlineUtc)
                throw new TimeoutException("Login sequence exceeded 30 seconds");
        }

        private static void ClearClipboard()
        {
            try
            {
                Clipboard.Clear();
            }
            catch
            {
                try { System.Windows.Forms.Clipboard.Clear(); } catch { /* ignore */ }
            }
        }

        private static string GetClipboardText()
        {
            try
            {
                return Clipboard.GetText() ?? string.Empty;
            }
            catch
            {
                try { return System.Windows.Forms.Clipboard.GetText() ?? string.Empty; }
                catch { return string.Empty; }
            }
        }

        private static void SetClipboardText(string text)
        {
            try
            {
                Clipboard.SetText(text ?? string.Empty);
            }
            catch
            {
                System.Windows.Forms.Clipboard.SetText(text ?? string.Empty);
            }
        }
    }
}
