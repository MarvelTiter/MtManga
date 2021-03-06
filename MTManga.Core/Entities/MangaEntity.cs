﻿using MT.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.Core.Entities {

    public class MangaEntity : ObservableObject {

        public MangaInfo Info { get; }

        public MangaEntity(MangaInfo info) {
            Info = info;
        }
        private BitmapImage _Cover;
        public BitmapImage Cover {
            get { return _Cover; }
            set { SetValue(ref _Cover, value); }
        }

        public IStorageItem StorageItem { get; set; }

        public bool CanMove(int pg, int fix) {
            var leftBound = Info.Current + Info.Offset + fix >= -1;
            var rightBound = Info.Current + pg + fix <= Info.Total;
            return leftBound && rightBound;
        }

        public string Status {
            get {
                if (Info.FileType == Enums.ItemType.List)
                    return $"合集({Info.Total})";
                if (Info.Current == 0)
                    return $"未看 / {Info.Total}";
                if (Info.Current + 1 == Info.Total)
                    return "已读完";
                return $"{Info.Current + 1} / {Info.Total}";
            }
        }
    }
}
