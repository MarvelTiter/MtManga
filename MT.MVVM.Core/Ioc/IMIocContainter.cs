using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.MVVM.Core.Ioc {
    public interface IMIocContainter {

        IMIocContainter RegisterScope<T>()
          where T : class;
        IMIocContainter RegisterScope(Type classType);
        IMIocContainter RegisterScope<TI, T>()
           where TI : class
           where T : class, TI;

        IMIocContainter RegisterSingle<T>(bool createImmediately, string key = null)
            where T : class;
        IMIocContainter RegisterSingle(Type classType, bool createImmediately, string key = null);

        IMIocContainter RegisterSingle<TI, T>(bool createImmediately, string key = null)
          where TI : class
          where T : class, TI;

        IMIocContainter RegisterSingle<T>(Func<T> factory, string key = null)
          where T : class;

        void UnRegister<T>(string key = null);
    }
}
