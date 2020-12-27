using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MT.UWP.Common.Extension;
using MTManga.UWP.Entities;
using MTManga.UWP.Enums;
using MTManga.UWP.Extention;
using MTManga.UWP.Models;
using MTManga.UWP.Services;
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
        private readonly IMangaReadingService mangaReadingService;

        public override void OnNavigateTo(NavigationEventArgs e) {
            _instance = e.Parameter as MangaEntity;
            Window.Current.Content.KeyDown += Content_KeyDown;
            mangaReadingService.DataCore = _instance;
            AppConfig.FixedChanged += AppConfig_FixedChanged;
            AppConfig.PageCountChanged += AppConfig_PageCountChange;
            AppConfig.PageModeChanged += AppConfig_PageModeChange;
            Read();
        }

        private void AppConfig_FixedChanged() {
            Read();
        }

        private void AppConfig_PageModeChange() {
            Read();
        }

        private void AppConfig_PageCountChange() {
            Read();
        }


        public override void OnNavigateFrom(NavigatedArgs e) {
            Window.Current.Content.KeyDown -= Content_KeyDown;
            AppConfig.FixedChanged -= AppConfig_FixedChanged;
            AppConfig.PageCountChanged -= AppConfig_PageCountChange;
            AppConfig.PageModeChanged -= AppConfig_PageModeChange;
        }

        public MangaReadVM(IMangaReadingService mangaReadingService) {
            this.mangaReadingService = mangaReadingService;
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

        private bool _ShowSetting;

        public bool ShowSetting {
            get { return _ShowSetting; }
            set { SetValue(ref _ShowSetting, value); }
        }


        public int PageCount => App.Helper.Setting.GetLocalSetting(ConfigEnum.PageCount, 1);
        public int PageMode => App.Helper.Setting.GetLocalSetting(ConfigEnum.PageMode, 1);
        public int Direction => App.Helper.Setting.GetLocalSetting(ConfigEnum.Direction, 1);
        public bool RepairedPageMode => App.Helper.Setting.GetLocalSetting(ConfigEnum.RepairedPageMode, false);

        public RelayCommand LeftCommand => new RelayCommand(() => {
            _instance.Info.Current += (PageCount + 1);
            Read();
        });
        public RelayCommand RightCommand => new RelayCommand(() => {
            _instance.Info.Current -= (PageCount + 1);
            Read();
        });

        public RelayCommand ShowCommand => new RelayCommand(() => {
            ShowSetting = true;
        });

        private void Content_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            if (e.Key == VirtualKey.A || e.Key == VirtualKey.Left) {
                LeftCommand.Execute(null);
            } else if (e.Key == VirtualKey.D || e.Key == VirtualKey.Right) {
                RightCommand.Execute(null);
            } else if (e.Key == VirtualKey.C) {
            } else if (e.Key == VirtualKey.Y || e.Key == VirtualKey.GamepadY) {
                ShowSetting = !ShowSetting;
            }
        }

        private async void Read() {
            var current = _instance.Info.Offset + (RepairedPageMode ? -1 : 0) + _instance.Info.Current;
            if (!_instance.CanMove)
                return;
            var first = await mangaReadingService.ReadAsync(current);
            if (PageCount == 0) {
                Left = first;
            } else if (PageCount == 1) {
                var second = await mangaReadingService.ReadAsync(current + 1);
                if (PageMode == 1) {
                    Right = first;
                    Left = second;
                } else {
                    Left = first;
                    Right = second;
                }
            }
            // save current
            await mangaReadingService.SaveReadingProgressAsync();
        }
    }
}
