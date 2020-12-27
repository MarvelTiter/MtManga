using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace MTManga.UWP.Controls {
    public sealed partial class ImageViewer : UserControl {
        public ImageViewer() {
            this.InitializeComponent();
        }

        public ICommand LeftCommand {
            get { return (ICommand)GetValue(LeftCommandProperty); }
            set { SetValue(LeftCommandProperty, value); }
        }
        public static readonly DependencyProperty LeftCommandProperty =
            DependencyProperty.Register(nameof(LeftCommand), typeof(ICommand), typeof(ImageViewer), new PropertyMetadata(null));


        public ICommand RightCommand {
            get { return (ICommand)GetValue(RightCommandProperty); }
            set { SetValue(RightCommandProperty, value); }
        }
        public static readonly DependencyProperty RightCommandProperty =
            DependencyProperty.Register(nameof(RightCommand), typeof(ICommand), typeof(ImageViewer), new PropertyMetadata(null));

        public BitmapImage Left {
            get { return (BitmapImage)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register(nameof(Left), typeof(BitmapImage), typeof(ImageViewer), new PropertyMetadata(null));


        public BitmapImage Right {
            get { return (BitmapImage)GetValue(RightProperty); }
            set { SetValue(RightProperty, value); }
        }
        public static readonly DependencyProperty RightProperty =
            DependencyProperty.Register(nameof(Right), typeof(BitmapImage), typeof(ImageViewer), new PropertyMetadata(null));

        public bool SingleMode {
            get { return (bool)GetValue(SingleModeProperty); }
            set { SetValue(SingleModeProperty, value); }
        }
        public static readonly DependencyProperty SingleModeProperty =
            DependencyProperty.Register(nameof(SingleMode), typeof(bool), typeof(ImageViewer), new PropertyMetadata(null, OnModeChanged));

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var viewer = d as ImageViewer;
            viewer.LeftImage.Visibility = viewer.SingleMode ? Visibility.Collapsed : Visibility.Visible;
            viewer.RightImage.Visibility = viewer.SingleMode ? Visibility.Collapsed : Visibility.Visible;
            viewer.FullImage.Visibility = !viewer.SingleMode ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
