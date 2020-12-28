using CommonServiceLocator;
using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MTManga.Core.Entities;
using MTManga.Core.Enums;
using MTManga.Core.Services;
using MTManga.UWP.Views;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;

namespace MTManga.UWP.ViewModels {
    public class IndexVM : ViewModelBase {

        private ObservableCollection<MangaEntity> _Mangas;
        private readonly IMangaCollectionService mangaCollectionService;

        public ObservableCollection<MangaEntity> Mangas {
            get { return _Mangas; }
            set { SetValue(ref _Mangas, value); }
        }

        private MangaEntity _SelectedManga;
        public MangaEntity SelectedManga {
            get { return _SelectedManga; }
            set {
                SetValue(ref _SelectedManga, value);
                Navigate();
            }
        }

        private bool _Loading;
        public bool Loading {
            get { return _Loading; }
            set { SetValue(ref _Loading, value); }
        }

        public IndexVM(IMangaCollectionService mangaCollectionService) {
            this.mangaCollectionService = mangaCollectionService;
        }

        public async void Load() {
            Loading = true;
            //if (Mangas != null)
            //    foreach (var item in Mangas) {
            //        item.Cover = null;
            //    }
            Mangas = await mangaCollectionService.LoadMangasAsync();
            Loading = false;
        }

        public void Navigate() {
            if (SelectedManga == null)
                return;
            if (SelectedManga.Info.FileType == ItemType.List)
                ServiceLocator.Current.GetInstance<NavigationList>()[Nav.ShellFrame].NavigateTo(nameof(MangaChapters), SelectedManga);
            else
                ServiceLocator.Current.GetInstance<NavigationList>()[Nav.ShellFrame].NavigateTo(nameof(MangaRead), SelectedManga);
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

        public override void OnNavigateTo(NavigationEventArgs e) {
            Load();
        }

        public override void OnNavigateFrom(NavigatedArgs e) {
        }

    }
}
