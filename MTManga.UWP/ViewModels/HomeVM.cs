using CommonServiceLocator;
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

                SetValue(ref _CurrentMenu, value);
            }
        }


        public HomeVM() {
            Menu = new List<MenuModel>();
            Menu.Add(new MenuModel {
                Header = "首页",
                //Content = ServiceLocator.Current.GetInstance<IndexView>()
            });
            Menu.Add(new MenuModel {
                Header = "设置",
                //Content = ServiceLocator.Current.GetInstance<Setting>()
            });
            
        }

        public RelayCommand NavigateCommand => new RelayCommand(() => {
            ViewModelLocator.Current.NavigationService.NavigateTo(nameof(MangaChapters));
        });


    }

    public class MenuModel {
        public string Header { get; set; }
        public object Content { get; set; }
    }
}
