using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LeagueOfSmurfs.Utils
{
    public class WindowUtils
    {
        private const int SW_RESTORE = 9;
        private const int SW_SHOW = 5;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const byte VK_MENU = 0x12; // Alt

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        public static IEnumerable<IntPtr> EnumerateProcessWindowHandles(Process process)
        {
            var handles = new List<IntPtr>();
            if (process == null)
                return handles;

            try
            {
                foreach (ProcessThread thread in process.Threads)
                    EnumThreadWindows(thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);
            }
            catch
            {
                // process may have exited
            }

            return handles;
        }

        private static string GetWindowTitle(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
                return string.Empty;
            StringBuilder sb = new StringBuilder(256);
            GetWindowText(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }

        /// <summary>
        /// Best visible HWND for a process (MainWindow or first visible owned window).
        /// </summary>
        public static IntPtr GetMainVisibleWindow(Process process)
        {
            if (process == null)
                return IntPtr.Zero;

            try
            {
                process.Refresh();
                if (process.MainWindowHandle != IntPtr.Zero
                    && IsWindow(process.MainWindowHandle)
                    && IsWindowVisible(process.MainWindowHandle))
                    return process.MainWindowHandle;

                foreach (IntPtr handle in EnumerateProcessWindowHandles(process))
                {
                    if (handle != IntPtr.Zero && IsWindowVisible(handle) && !string.IsNullOrWhiteSpace(GetWindowTitle(handle)))
                        return handle;
                }
            }
            catch
            {
                // process may have exited
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Finds the Riot Client UX process that owns the real login window.
        /// Important: many "Riot Client" processes exist; only one has a visible window.
        /// </summary>
        public static Process FindRiotClientProcess()
        {
            // Prefer UX names first — RiotClientServices has no login UI
            string[] names = { "Riot Client", "RiotClientUx", "RiotClient" };

            Process best = null;
            foreach (string name in names)
            {
                Process[] found;
                try { found = Process.GetProcessesByName(name); }
                catch { continue; }

                foreach (Process p in found)
                {
                    try
                    {
                        p.Refresh();
                        IntPtr hwnd = p.MainWindowHandle;
                        if (hwnd == IntPtr.Zero || !IsWindowVisible(hwnd))
                            hwnd = GetMainVisibleWindow(p);
                        if (hwnd == IntPtr.Zero)
                            continue;

                        string title = GetWindowTitle(hwnd);
                        Debug.WriteLine($"Riot candidate: name={p.ProcessName} pid={p.Id} hwnd={hwnd} title='{title}'");

                        // Exact login window title is usually "Riot Client"
                        if (!string.IsNullOrWhiteSpace(title)
                            && title.IndexOf("Riot", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Debug.WriteLine($"Selected Riot window process pid={p.Id}");
                            return p;
                        }

                        if (best == null)
                            best = p;
                    }
                    catch
                    {
                        // ignore dead process
                    }
                }
            }

            // Fallback: scan all top-level windows for a Riot Client title
            if (best == null)
            {
                IntPtr foundHwnd = IntPtr.Zero;
                uint foundPid = 0;
                EnumWindows((hWnd, lParam) =>
                {
                    if (!IsWindowVisible(hWnd))
                        return true;
                    string title = GetWindowTitle(hWnd);
                    if (string.IsNullOrWhiteSpace(title))
                        return true;
                    if (title.IndexOf("Riot Client", StringComparison.OrdinalIgnoreCase) < 0
                        && !string.Equals(title, "Riot", StringComparison.OrdinalIgnoreCase))
                        return true;

                    GetWindowThreadProcessId(hWnd, out uint pid);
                    foundHwnd = hWnd;
                    foundPid = pid;
                    return false;
                }, IntPtr.Zero);

                if (foundPid != 0)
                {
                    try
                    {
                        best = Process.GetProcessById((int)foundPid);
                        Debug.WriteLine($"Selected Riot window via EnumWindows pid={foundPid} hwnd={foundHwnd} title='{GetWindowTitle(foundHwnd)}'");
                    }
                    catch
                    {
                        best = null;
                    }
                }
            }

            if (best != null)
                Debug.WriteLine($"Selected Riot process fallback pid={best.Id} name={best.ProcessName}");
            else
                Debug.WriteLine("No Riot Client UX process with a visible window found");

            return best;
        }

        /// <summary>
        /// True if the foreground window belongs to the given process (handles CEF child HWNDs).
        /// </summary>
        public static bool IsProcessForeground(Process process)
        {
            if (process == null)
                return false;

            IntPtr fg = GetForegroundWindow();
            if (fg == IntPtr.Zero)
                return false;

            GetWindowThreadProcessId(fg, out uint pid);
            try
            {
                return pid == (uint)process.Id;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries hard to bring the Riot client window to the foreground.
        /// </summary>
        public static bool EnsureForeground(Process process)
        {
            if (process == null)
                return false;

            IntPtr hwnd = GetMainVisibleWindow(process);
            if (hwnd == IntPtr.Zero)
                return false;

            if (IsProcessForeground(process))
                return true;

            try
            {
                if (IsIconic(hwnd))
                    ShowWindow(hwnd, SW_RESTORE);
                else
                    ShowWindow(hwnd, SW_SHOW);

                // Alt key trick: helps bypass Windows foreground lock
                keybd_event(VK_MENU, 0, 0, UIntPtr.Zero);

                IntPtr foreground = GetForegroundWindow();
                uint foreThread = GetWindowThreadProcessId(foreground, out _);
                uint appThread = GetCurrentThreadId();

                if (foreThread != appThread)
                {
                    AttachThreadInput(appThread, foreThread, true);
                    BringWindowToTop(hwnd);
                    SetForegroundWindow(hwnd);
                    AttachThreadInput(appThread, foreThread, false);
                }
                else
                {
                    BringWindowToTop(hwnd);
                    SetForegroundWindow(hwnd);
                }

                keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EnsureForeground failed: " + ex.Message);
                return false;
            }

            bool ok = IsProcessForeground(process);
            Debug.WriteLine($"EnsureForeground pid={process.Id} hwnd={hwnd} success={ok} fg={GetForegroundWindow()}");
            return ok;
        }

        /// <summary>
        /// Once per second until deadline: re-resolve Riot window, check focus, refocus if needed.
        /// </summary>
        public static async Task KeepFocusedLoopAsync(DateTime deadlineUtc, System.Threading.CancellationToken token)
        {
            while (!token.IsCancellationRequested && DateTime.UtcNow < deadlineUtc)
            {
                try
                {
                    Process process = FindRiotClientProcess();
                    if (process != null && !IsProcessForeground(process))
                    {
                        Debug.WriteLine("Riot client lost focus — refocusing");
                        EnsureForeground(process);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("KeepFocusedLoopAsync error: " + ex.Message);
                }

                try
                {
                    await Task.Delay(1000, token).ConfigureAwait(true);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Sends keys only when Riot UX is focused.
        /// Focus check/refocus and SendKeys happen back-to-back (no delay between them).
        /// The 1s wait only applies when focus could not be acquired at all.
        /// </summary>
        public static async Task<bool> SendKeysWhenFocusedAsync(string keys, DateTime deadlineUtc)
        {
            while (DateTime.UtcNow < deadlineUtc)
            {
                Process process = FindRiotClientProcess();
                if (process != null)
                {
                    // Focus then write immediately — no await between the two
                    if (!IsProcessForeground(process))
                        EnsureForeground(process);

                    if (IsProcessForeground(process))
                    {
                        // Last synchronous check right before sending
                        if (IsProcessForeground(process))
                        {
                            System.Windows.Forms.SendKeys.SendWait(keys);
                            return true;
                        }
                    }

                    // Focus still missing: tight retries (no 1s gap after a visible refocus)
                    for (int i = 0; i < 10 && DateTime.UtcNow < deadlineUtc; i++)
                    {
                        await Task.Delay(20).ConfigureAwait(true);
                        process = FindRiotClientProcess() ?? process;
                        if (process == null)
                            break;

                        if (!IsProcessForeground(process))
                            EnsureForeground(process);

                        if (IsProcessForeground(process))
                        {
                            System.Windows.Forms.SendKeys.SendWait(keys);
                            return true;
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("SendKeysWhenFocusedAsync: Riot UX window not found yet");
                }

                // Only when we still don't have focus after tight retries
                int remainingMs = (int)(deadlineUtc - DateTime.UtcNow).TotalMilliseconds;
                if (remainingMs <= 0)
                    break;
                await Task.Delay(Math.Min(1000, remainingMs)).ConfigureAwait(true);
            }

            Debug.WriteLine("SendKeysWhenFocusedAsync timed out for: " + keys);
            return false;
        }
    }
}
