using LeagueOfSmurfs.Configuration;
using LeagueOfSmurfs.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using DrawingBitmap = System.Drawing.Bitmap;

namespace LeagueOfSmurfs.Services
{
    public static class ImageService
    {
        public static BitmapSource FromDrawing(Image image)
        {
            if (image == null)
                return null;

            using (var bitmap = new DrawingBitmap(image))
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                var result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        public static BitmapSource FromThemed(string key, Image source)
        {
            if (source == null)
                return null;
            return FromDrawing(ThemeImages.Resolve(key, source));
        }

        /// <summary>
        /// Captures the current GIF frame and optionally tints it for Valorant (uncached — safe for animation).
        /// </summary>
        public static BitmapSource FromAnimatedFrame(Image frame, bool tintForValorant)
        {
            if (frame == null)
                return null;

            if (!tintForValorant)
                return FromDrawing(frame);

            using (DrawingBitmap tinted = ThemeImages.Tint(frame, AppTheme.ValorantAccent))
                return FromDrawing(tinted);
        }

        public static BitmapSource FromPack(string relativePath)
        {
            try
            {
                var uri = new Uri("pack://application:,,,/" + relativePath.Replace('\\', '/'), UriKind.Absolute);
                var image = new BitmapImage(uri);
                image.Freeze();
                return image;
            }
            catch
            {
                return null;
            }
        }
    }
}
