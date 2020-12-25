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
        public StorageFolder RootFolder { get; set; }

        public async Task<ObservableCollection<MangaEntity>> LoadMangasAsync() {
            InitFolder();
            var Mangas = new ObservableCollection<MangaEntity>();
            if (RootFolder == null)
                return Mangas;
            var items = await RootFolder.GetLocalItemInFolderAsync(".zip");
            foreach (var item in items) {
                MangaEntity mangaEntity = new MangaEntity();
                mangaEntity.Title = item.Name;
                mangaEntity.Cover = await GetBitmap(item);
                Mangas.Add(mangaEntity);
            }
            return Mangas;
        }

        private async void InitFolder() {
            if (App.Helper.Setting.GetLocalSetting(ConfigEnum.RootFolderToken, out string temp)) {
                var folder = await App.Helper.IO.GetUserFolderAsync(temp);
                RootFolder = folder;
            } else {
                //var folder = await App.Helper.IO.GetUserFolderAsync();
                //var token = App.Helper.IO.SaveFolder(folder);
                //App.Helper.Setting.SaveLocalSetting(ConfigEnum.RootFolderToken, token);
                //RootFolder = folder;
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
            var file = item as StorageFile;
            if (file.FileType == ".zip") {
                var fileStream = await file.OpenReadAsync();
                IRandomAccessStream randomAccess = null;
                var zipArchive = new ZipArchive(fileStream.AsStream());
                MemoryStream mem = null;
                try {
                    foreach (var entry in zipArchive.Entries) {
                        var s = entry.Open();
                        mem = s.ToMemoryStream();
                        if (mem.Length > 0) {
                            randomAccess = mem.AsRandomAccessStream();
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.DecodePixelWidth = 160;
                            bitmapImage.DecodePixelHeight = 200;
                            randomAccess.Seek(0);
                            if (randomAccess.Size > 0)
                                await bitmapImage.SetSourceAsync(randomAccess);
                            return bitmapImage;
                        } else {
                            s.Dispose();
                        }
                    }
                } finally {
                    zipArchive?.Dispose();
                    randomAccess?.Dispose();
                    fileStream?.Dispose();
                    mem?.Dispose();
                }
            }
            return null;
        }

        private async Task<BitmapImage> HandleFolder(IStorageItem item) {
            var nameFolder = item as StorageFolder;
            try {
                var cover = await nameFolder.TryGetItemAsync("cover.jpg");
                if (cover == null)
                    return null;
                using (var randomAccess = await (cover as StorageFile).OpenAsync(FileAccessMode.Read)) {
                    BitmapImage bitmapImage = new BitmapImage();
                    if (randomAccess.Size > 0)
                        await bitmapImage.SetSourceAsync(randomAccess);
                    return bitmapImage;
                }
            } catch (FileNotFoundException ex) {
                return null;
            }
        }
    }
}
