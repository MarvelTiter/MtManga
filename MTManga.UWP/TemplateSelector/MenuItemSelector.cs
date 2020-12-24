using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MTManga.UWP.TemplateSelector {
    public class MenuItemSelector: DataTemplateSelector {
        public DataTemplate MenuItem { get; set; }
        protected override DataTemplate SelectTemplateCore(object item) {
            return MenuItem;
        }
    }
}
