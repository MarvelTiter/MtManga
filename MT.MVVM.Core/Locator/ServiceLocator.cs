using MT.MVVM.Core.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.MVVM.Core {
    public class ServiceLocator {
        public static IMIocController Current { get; private set; }
        public static void SetLocatorProvider(Func<IMIocController> func) {
            Current = func.Invoke();
        }
    }
}
