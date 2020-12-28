using CommonServiceLocator;
using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MTManga.Core.Enums;
using MTManga.Core.Models;
using MTManga.UWP.Views;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

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
                SetValue(ref _CurrentMenu, value);
            }
        }

        private int _CurrentIndex;
        public int CurrentIndex {
            get { return _CurrentIndex; }
            set {
                var item = Menu[value];
                var trans = CalcSlide(_CurrentIndex > value);
                SetValue(ref _CurrentIndex, value);
                Navigator.NavigateTo(item.Content, trans);
            }
        }
               
        public INavigationService Navigator => ServiceLocator.Current.GetInstance<NavigationList>()[Nav.IndexFrame];

        private SlideNavigationTransitionInfo CalcSlide(bool slide) {
            return new SlideNavigationTransitionInfo {
                Effect = slide ? SlideNavigationTransitionEffect.FromLeft : SlideNavigationTransitionEffect.FromRight
            };
        }

        public HomeVM() {
            Menu = new List<MenuModel>();
            Menu.Add(new MenuModel {
                Header = "首页",
                Content = nameof(IndexView)
            });
            Menu.Add(new MenuModel {
                Header = "设置",
                Content = nameof(Setting)
            });

        }

        public override void OnNavigateTo(NavigationEventArgs e) {
            CurrentIndex = 0;
        }

    }
}