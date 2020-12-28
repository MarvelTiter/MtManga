using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.Core.Services {
    public interface IMangaReadingService : IDisposable {
        object DataCore { get; set; }
        Task<BitmapImage> ReadAsync(int index);

        Task SaveReadingProgressAsync();
    }
}
