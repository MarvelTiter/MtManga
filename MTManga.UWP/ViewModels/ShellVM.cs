using CommonServiceLocator;
using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MTManga.UWP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.ViewModels {
    public class ShellVM : ViewModelBase {
        public RelayCommand ShellBackCommand => new RelayCommand(() => {
            ServiceLocator.Current.GetInstance<NavigationList>()[Nav.ShellFrame].GoBack();
        });
    }
}
