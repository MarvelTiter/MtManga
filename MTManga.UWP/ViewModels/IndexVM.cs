using MT.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MTManga.UWP.ViewModels {
    public class IndexVM : ObservableObject {
        private string _Text;
        public string Text {
            get { return _Text; }
            set { SetValue(ref _Text, value); }
        }

        public RelayCommand UpdateCommand => new RelayCommand(Update);

        private void Update() {
            Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
