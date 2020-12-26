using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace MT.UWP.ControlLib {
    [TemplatePart(Name = PART_Root, Type = typeof(Grid))]
    public sealed class AcrylicMask : Control {
        private const string PART_Root = "PART_Root";
        private Grid _rootGrid;
        public AcrylicMask() {
            DefaultStyleKey = typeof(AcrylicMask);
        }

        #region property
        public object TopArea {
            get { return (object)GetValue(TopAreaProperty); }
            set { SetValue(TopAreaProperty, value); }
        }
        public static readonly DependencyProperty TopAreaProperty =
            DependencyProperty.Register(nameof(TopArea), typeof(object), typeof(AcrylicMask), new PropertyMetadata(null));

        public object BottomArea {
            get { return (object)GetValue(BottomAreaProperty); }
            set { SetValue(BottomAreaProperty, value); }
        }

        public static readonly DependencyProperty BottomAreaProperty =
            DependencyProperty.Register(nameof(BottomArea), typeof(object), typeof(AcrylicMask), new PropertyMetadata(null));


        public bool Show {
            get { return (bool)GetValue(ShowProperty); }
            set { SetValue(ShowProperty, value); }
        }

        public static readonly DependencyProperty ShowProperty =
            DependencyProperty.Register("Show", typeof(bool), typeof(AcrylicMask), new PropertyMetadata(false));

        #endregion

        protected override void OnApplyTemplate() {
            base.OnApplyTemplate();
            _rootGrid = GetTemplateChild(PART_Root) as Grid;
        }

    }
}
