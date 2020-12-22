using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MTManga.UWP.Converters {
    public abstract class BaseConverter:IValueConverter {
        public abstract object Convert(object value, Type targetType, object parameter, string language);

        public virtual object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
