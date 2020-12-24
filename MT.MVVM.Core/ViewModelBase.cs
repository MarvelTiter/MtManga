using MT.MVVM.Core.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MT.MVVM.Core {
    public class ViewModelBase : ObservableObject, INavigable {
        public virtual void OnNavigateFrom(NavigatedArgs e) {
        }

        public virtual void OnNavigateTo(NavigationEventArgs e) {
        }

        public virtual void OnNavigatingFrom(NavigatingCancelEventArgs e) {
        }
    }
}
