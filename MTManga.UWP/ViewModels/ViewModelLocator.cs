using MT.MVVM.Core;
using MT.MVVM.Core.Ioc;
using MT.MVVM.Core.View;

namespace MTManga.UWP.ViewModels {
    public class ViewModelLocator {
        private static ViewModelLocator _instance;
        public static ViewModelLocator Current {
            get {
                lock (_instance) {
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
        }

        public IndexVM Main => ServiceLocator.Current.GetSingleton<IndexVM>();


        private void InitNavigation() {
            var nav = new NavigationService();
            //

            NavigationService = nav;
        }
    }
}
