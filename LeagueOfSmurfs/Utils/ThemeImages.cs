using LeagueOfSmurfs.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LeagueOfSmurfs.Utils
{
    /// <summary>
    /// Recolors UI bitmaps toward the active game accent (Valorant blue).
    /// </summary>
    public static class ThemeImages
    {
        private static readonly object sync = new object();
        private static readonly Dictionary<string, Image> originals = new Dictionary<string, Image>();
        private static readonly Dictionary<string, Image> tinted = new Dictionary<string, Image>();

        public static Image Resolve(string key, Image source)
        {
            if (source == null || string.IsNullOrEmpty(key))
                return source;

            lock (sync)
            {
                if (!originals.ContainsKey(key))
                    originals[key] = source;

                Image original = originals[key];
                if (!AppTheme.IsValorant)
                    return original;

                string tintKey = key + ":" + AppTheme.ValorantAccent.ToArgb().ToString("X");
                Image cached;
                if (tinted.TryGetValue(tintKey, out cached))
                    return cached;

                cached = Tint(original, AppTheme.ValorantAccent);
                tinted[tintKey] = cached;
                return cached;
            }
        }

        public static Bitmap Tint(Image source, Color tint)
        {
            int width = source.Width;
            int height = source.Height;
            Bitmap srcBmp = source as Bitmap;
            bool disposeSrc = false;
            if (srcBmp == null)
            {
                srcBmp = new Bitmap(source);
                disposeSrc = true;
            }

            Bitmap dst = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, width, height);

            BitmapData srcData = srcBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dst.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try
            {
                int bytes = Math.Abs(srcData.Stride) * height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

                float tr = tint.R / 255f;
                float tg = tint.G / 255f;
                float tb = tint.B / 255f;

                for (int i = 0; i < buffer.Length; i += 4)
                {
                    byte a = buffer[i + 3];
                    if (a == 0)
                        continue;

                    byte b = buffer[i];
                    byte g = buffer[i + 1];
                    byte r = buffer[i + 2];

                    // Luminance → multiply by accent (keeps shape, shifts purple UI to blue)
                    float luma = (r * 0.299f + g * 0.587f + b * 0.114f) / 255f;
                    // Slight boost so icons stay readable on dark panels
                    float strength = Math.Min(1f, luma * 1.25f);

                    buffer[i] = (byte)Clamp(tb * strength * 255f);
                    buffer[i + 1] = (byte)Clamp(tg * strength * 255f);
                    buffer[i + 2] = (byte)Clamp(tr * strength * 255f);
                    // keep alpha
                }

                Marshal.Copy(buffer, 0, dstData.Scan0, bytes);
            }
            finally
            {
                srcBmp.UnlockBits(srcData);
                dst.UnlockBits(dstData);
                if (disposeSrc)
                    srcBmp.Dispose();
            }

            return dst;
        }

        private static int Clamp(float value)
        {
            if (value < 0f) return 0;
            if (value > 255f) return 255;
            return (int)value;
        }
    }
}
