using MT.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.Models {
    public class AppConfig : ObservableObject {

        private bool _IsHideTitleBarButtonInFullScreen;
        public bool IsHideTitleBarButtonInFullScreen {
            get { return _IsHideTitleBarButtonInFullScreen; }
            set { SetValue(ref _IsHideTitleBarButtonInFullScreen, value); }
        }
    }
}
