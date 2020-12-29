using MT.UWP.ControlLib.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MTManga.UWP.Converters {
    public class DirectionIconConverter : BaseConverter {
        public override object Convert(object value, Type targetType, object parameter, string language) {
            var icon = new FontIcon();

            if ((int)value == 0) {
                icon.Glyph = "\uF0AF";
            } else {
                icon.Glyph = "\uF0B0";
            }
            return icon;

        }
    }
}
