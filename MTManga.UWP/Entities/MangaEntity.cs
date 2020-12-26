using MTManga.UWP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.UWP.Entities {
    public class MangaInfo {
        public string Title { get; set; }
        public int Total { get; set; }
        public float Current { get; set; }
        public ItemType FileType { get; set; } = ItemType.UnSet;
        public int Offset { get; set; }
    }

    public class MangaEntity {
        public MangaInfo Info { get; }

        public MangaEntity(MangaInfo info) {
            Info = info;
        }
        public BitmapImage Cover { get; set; }
        public IStorageItem StorageItem { get; set; }
    }
}
