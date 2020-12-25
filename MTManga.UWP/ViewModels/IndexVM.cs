using CommonServiceLocator;
using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MTManga.UWP.Entities;
using MTManga.UWP.Enums;
using MTManga.UWP.Services;
using MTManga.UWP.Views;
using System.Collections.ObjectModel;

namespace MTManga.UWP.ViewModels {
    public class IndexVM : ObservableObject {

        private ObservableCollection<MangaEntity> _Mangas;
        private readonly IMangaCollectionService mangaCollectionService;

        public ObservableCollection<MangaEntity> Mangas {
            get { return _Mangas; }
            set { SetValue(ref _Mangas, value); }
        }

        private object _SelectedManga;
        public object SelectedManga {
            get { return _SelectedManga; }
            set { SetValue<object>(ref _SelectedManga, value); }
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
            if (folder == null)
                return;
            var token = App.Helper.IO.SaveFolder(folder);
            App.Helper.Setting.SaveLocalSetting(ConfigEnum.RootFolderToken, token);
            Load();
        }

        public RelayCommand<object> NavigateCommand => new RelayCommand<object>(A);
        public void A(object e) {
            ServiceLocator.Current.GetInstance<NavigationServiceList>()["ShellFrame"].NavigateTo(nameof(MangaChapters), e);
        }
    }
}
