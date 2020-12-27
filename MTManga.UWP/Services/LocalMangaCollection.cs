using MT.UWP.Common.Extension;
using MTManga.UWP.Entities;
using MTManga.UWP.Enums;
using MTManga.UWP.Extention;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.UWP.Services {
    public class LocalMangaCollection : IMangaCollectionService {
        public object DataCore { get; set; }
        private StorageFolder _folder;
        private string saveName;
        private MangaEntity _entity;
        private uint groupId {
            get {
                var i = _entity?.Info.Group;
                return i.HasValue ? (uint)i.Value : 0;
            }
        }
        private uint groupSize {
            get {
                var i = _entity?.Info.GroupSize;
                return i.HasValue ? (uint)i.Value : 0;
            }
        }
        public async Task<ObservableCollection<MangaEntity>> LoadMangasAsync() {
            InitFolder();
            var Mangas = new ObservableCollection<MangaEntity>();
            if (_folder == null)
                return Mangas;
            var infos = await App.Helper.IO.GetLocalDataAsync<List<MangaInfo>>(saveName);
            IReadOnlyList<IStorageItem> items = null;
            if (groupId >= 0 && groupSize > 0)
                items = await _folder.GetLocalItemInFolderAsync(groupId, groupSize, ".zip");
            else
                items = await _folder.GetLocalItemInFolderAsync(".zip");
            foreach (var item in items) {
                var info = infos.FirstOrDefault(i => i.Title == item.Name);
                if (info == null) {
                    info = new MangaInfo();
                    info.Title = item.Name;
                    info.Offset = 0;
                    infos.Add(info);
                }
                MangaEntity mangaEntity = new MangaEntity(info);
                mangaEntity.StorageItem = item;
                await InitManga(mangaEntity);
                Mangas.Add(mangaEntity);
            }
            await App.Helper.IO.SetLocalDataAsync(saveName, infos);
            return Mangas;
        }

        private async void InitFolder() {
            if (DataCore != null) {
                _entity = DataCore as MangaEntity;
                _folder = _entity.StorageItem.Folder();
                saveName = _entity.Info.SavedName + _entity.Info.Group;
            } else if (App.Helper.Setting.GetLocalSetting(ConfigEnum.RootFolderToken, out string temp)) {
                var folder = await App.Helper.IO.GetUserFolderAsync(temp);
                _folder = folder;
                saveName = _folder.Name;
            }
        }

        private async Task InitManga(MangaEntity mangaEntity) {
            // 获取封面
            mangaEntity.Cover = await GetBitmap(mangaEntity.StorageItem);
            // 检查目录类型
            if (mangaEntity.Info.FileType == ItemType.UnSet) {
                if (mangaEntity.StorageItem.IsOfType(StorageItemTypes.File)) { // zip 文件
                    mangaEntity.Info.FileType = ItemType.ZipManga;
                    mangaEntity.Info.SavedName = saveName;
                } else { // 文件夹
                    var folder = mangaEntity.StorageItem.Folder();
                    var first = await folder.GetFirstItem();
                    if (first.IsOfType(StorageItemTypes.Folder) || first.File()?.FileType == ".zip") {
                        mangaEntity.Info.FileType = ItemType.List;
                        mangaEntity.Info.SavedName = folder.Name;
                    } else {
                        mangaEntity.Info.FileType = ItemType.FolderManga;
                        mangaEntity.Info.SavedName = saveName;
                    }
                }
            }
            // 获取总数
            if (mangaEntity.Info.Total == 0) {
                var r = await GetItemCount(mangaEntity);
                mangaEntity.Info.Offset = r[0];
                mangaEntity.Info.Total = r[1];
            }
        }

        private async Task<int[]> GetItemCount(MangaEntity e) {
            var item = e.StorageItem;
            if (item.IsOfType(StorageItemTypes.Folder)) {
                var total = 0;
                if (e.Info.FileType == ItemType.FolderManga)
                    total = await item.Folder().GetFolderFileCount(".jpg", ".png");
                else
                    total = await item.Folder().GetFolderFileCount(".zip");
                return new int[] { 0, total };
            } else {
                var offset = 0;
                var total = 0;
                var file = item.File();
                if (file.FileType == ".zip") {
                    var fileStream = await file.OpenReadAsync();
                    IRandomAccessStream randomAccess = null;
                    var zipArchive = new ZipArchive(fileStream.AsStream());
                    MemoryStream mem = null;
                    total = zipArchive.Entries.Count;
                    try {
                        foreach (var entry in zipArchive.Entries) {
                            var s = entry.Open();
                            mem = s.ToMemoryStream();
                            if (mem.Length == 0) {
                                offset++;
                                total--;
                            } else {
                                break;
                            }
                            s?.Dispose();
                        }
                    } finally {
                        zipArchive?.Dispose();
                        zipArchive = null;
                        randomAccess?.Dispose();
                        randomAccess = null;
                        fileStream?.Dispose();
                        fileStream = null;
                        mem?.Dispose();
                        mem = null;
                    }
                }
                return new int[] { offset, total };
            }
        }

        private async Task<BitmapImage> GetBitmap(IStorageItem item) {
            if (item.IsOfType(StorageItemTypes.File)) {
                return await HandleFile(item);
            } else if (item.IsOfType(StorageItemTypes.Folder)) {
                return await HandleFolder(item);
            }
            return null;
        }

        private async Task<BitmapImage> HandleFile(IStorageItem item) {
            var file = item.File();
            if (file.FileType == ".zip") {
                var fileStream = await file.OpenStreamForReadAsync();
                IRandomAccessStream randomAccess = null;
                var zipArchive = new ZipArchive(fileStream);
                MemoryStream mem = null;
                try {
                    foreach (var entry in zipArchive.Entries) {
                        var s = entry.Open();
                        mem = s.ToMemoryStream();
                        if (mem.Length > 0) {
                            randomAccess = mem.AsRandomAccessStream();
                            return await WriteBitmap(randomAccess);
                        }
                        s?.Dispose();
                    }
                } finally {
                    zipArchive?.Dispose();
                    zipArchive = null;
                    randomAccess?.Dispose();
                    randomAccess = null;
                    fileStream?.Dispose();
                    fileStream = null;
                    mem?.Dispose();
                    mem = null;
                    GC.Collect();
                }
            } else {
                using (var randomAccess = await (item as StorageFile).OpenAsync(FileAccessMode.Read)) {
                    return await WriteBitmap(randomAccess);
                }
            }
            return null;
        }

        private async Task<BitmapImage> HandleFolder(IStorageItem item) {
            var nameFolder = item.Folder();
            try {
                IStorageItem cover = await nameFolder.TryGetItemAsync("cover.jpg");
                if (cover == null) {
                    cover = await nameFolder.GetFirstItem();
                }
                return await GetBitmap(cover);
            } catch (FileNotFoundException ex) {
                return null;
            }
        }

        private async Task<BitmapImage> WriteBitmap(IRandomAccessStream randomAccess) {
            return await randomAccess.WriteBitmap(200, 323);
        }
    }
}
