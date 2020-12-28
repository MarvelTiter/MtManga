using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace MT.UWP.ControlLib {
    public sealed partial class AppTitleBar : UserControl {

        private UISettings _uiSettings;
        private AccessibilitySettings _accessibilitySettings;
        private ApplicationView _appView;
        private CoreApplicationViewTitleBar _coreTitleBar;

        private const string BackToWindow = "\uE73F";
        private const string FullScreen = "\uE740";

        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(AppTitleBar), new PropertyMetadata(null));

        public string WindowStateIcon {
            get { return (string)GetValue(WindowStateIconProperty); }
            set { SetValue(WindowStateIconProperty, value); }
        }

        public static readonly DependencyProperty WindowStateIconProperty =
            DependencyProperty.Register(nameof(WindowStateIcon), typeof(string), typeof(AppTitleBar), new PropertyMetadata(FullScreen));



        public ICommand BackCommand {
            get { return (ICommand)GetValue(BackCommandProperty); }
            set { SetValue(BackCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackCommandProperty =
            DependencyProperty.Register(nameof(BackCommand), typeof(ICommand), typeof(AppTitleBar), new PropertyMetadata(null));



        public bool HideInFullScreen {
            get { return (bool)GetValue(HideInFullScreenProperty); }
            set { SetValue(HideInFullScreenProperty, value); }
        }

        public static readonly DependencyProperty HideInFullScreenProperty =
            DependencyProperty.Register(nameof(HideInFullScreen), typeof(bool), typeof(AppTitleBar), new PropertyMetadata(false, OnHideInFullScreenChanged));

        private static async void OnHideInFullScreenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var titleBar = d as AppTitleBar;
            await titleBar.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                titleBar.AppTitleCustomButtonBar.Visibility = titleBar.HideInFullScreen && titleBar.IsFullScreenMode ? Visibility.Collapsed : Visibility.Visible;
            });
        }

        public bool IsFullScreenMode => _appView.IsFullScreenMode;


        public AppTitleBar() {
            this.InitializeComponent();
            _uiSettings = new UISettings();
            _accessibilitySettings = new AccessibilitySettings();
            _appView = ApplicationView.GetForCurrentView();
            _coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            _coreTitleBar.ExtendViewIntoTitleBar = true;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            Buttons.CollectionChanged += OnButtonsCollectionChanged;
        }

        public ObservableCollection<Button> Buttons { get; } = new ObservableCollection<Button>();

        private void OnLoaded(object sender, RoutedEventArgs e) {
            Window.Current.SetTitleBar(BackgroundElement);
            // Register events
            //_coreTitleBar.IsVisibleChanged += OnIsVisibleChanged;
            _coreTitleBar.LayoutMetricsChanged += OnLayoutMetricsChanged;

            _uiSettings.ColorValuesChanged += OnColorValuesChanged;
            _accessibilitySettings.HighContrastChanged += OnHighContrastChanged;
            _appView.VisibleBoundsChanged += OnVisibleBoundsChanged;
            Window.Current.Activated += OnWindowActivated;
            // Set properties
            LayoutRoot.Height = _coreTitleBar.Height;
            SetTitleBarControlColors();

            SetTitleBarPadding();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e) {
            // Unregister events
            _coreTitleBar.LayoutMetricsChanged -= OnLayoutMetricsChanged;
            //_coreTitleBar.IsVisibleChanged -= OnIsVisibleChanged;
            _uiSettings.ColorValuesChanged -= OnColorValuesChanged;
            _accessibilitySettings.HighContrastChanged -= OnHighContrastChanged;
            Window.Current.Activated -= OnWindowActivated;
            _appView.VisibleBoundsChanged -= OnVisibleBoundsChanged;
        }


        private void SetTitleBarPadding() {
            double leftAddition = 0;
            double rightAddition = 0;

            if (FlowDirection == FlowDirection.LeftToRight) {
                leftAddition = _coreTitleBar.SystemOverlayLeftInset;
                rightAddition = _coreTitleBar.SystemOverlayRightInset;
            } else {
                leftAddition = _coreTitleBar.SystemOverlayRightInset;
                rightAddition = _coreTitleBar.SystemOverlayLeftInset;
            }

            LayoutRoot.Padding = new Thickness(leftAddition, 0, rightAddition, 0);
        }

        private void OnIsVisibleChanged(CoreApplicationViewTitleBar sender, object args) {

        }

        private void OnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args) {
            LayoutRoot.Height = _coreTitleBar.Height;
            SetTitleBarPadding();
        }

        private async void OnColorValuesChanged(UISettings sender, object e) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { SetTitleBarControlColors(); });
        }

        private void SetTitleBarControlColors() {
            if (_appView == null)
                return;

            var applicationTitleBar = _appView.TitleBar;
            if (applicationTitleBar == null)
                return;

            if (_accessibilitySettings.HighContrast) {
                // Reset to use default colors.
                applicationTitleBar.ButtonBackgroundColor = null;
                applicationTitleBar.ButtonForegroundColor = null;
                applicationTitleBar.ButtonInactiveBackgroundColor = null;
                applicationTitleBar.ButtonInactiveForegroundColor = null;
                applicationTitleBar.ButtonHoverBackgroundColor = null;
                applicationTitleBar.ButtonHoverForegroundColor = null;
                applicationTitleBar.ButtonPressedBackgroundColor = null;
                applicationTitleBar.ButtonPressedForegroundColor = null;
            } else {
                Color bgColor = Colors.Transparent;
                applicationTitleBar.ButtonBackgroundColor = bgColor;
                applicationTitleBar.ButtonInactiveBackgroundColor = bgColor;
            }
        }

        private async void OnHighContrastChanged(AccessibilitySettings sender, Object args) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                SetTitleBarControlColors();
            });
        }

        private void OnWindowActivated(Object sender, WindowActivatedEventArgs e) {
            VisualStateManager.GoToState(
                this, e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);
        }

        private void OnButtonsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            ItemsPanel.Children.Clear();
            foreach (var button in Buttons) {
                ItemsPanel.Children.Add(button);
            }
        }

        private void OnVisibleBoundsChanged(ApplicationView sender, object args) {
            AppTitleCustomButtonBar.Visibility = HideInFullScreen && IsFullScreenMode ? Visibility.Collapsed : Visibility.Visible;
            SetButtonIcon();
        }

        private void SetButtonIcon() {
            if (IsFullScreenMode) {
                SetValue(WindowStateIconProperty, BackToWindow);
            } else {
                SetValue(WindowStateIconProperty, FullScreen);
            }
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e) {
            if (IsFullScreenMode) {
                _appView.ExitFullScreenMode();
            } else {
                _appView.TryEnterFullScreenMode();
            }

        }
    }
}
