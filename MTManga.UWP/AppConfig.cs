using MT.MVVM.Core;
using MTManga.Core.Enums;
using System;

namespace MTManga.UWP {
    public class AppConfig : ObservableObject {

        public static event Action PageModeChanged;
        public static event Action PageCountChanged;
        public static event Action DirectionChanged;
        public static event Action FixedChanged;

        public RelayCommand UpdatePageCountCommand => new RelayCommand(() => {
            PageCount += 1;
        });
        public RelayCommand UpdatePageModeCommand => new RelayCommand(() => {
            PageMode += 1;
        });
        public RelayCommand UpdateDirectionCommand => new RelayCommand(() => {
            Direction += 1;
        });
        public RelayCommand UpdateRepairedPageModeCommand => new RelayCommand(() => {
            RepairedPageMode = !RepairedPageMode;
        });

        private bool _IsHideTitleBarButtonInFullScreen = App.Helper.Setting.GetLocalSetting(ConfigEnum.IsHideTitleBarButtonInFullScreen, false);
        public bool IsHideTitleBarButtonInFullScreen {
            get { return _IsHideTitleBarButtonInFullScreen; }
            set {
                SetValue(ref _IsHideTitleBarButtonInFullScreen, value);
                App.Helper.Setting.SaveLocalSetting(ConfigEnum.IsHideTitleBarButtonInFullScreen, value);
            }
        }

        private int _PageCount = App.Helper.Setting.GetLocalSetting(ConfigEnum.PageCount, 1);
        public int PageCount {
            get { return _PageCount; }
            set {
                value = value % 2;
                SetValue(ref _PageCount, value);
                App.Helper.Setting.SaveLocalSetting(ConfigEnum.PageCount, value);
                PageCountChanged?.Invoke();
            }
        }


        private int _PageMode = App.Helper.Setting.GetLocalSetting(ConfigEnum.PageMode, 1);
        public int PageMode {
            get { return _PageMode; }
            set {
                value = value % 2;
                SetValue(ref _PageMode, value);
                App.Helper.Setting.SaveLocalSetting(ConfigEnum.PageMode, value);
                PageModeChanged?.Invoke();
            }
        }

        private int _Direction = App.Helper.Setting.GetLocalSetting(ConfigEnum.Direction, 1);
        public int Direction {
            get { return _Direction; }
            set {
                value = value % 2;
                SetValue(ref _Direction, value);
                App.Helper.Setting.SaveLocalSetting(ConfigEnum.Direction, value);
                DirectionChanged?.Invoke();
            }
        }

        private int _GroupSize = App.Helper.Setting.GetLocalSetting(ConfigEnum.GroupSize, 20);
        public int GroupSize {
            get { return _GroupSize; }
            set {
                SetValue(ref _GroupSize, value);
                App.Helper.Setting.SaveLocalSetting(ConfigEnum.GroupSize, value);
            }
        }

        private bool _RepairedPageMode = App.Helper.Setting.GetLocalSetting(ConfigEnum.RepairedPageMode, false);
        public bool RepairedPageMode {
            get { return _RepairedPageMode; }
            set {
                SetValue(ref _RepairedPageMode, value);
                App.Helper.Setting.SaveLocalSetting(ConfigEnum.RepairedPageMode, value);
                FixedChanged?.Invoke();
            }
        }

    }
}
