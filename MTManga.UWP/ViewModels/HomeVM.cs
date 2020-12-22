using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MTManga.UWP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.ViewModels {
    public class HomeVM : ObservableObject {

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


        public HomeVM() {
            Menu = new List<MenuModel>();
            Menu.Add(new MenuModel {
                Header = "首页",
                Content = null//ServiceLocator.Current.GetSingleton<IndexView>()
            }) ;
            Menu.Add(new MenuModel {
                Header = "设置",
                Content = null//ServiceLocator.Current.GetSingleton<Setting>()
            });
        }
    }

    public class MenuModel {
        public string Header { get; set; }
        public object Content { get; set; }
    }
}
