using MT.MVVM.Core;
using MT.MVVM.Core.Ioc;
using MT.MVVM.Core.View;
using MTManga.UWP.Pages;
using MTManga.UWP.Views;

namespace MTManga.UWP.ViewModels {
    public class ViewModelLocator {
        private static ViewModelLocator _instance;
        private static object _insLock = new object();
        public static ViewModelLocator Current {
            get {
                lock (_insLock) {
                    if (_instance == null)
                        _instance = new ViewModelLocator();
                    return _instance;
                }
            }
        }

        public INavigationService NavigationService { get; set; }
        public ViewModelLocator() {
            ServiceLocator.SetLocatorProvider(() => MIoC.Default);
            InitNavigation();
            MIoC.Default.RegisterSingle<IndexVM>();
            MIoC.Default.RegisterScope<IndexView>();
            MIoC.Default.RegisterScope<Setting>();
            MIoC.Default.RegisterSingle<HomeVM>();
            _instance = this;
        }

        public HomeVM Home => ServiceLocator.Current.GetSingleton<HomeVM>();

        public IndexVM Main => ServiceLocator.Current.GetSingleton<IndexVM>();


        private void InitNavigation() {
            var nav = new NavigationService("rootFrame");
            //
            nav.Configura(nameof(MangaChapters), typeof(MangaChapters));

            NavigationService = nav;
        }
    }
}
