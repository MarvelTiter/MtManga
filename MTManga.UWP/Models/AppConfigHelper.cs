using MT.MVVM.Core;
using MTManga.UWP.Enums;
using MTManga.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.Models {
    public class AppConfigHelper:ObservableObject {

        private AppConfig _Config;
        public AppConfig Config {
            get { return _Config; }
            set { SetValue(ref _Config, value); }
        }

        public AppConfigHelper() {
            Config = App.Helper.Setting.GetSetting<AppConfig>(ConfigEnum.All);
        }
    }
}
