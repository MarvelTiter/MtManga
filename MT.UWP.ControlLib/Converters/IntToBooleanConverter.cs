using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.UWP.ControlLib.Converters {
    public class IntToBooleanConverter : BaseConverter {
        public override object Convert(object value, Type targetType, object parameter, string language) {
            return (int)value == 0;
        }
    }
}
