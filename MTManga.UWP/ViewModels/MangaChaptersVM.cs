using CommonServiceLocator;
using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MTManga.UWP.Entities;
using MTManga.UWP.Enums;
using MTManga.UWP.Models;
using MTManga.UWP.Services;
using MTManga.UWP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MTManga.UWP.ViewModels {
    public class MangaChaptersVM : ViewModelBase {
        private readonly IMangaCollectionService mangaCollectionService;
        public MangaChaptersVM(IMangaCollectionService mangaCollectionService) {
            this.mangaCollectionService = mangaCollectionService;
        }

        public int GroupSize => App.Helper.Setting.GetLocalSetting(ConfigEnum.GroupSize, 20);

        private MangaEntity _Model;
        public override void OnNavigateTo(NavigationEventArgs e) {
            if (e.NavigationMode != NavigationMode.New) {
                SelectedManga = null;
                Load();
                return;
            }
            Model = e.Parameter as MangaEntity;
            mangaCollectionService.DataCore = Model;
            Groups = new ObservableCollection<MenuModel>();
            var hc = Model.Info.Total / GroupSize + 1;
            for (int i = 0; i < hc; i++) {
                Groups.Add(new MenuModel {
                    Header = $"{i * GroupSize + 1}-{GroupSize * (i + 1)}"
                });
            }
            GroupIndex = 0;
        }


        public MangaEntity Model {
            get { return _Model; }
            set { SetValue(ref _Model, value); }
        }

        private ObservableCollection<MenuModel> _Groups;
        public ObservableCollection<MenuModel> Groups {
            get { return _Groups; }
            set { SetValue(ref _Groups, value); }
        }

        private ObservableCollection<MangaEntity> _Mangas;
        public ObservableCollection<MangaEntity> Mangas {
            get { return _Mangas; }
            set { SetValue(ref _Mangas, value); }
        }

        private MangaEntity _SelectedManga;
        public MangaEntity SelectedManga {
            get { return _SelectedManga; }
            set {
                SetValue(ref _SelectedManga, value);
                if (value == null)
                    return;
                ServiceLocator.Current.GetInstance<NavigationList>()[Nav.ShellFrame].NavigateTo(nameof(MangaRead), value);
            }
        }

        private int _GroupIndex;

        public int GroupIndex {
            get { return _GroupIndex; }
            set {
                SetValue(ref _GroupIndex, value);
                Load();
            }
        }

        private bool _Loading;
        public bool Loading {
            get { return _Loading; }
            set { SetValue(ref _Loading, value); }
        }


        private async void Load() {
            Loading = true;
            Model.Info.Group = GroupIndex;
            Model.Info.GroupSize = GroupSize;
            Mangas?.Clear();
            Mangas = await mangaCollectionService.LoadMangasAsync();
            Loading = false;
        }
    }
}
