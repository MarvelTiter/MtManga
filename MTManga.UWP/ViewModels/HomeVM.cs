using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MTManga.UWP.Pages;
using MTManga.UWP.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace MTManga.UWP.ViewModels {
    public class HomeVM : ViewModelBase {

        private List<MenuModel> _Menu;
        public List<MenuModel> Menu {
            get { return _Menu; }
            set { SetValue(ref _Menu, value); }
        }

        private object _Content;
        public object Content {
            get { return _Content; }
            set { SetValue(ref _Content, value); }
        }

        private MenuModel _CurrentMenu;
        public MenuModel CurrentMenu {
            get { return _CurrentMenu; }
            set {
                if (value.Content.Name == nameof(IndexView)) {
                    Content = ServiceLocator.Current.GetScope<IndexView>();
                } else if (value.Content.Name == nameof(Setting)) {
                    Content = ServiceLocator.Current.GetScope<Setting>();

                }
                SetValue(ref _CurrentMenu, value);
            }
        }


        public HomeVM() {
            Menu = new List<MenuModel>();
            Menu.Add(new MenuModel {
                Header = "首页",
                Content = typeof(IndexView)
            });
            Menu.Add(new MenuModel {
                Header = "设置",
                Content = typeof(Setting)
            });
        }

        public RelayCommand NavigateCommand => new RelayCommand(() => {
            ViewModelLocator.Current.NavigationService.NavigateTo(nameof(MangaChapters));
        });

    }

    public class MenuModel {
        public string Header { get; set; }
        public Type Content { get; set; }
    }
}
