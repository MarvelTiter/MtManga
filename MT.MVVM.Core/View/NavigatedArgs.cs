using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MT.MVVM.Core.View {
    public class NavigatedArgs {
        public object Content { get; set; }
        public NavigationMode NavigationMode { get; set; }
    }
}
