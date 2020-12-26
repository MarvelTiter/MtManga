using MT.MVVM.Core;
using MTManga.UWP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.Models {
    public class AppConfig : ObservableObject {

        private bool _IsHideTitleBarButtonInFullScreen = App.Helper.Setting.GetLocalSetting(ConfigEnum.IsHideTitleBarButtonInFullScreen, false);
        public bool IsHideTitleBarButtonInFullScreen {
            get { return _IsHideTitleBarButtonInFullScreen; }
            set {
                SetValue(ref _IsHideTitleBarButtonInFullScreen, value);
                App.Helper.Setting.SaveLocalSetting(ConfigEnum.IsHideTitleBarButtonInFullScreen, value);
            }
        }

        private int _PageCount = App.Helper.Setting.GetLocalSetting(ConfigEnum.PageCount, 1);
        /// <summary>
        /// 0单页、1双页模式
        /// </summary>
        public int PageCount {
            get { return _PageCount; }
            set {
                SetValue(ref _PageCount, value);
                App.Helper.Setting.SaveLocalSetting(ConfigEnum.PageCount, value);
            }
        }

        /// <summary>
        /// 合页模式 0: L/R  1: R/L
        /// </summary>

        private int _PageMode = App.Helper.Setting.GetLocalSetting(ConfigEnum.PageMode, 1);
        public int PageMode {
            get { return _PageMode; }
            set {
                SetValue(ref _PageMode, value);
                App.Helper.Setting.GetLocalSetting(ConfigEnum.PageMode, 1);
            }
        }

        private int _Direction = App.Helper.Setting.GetLocalSetting(ConfigEnum.Direction, 1);
        /// <summary>
        /// 翻页方向  0: 左向右  1: 右向左  
        /// </summary>
        public int Direction {
            get { return _Direction; }
            set {
                SetValue(ref _Direction, value);
                App.Helper.Setting.GetLocalSetting(ConfigEnum.Direction, 1);
            }
        }
    }
}
