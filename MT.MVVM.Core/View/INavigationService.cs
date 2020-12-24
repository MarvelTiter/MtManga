using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MT.MVVM.Core.View {
    public interface INavigationService {

        Frame CurrentFrame { get; set; }

        string CurrentPageKey { get; }

        void GoBack();

        void GoForward();

        bool NavigateTo(string pageKey, NavigationTransitionInfo infoOverride = null);

        bool NavigateTo(string pageKey, object parameter, NavigationTransitionInfo infoOverride = null);

    }
}
