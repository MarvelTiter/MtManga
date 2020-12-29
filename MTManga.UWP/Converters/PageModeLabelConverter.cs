using MT.UWP.ControlLib.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.Converters {
    public class PageModeLabelConverter : BaseConverter {
        public override object Convert(object value, Type targetType, object parameter, string language) {
            if ((int)value == 0) {
                return "单页模式(X)";
            } else {
                return "双页模式(X)"; 
            }
        }
    }
}
