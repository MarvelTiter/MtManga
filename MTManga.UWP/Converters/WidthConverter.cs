using MT.UWP.ControlLib.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MTManga.UWP.Converters {
    public class WidthConverter : BaseConverter {
        public override object Convert(object value, Type targetType, object parameter, string language) {
            if ((int)value == 0) {
                return 0;
            } else {                
                return Window.Current.Content.ActualSize.X / 2; 
            }
        }
    }
}
