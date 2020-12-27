using MT.MVVM.Core;
using MTManga.UWP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.ViewModels {
    public class SettingVM : ViewModelBase {
        public RelayCommand ClearCommand => new RelayCommand(async () => {
            await App.Helper.IO.ClearLocalDataAsync();
        });

        public RelayCommand PickFolderCommand => new RelayCommand(async () => {
            var folder = await App.Helper.IO.GetUserFolderAsync();
            if (folder == null)
                return;
            var token = App.Helper.IO.SaveFolder(folder);
            App.Helper.Setting.SaveLocalSetting(ConfigEnum.RootFolderToken, token);
        });
    }
}
