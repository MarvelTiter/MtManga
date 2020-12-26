using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MT.UWP.Common.Extension;
using MTManga.UWP.Entities;
using MTManga.UWP.Enums;
using MTManga.UWP.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MTManga.UWP.ViewModels {
    public class MangaReadVM : ViewModelBase {
        public MangaEntity _instance;
        public override void OnNavigateTo(NavigationEventArgs e) {
            _instance = e.Parameter as MangaEntity;
            Window.Current.Content.KeyDown += Content_KeyDown;
            Read();
        }

        public override void OnNavigateFrom(NavigatedArgs e) {
            Window.Current.Content.KeyDown -= Content_KeyDown;
        }

        private BitmapImage _Left;
        public BitmapImage Left {
            get { return _Left; }
            set { SetValue(ref _Left, value); }
        }
        private BitmapImage _Right;
        public BitmapImage Right {
            get { return _Right; }
            set { SetValue(ref _Right, value); }
        }

        public int PageCount => App.Helper.Setting.GetLocalSetting(ConfigEnum.PageCount, 1);
        public int PageMode => App.Helper.Setting.GetLocalSetting(ConfigEnum.PageMode, 1);
        public int Direction => App.Helper.Setting.GetLocalSetting(ConfigEnum.Direction, 1);

        public RelayCommand LeftCommand => new RelayCommand(() => {

        });
        public RelayCommand RightCommand => new RelayCommand(() => {

        });

        private void Content_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            e.Handled = true;
            if (e.Key == VirtualKey.A || e.Key == VirtualKey.Left) {

            } else if (e.Key == VirtualKey.D || e.Key == VirtualKey.Right) {

            } else if (e.Key == VirtualKey.C) {

            }
        }

        private async void Read() {
            if (_instance.Info.FileType == ItemType.ZipManga) {
                // StorageFile ZipArchive
            } else {
                // StorageFolder
                var folder = _instance.StorageItem.Folder();
                var pics = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery, 0, 2);
                Right = await (await pics[0].OpenAsync(Windows.Storage.FileAccessMode.Read)).WriteBitmap();
                Left = await (await pics[1].OpenAsync(Windows.Storage.FileAccessMode.Read)).WriteBitmap();
            }
        }
    }
}
