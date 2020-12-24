using MT.MVVM.Core;
using MTManga.UWP.Entities;
using MTManga.UWP.Enums;
using MTManga.UWP.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.UWP.ViewModels {
    public class IndexVM : ObservableObject {

        private ObservableCollection<MangaEntity> _Mangas;
        public ObservableCollection<MangaEntity> Mangas {
            get { return _Mangas; }
            set { SetValue(ref _Mangas, value); }
        }


        public IndexVM() {
            Init();
            Mangas = new ObservableCollection<MangaEntity>();
        }

        private async void Init() {
            if (App.Helper.Setting.GetLocalSetting(ConfigEnum.RootFolderToken, out string temp)) {
                var folder = await App.Helper.IO.GetUserFolderAsync(temp);
                await InitItems(folder);
            }
        }

        public RelayCommand SelectCommand => new RelayCommand(OpenFiles);

        private async void OpenFiles() {
            var folder = await App.Helper.IO.GetUserFolderAsync();
            var token = App.Helper.IO.SaveFolder(folder);
            App.Helper.Setting.SaveLocalSetting(ConfigEnum.RootFolderToken, token);
            await InitItems(folder);
        }


        public RelayCommand NavigateCommand => new RelayCommand(() => {
            //ViewModelLocator.Current.NavigationService.NavigateTo(nameof(BlankPage1));
        });



        private async Task InitItems(StorageFolder folder) {
            var items = await App.Helper.IO.GetLocalItemInFolderAsync(folder, ".zip");
            foreach (var item in items) {
                MangaEntity mangaEntity = new MangaEntity();
                mangaEntity.Title = item.Name;
                mangaEntity.Cover = await GetBitmap(item);
                Mangas.Add(mangaEntity);
            }
        }


        private async Task<BitmapImage> GetBitmap(IStorageItem item) {
            if (item.IsOfType(StorageItemTypes.File))
                return null;
            var nameFolder = item as StorageFolder;
            try {
                var cover = await nameFolder.TryGetItemAsync("cover.jpg");
                if (cover == null)
                    return null;
                using (var randomAccess = await (cover as StorageFile).OpenAsync(FileAccessMode.Read)) {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.DecodePixelWidth = 160;
                    bitmapImage.DecodePixelHeight = 200;
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
