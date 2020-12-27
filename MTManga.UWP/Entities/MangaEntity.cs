using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.UWP.Entities {

    public class MangaEntity {
        public MangaInfo Info { get; }

        public MangaEntity(MangaInfo info) {
            Info = info;
        }
        public BitmapImage Cover { get; set; }
        public IStorageItem StorageItem { get; set; }

        public bool CanMove => Info.Current + Info.Offset >= 0 && Info.Current + Info.Offset < Info.Total;
    }
}
