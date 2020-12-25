﻿using CommonServiceLocator;
using MT.MVVM.Core;
using MT.MVVM.Core.View;
using MTManga.UWP.Views;
using System;
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
                SetValue<int>(ref _CurrentIndex, value);
                Navigator.NavigateTo(item.Content.Name, trans);
            }
        }

        public INavigationService Navigator => ServiceLocator.Current.GetInstance<NavigationServiceList>()[ViewModelLocator.IndexFrame];

        private SlideNavigationTransitionInfo CalcSlide(bool slide) {
            return new SlideNavigationTransitionInfo {
                Effect = slide ? SlideNavigationTransitionEffect.FromLeft : SlideNavigationTransitionEffect.FromRight
            };
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

        public override void OnNavigateTo(NavigationEventArgs e) {
            CurrentIndex = 0;
        }
    }

    public class MenuModel {
        public string Header { get; set; }
        public Type Content { get; set; }
    }
}
