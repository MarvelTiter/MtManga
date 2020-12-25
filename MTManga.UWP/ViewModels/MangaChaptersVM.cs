using CommonServiceLocator;
using MT.MVVM.Core;
using MT.MVVM.Core.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MTManga.UWP.ViewModels {
    public class MangaChaptersVM : ViewModelBase {
        //public INavigationService Navigator => ViewModelLocator.Current.NavigationService[ViewModelLocator.ShellFrame];

        public RelayCommand BackCommand => new RelayCommand(() => {
            ServiceLocator.Current.GetInstance<NavigationServiceList>()["ShellFrame"].GoBack();
        });
    }
}
