using MTManga.UWP.Enums;
using MTManga.UWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace MTManga.UWP.Views {
    public sealed partial class Setting : Page {

        public Setting() {
            this.InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {

        }


        //public bool GetS1() {
        //    return App.AppConfig.IsHideTitleBarButtonInFullScreen;
        //}

        //public void SaveS1(bool? value) {
        //    App.AppConfig.IsHideTitleBarButtonInFullScreen = value.HasValue && value.Value;
        //}
    }

}
