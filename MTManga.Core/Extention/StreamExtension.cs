using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.Core.Extention {
    public static class StreamExtension {
        public static MemoryStream ToMemoryStream(this Stream self) {
            MemoryStream mem = new MemoryStream();
            try {
                byte[] buffer = new byte[1024 * 4];
                while (self.Read(buffer, 0, buffer.Length) > 0) {
                    mem.Write(buffer, 0, buffer.Length);
                }
                mem.Seek(0, SeekOrigin.Begin);
                return mem;
            } catch (Exception) {
                return mem;
            } finally {
                self.Dispose();
            }

        }

        public static async Task<BitmapImage> WriteBitmap(this IRandomAccessStream randomAccess, int width = 0, int height = 0) {
            try {
                BitmapImage bitmapImage = new BitmapImage();
                if (width > 0)
                    bitmapImage.DecodePixelWidth = width;
                if (height > 0)
                    bitmapImage.DecodePixelHeight = height;
                randomAccess.Seek(0);
                if (randomAccess.Size > 0)
                    await bitmapImage.SetSourceAsync(randomAccess);
                return bitmapImage;
            } catch (Exception) {
                return null;
            } finally {
                randomAccess?.Dispose();
            }
        }
    }
}
