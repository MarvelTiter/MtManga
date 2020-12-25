using CommonServiceLocator;
using MT.MVVM.Core.View;
using MTManga.UWP.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MTManga.UWP {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ShellPage : Page {
        public ShellPage() {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            //ShellFrame.Navigate(typeof(Home));
            var navtor = ServiceLocator.Current.GetInstance<NavigationServiceList>();
            navtor["ShellFrame"].NavigateTo(nameof(Home));
        }
    }
}
