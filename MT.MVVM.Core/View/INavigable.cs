﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MT.MVVM.Core.View {
    public interface INavigable {
        void OnNavigateTo(NavigationEventArgs e);
        void OnNavigateFrom(NavigatedArgs e);
        void OnNavigatingFrom(NavigatingCancelEventArgs e);
    }
}
