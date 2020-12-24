using MT.MVVM.Core;
using MT.UWP.Common.Extension;
using MTManga.UWP.Entities;
using MTManga.UWP.Enums;
using MTManga.UWP.Extention;
using MTManga.UWP.Pages;
using MTManga.UWP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.UWP.ViewModels {
    public class IndexVM : ObservableObject {

        private ObservableCollection<MangaEntity> _Mangas;
        private readonly IMangaCollectionService mangaCollectionService;

        public ObservableCollection<MangaEntity> Mangas {
            get { return _Mangas; }
            set { SetValue(ref _Mangas, value); }
        }


        public IndexVM(IMangaCollectionService mangaCollectionService) {
            this.mangaCollectionService = mangaCollectionService;
            Load();
        }

        public async void Load() {
            Mangas = await mangaCollectionService.LoadMangasAsync();
        }

       

        public RelayCommand SelectCommand => new RelayCommand(OpenFiles);

        private async void OpenFiles() {
            var folder = await App.Helper.IO.GetUserFolderAsync();
            var token = App.Helper.IO.SaveFolder(folder);
            App.Helper.Setting.SaveLocalSetting(ConfigEnum.RootFolderToken, token);
            Load();
        }

        public RelayCommand NavigateCommand => new RelayCommand(() => {
            ViewModelLocator.Current.NavigationService.NavigateTo(nameof(MangaChapters));
        });



       

       

       
    }
}
