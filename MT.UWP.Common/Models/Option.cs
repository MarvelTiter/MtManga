using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.UWP.Common.Models {
    public class Option {
        public string SettingContainerName { get; set; }


        public Option(string settingContainer) {
            SettingContainerName = settingContainer;
        }
    }
}
