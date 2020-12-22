using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MTManga.UWP.Converters {
    class IntBooleanConverter : BaseConverter {
        public override object Convert(object value, Type targetType, object parameter, string language) {
            if (value == null || (int)value == 0)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
    }
}
