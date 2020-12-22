using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.UWP.Entities {
    public class MangaEntity {
        public string Title { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }
        public BitmapImage Cover { get; set; }
        public List<MangaEntity> Chapters { get; set; }
        public FileAttributes FileType { get; set; }
    }
}
