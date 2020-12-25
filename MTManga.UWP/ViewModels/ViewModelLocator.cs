using CommonServiceLocator;
using MT.MVVM.Core.Ioc;
using MT.MVVM.Core.View;
using MTManga.UWP.Enums;
using MTManga.UWP.Services;
using MTManga.UWP.Views;

namespace MTManga.UWP.ViewModels {

    public class ViewModelLocator {
        public const string ShellFrame = "ShellFrame";
        public const string IndexFrame = "IndexFrame";

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

        public ViewModelLocator() {
            ServiceLocator.SetLocatorProvider(() => MIoC.Default);
            MIoC.Default.RegisterSingle<IndexVM>(false)
                .RegisterSingle<HomeVM>(false)
                .RegisterScope<IMangaCollectionService, LocalMangaCollection>()
                .RegisterScope<IMangaService, LocalManga>()
                .RegisterScope<MangaChaptersVM>()
                .RegisterSingle<ShellVM>(false)
                .RegisterSingle(() => RegisterContentView())
                .RegisterSingle(() => InitNavigation())
                ;
            _instance = this;
        }

        public HomeVM Home => ServiceLocator.Current.GetInstance<HomeVM>();

        public IndexVM Main => ServiceLocator.Current.GetInstance<IndexVM>();

        public MangaChaptersVM Chapters => ServiceLocator.Current.GetInstance<MangaChaptersVM>();

        public MangaReadVM Manga => ServiceLocator.Current.GetInstance<MangaReadVM>();

        public ShellVM Shell => ServiceLocator.Current.GetInstance<ShellVM>();


        private NavigationList InitNavigation() {
            var service = NavigationList.Instance;
            var shellNav = new NavigationService(Nav.ShellFrame);
            //
            shellNav.Configura<Home>();
            shellNav.Configura<MangaChapters>();
            shellNav.Configura<MangaRead>();
            service.Register(Nav.ShellFrame, shellNav);

            var thrNav = new NavigationService(Nav.IndexFrame);
            thrNav.Configura<IndexView>();
            thrNav.Configura<Setting>();
            service.Register(Nav.IndexFrame, thrNav);

            return service;
        }

        private ContentViewService RegisterContentView() {
            ContentViewService service = new ContentViewService();
            return service;
        }
    }
}
