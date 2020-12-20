using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.MVVM.Core.Ioc {
    public interface IMIocController {
        T GetScope<T>();
        T GetSingleton<T>();
        T GetScope<T>(string key);
        T GetSingleton<T>(string key);

    }
}
