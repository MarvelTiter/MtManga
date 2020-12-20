using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MT.MVVM.Core.View {
    public interface INavigationService {

        Frame CurrentFrame { get; set; }

        string CurrentPageKey { get; }

        void GoBack();

        void NavigateTo(string pageKey);

        void NavigateTo(string pageKey, object parameter);

    }
}
