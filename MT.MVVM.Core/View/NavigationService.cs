using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MT.MVVM.Core.View {
    public class NavigationService : INavigationService {
        private const string Root = "__Root__";
        private const string Unknow = "__Unknow__";

        public NavigationService() {

        }

        public NavigationService(string customFrameName) {
            this.customFrameName = customFrameName;
        }

        private Frame FindFrameFromVisualTreeByName(DependencyObject parent) {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            if (count == 0) {
                return null;
            }
            for (var i = 0; i < count; i++) {
                var fe = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (fe is Frame frame) {
                    if (frame.Name == customFrameName) {
                        return frame;
                    }
                } else {
                    fe = FindFrameFromVisualTreeByName(fe);
                    if (fe != null) {
                        return fe as Frame;
                    }
                }
                
            }
            return null;
        }

        public string CurrentPageKey {
            get {
                lock (_pagesByKey) {
                    if (CurrentFrame.BackStackDepth == 0) {
                        return Root;
                    }

                    if (CurrentFrame.Content == null) {
                        return Unknow;
                    }

                    Type currentType = CurrentFrame.Content.GetType();
                    if (!_pagesByKey.ContainsValue(currentType)) {
                        return Unknow;
                    }

                    return _pagesByKey.FirstOrDefault((KeyValuePair<string, Type> i) => (object)i.Value == currentType).Key;
                }
            }
        }
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private readonly string customFrameName;
        private Frame _currentFrame;
        public Frame CurrentFrame {
            get {
                if (_currentFrame == null)
                    _currentFrame = FindFrameFromVisualTreeByName(Window.Current.Content);
                return _currentFrame;
            }
            set => _currentFrame = value;
        }

        public void GoBack() {
            if (CurrentFrame.CanGoBack) {
                CurrentFrame.GoBack();
            }
        }

        public void NavigateTo(string pageKey) {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter) {
            lock (_pagesByKey) {
                if (!_pagesByKey.ContainsKey(pageKey)) {
                    throw new ArgumentException($"No such page: {pageKey}.", "pageKey");
                }
                CurrentFrame.Navigate(_pagesByKey[pageKey], parameter);
            }
        }

        public void Configura(string key, Type pageType) {
            lock (_pagesByKey) {
                if (_pagesByKey.ContainsKey(key)) {
                    throw new ArgumentException("This key is already used: " + key);
                }

                if (_pagesByKey.ContainsValue(pageType)) {
                    throw new ArgumentException("This type is already configured with key " + _pagesByKey.First((KeyValuePair<string, Type> p) => (object)p.Value == pageType).Key);
                }

                _pagesByKey.Add(key, pageType);
            }
        }
    }
}
