using MT.UWP.Common.Extension;
using MTManga.UWP.Entities;
using MTManga.UWP.Exceptions;
using MTManga.UWP.Extention;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.UWP.Services {
    public class LocalMangaReading : IMangaReadingService {
        public object DataCore { get; set; }
        private MangaEntity _entity;
        private StorageFolder _folder;
        private ZipArchive _zip;
        private bool flag;
        private List<MangaInfo> _infos;
        private string saveName;
        public async Task<BitmapImage> ReadAsync(int index) {
            await Init();
            if (index < 0)
                return null;
            if (_entity.Info.FileType == Enums.ItemType.ZipManga) {
                // StorageFile ZipArchive
                var entry = _zip.Entries[index];
                return await entry.Open().ToMemoryStream().AsRandomAccessStream().WriteBitmap();
            } else {
                // StorageFolder
                var file = await _folder.GetFilesAsync(CommonFileQuery.DefaultQuery, (uint)index, 1);
                var random = await file[0].OpenAsync(FileAccessMode.Read);
                return await random.WriteBitmap();
            }
        }

        public async Task SaveReadingProgressAsync() {
            _infos = await App.Helper.IO.GetLocalDataAsync<List<MangaInfo>>(saveName);
            _infos.ForEach(i => {
                if (i.Title == _entity.Info.Title) {
                    i.Current = _entity.Info.Current;
                }
            });
            await App.Helper.IO.SetLocalDataAsync(saveName, _infos);
        }

        public async Task Init() {
            if (!flag) {
                _entity = DataCore as MangaEntity;
                if (_entity.Info.FileType == Enums.ItemType.List)
                    throw new CustomException($"目录结构异常！{_entity.StorageItem.Name}");
                _folder = _entity.Info.FileType == Enums.ItemType.FolderManga ? _entity.StorageItem.Folder() : null;
                _zip = _entity.Info.FileType == Enums.ItemType.ZipManga ? await createZipZrchive() : null;
                saveName = _entity.Info.SavedName;
                flag = true;
            }
        }

        private async Task<ZipArchive> createZipZrchive() {
            var file = _entity.StorageItem.File();
            var fileStream = await file.OpenReadAsync();
            var zipArchive = new ZipArchive(fileStream.AsStream());
            return zipArchive;
        }
    }
}
