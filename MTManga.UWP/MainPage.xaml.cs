using MTManga.UWP.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MTManga.UWP {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page {
        public List<MenuModel> Menu { get; set; }
        public MainPage() {
            this.InitializeComponent();
            Menu = new List<MenuModel>();
            Menu.Add(new MenuModel {
                Header = "首页",
                Content = new IndexView()
            });
            Menu.Add(new MenuModel {
                Header = "设置",
                Content = new Setting()
            });
            DataContext = this;
        }
    }

    public class MenuModel {
        public string Header { get; set; }
        public object Content { get; set; }
    }
}
