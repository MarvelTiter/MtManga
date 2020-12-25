using CommonServiceLocator;
using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MTManga.UWP.Entities;
using MTManga.UWP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MTManga.UWP.ViewModels {
    public class MangaChaptersVM : ViewModelBase {
        private MangaEntity _Model;
        public MangaEntity Model {
            get { return _Model; }
            set { SetValue(ref _Model, value); }
        }


        public override void OnNavigateTo(NavigationEventArgs e) {
            Model = e.Parameter as MangaEntity;
        }

    }
}
