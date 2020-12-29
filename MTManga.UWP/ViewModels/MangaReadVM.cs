using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MT.UWP.Common;
using MTManga.Core.Entities;
using MTManga.Core.Enums;
using MTManga.Core.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MTManga.UWP.ViewModels {
    public class MangaReadVM : ViewModelBase {
        public MangaEntity _instance;
        private readonly IMangaReadingService mangaReadingService;
        private readonly KeyEventRunner keyEventRunner;

        public override void OnNavigateTo(NavigationEventArgs e) {
            _instance = e.Parameter as MangaEntity;
            Window.Current.Content.KeyDown += Content_KeyDown;
            mangaReadingService.DataCore = _instance;
            AppConfig.FixedChanged += AppConfig_FixedChanged;
            AppConfig.PageCountChanged += AppConfig_PageCountChange;
            AppConfig.PageModeChanged += AppConfig_PageModeChange;
            Read();
        }


        public async override void OnNavigateFrom(NavigatedArgs e) {
            Window.Current.Content.KeyDown -= Content_KeyDown;
            AppConfig.FixedChanged -= AppConfig_FixedChanged;
            AppConfig.PageCountChanged -= AppConfig_PageCountChange;
            AppConfig.PageModeChanged -= AppConfig_PageModeChange; 
            mangaReadingService.Dispose();
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

        public MangaReadVM(IMangaReadingService mangaReadingService, KeyEventRunner keyEventRunner) {
            this.mangaReadingService = mangaReadingService;
            this.keyEventRunner = keyEventRunner;
            InitEvent();
        }

        private void InitEvent() {
            void left() => LeftCommand.Execute(null);
            void right() => RightCommand.Execute(null);
            void toggle() => ShowSetting = !ShowSetting;
            var cfg = App.Current.Resources["Config"] as AppConfig;
            void pageCount() => cfg.UpdatePageCountCommand.Execute(null);
            void repaired() => cfg.UpdateRepairedPageModeCommand.Execute(null);
            void pageMode() => cfg.UpdatePageModeCommand.Execute(null);
            // 左翻
            keyEventRunner.RegisterAction(VirtualKey.A, left);
            keyEventRunner.RegisterAction(VirtualKey.Left, left);
            // 右翻
            keyEventRunner.RegisterAction(VirtualKey.D, right);
            keyEventRunner.RegisterAction(VirtualKey.Right, right);
            // 显示设置
            keyEventRunner.RegisterAction(VirtualKey.Y, toggle);
            keyEventRunner.RegisterAction(VirtualKey.GamepadY, toggle);
            // 单页/双页
            keyEventRunner.RegisterAction(VirtualKey.X, pageCount);
            keyEventRunner.RegisterAction(VirtualKey.GamepadX, pageCount);
            // 修复合页
            keyEventRunner.RegisterAction(VirtualKey.C, repaired);
            keyEventRunner.RegisterAction(VirtualKey.GamepadB, repaired);
            // L/R R/L
            keyEventRunner.RegisterAction(VirtualKey.G, pageMode);
            keyEventRunner.RegisterAction(VirtualKey.GamepadA, pageMode);
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
        public string CurrentName => _instance.Info.Title;
        public int CurrentIndex {
            get {
                return _instance.Info.Current + 1;
            }
            set {
                _instance.Info.Current = value - 1;
                Read();
            }
        }
        public int TotalPage => _instance.Info.Total;

        public int PageCount => App.Helper.Setting.GetLocalSetting(ConfigEnum.PageCount, 1);
        public int PageMode => App.Helper.Setting.GetLocalSetting(ConfigEnum.PageMode, 1);
        public int Direction => App.Helper.Setting.GetLocalSetting(ConfigEnum.Direction, 1);
        public bool RepairedPageMode => App.Helper.Setting.GetLocalSetting(ConfigEnum.RepairedPageMode, false);

        public int FixedOffset => (RepairedPageMode && PageCount > 0 ? -1 : 0);

        public RelayCommand LeftCommand => new RelayCommand(() => {
            if (Direction == 1)
                IndexForward();
            else
                IndexBack();
            Read();
        });
        public RelayCommand RightCommand => new RelayCommand(() => {
            if (Direction == 1)
                IndexBack();
            else
                IndexForward();
            Read();
        });

        private void IndexBack() {
            _instance.Info.Current -= PageCount + 1;
        }

        private void IndexForward() {
            _instance.Info.Current += PageCount + 1;
        }

        public RelayCommand ShowCommand => new RelayCommand(() => {
            ShowSetting = true;
        });

        private void Content_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            keyEventRunner[e.Key]?.Invoke();
        }

        private int NextIndex() {
            if (!_instance.CanMove(PageCount, FixedOffset))
                return -1;
            var current = _instance.Info.Offset + FixedOffset + _instance.Info.Current;
            return current;
        }


        private async Task<BitmapImage[]> ImagesToShow() {
            var next = NextIndex();
            BitmapImage first = await mangaReadingService.ReadAsync(next);
            BitmapImage second = null;
            if (PageCount == 1) {
                next = NextIndex() + 1;
                second = await mangaReadingService.ReadAsync(next);
            }
            return new BitmapImage[] { first, second };

        }

        private async void Read() {
            var images = await ImagesToShow();
            if (PageCount == 0)
                Left = images[0];
            else {
                if (PageMode == 1) {
                    Right = images[0];
                    Left = images[1];
                } else {
                    Left = images[0];
                    Right = images[1];
                }
            }
            // save current
            await mangaReadingService.SaveReadingProgressAsync();
            RaisePropertyChanged(nameof(CurrentIndex));
        }
    }
}
