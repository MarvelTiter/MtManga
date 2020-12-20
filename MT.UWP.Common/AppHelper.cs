using MT.UWP.Common.IServices;
using MT.UWP.Common.Models;

namespace MT.UWP.Common {
    public class AppHelper {
        private readonly Option option;

        public SettingService Setting { get; private set; }

        public IOService IO { get; private set; }

        public AppHelper(Option option) {
            this.option = option;
            Setting = new SettingService(option);
            IO = new IOService();
        }     

    }
}
