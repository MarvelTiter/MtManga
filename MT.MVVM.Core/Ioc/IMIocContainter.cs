using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.MVVM.Core.Ioc {
    public interface IMIocContainter {

        void RegisterScope<T>()
          where T : class;
        void RegisterScope<TI, T>()
           where TI : class
           where T : class, TI;

        void RegisterSingle<T>(bool createImmediately, string key = null)
            where T : class;

        void RegisterSingle<TI, T>(bool createImmediately, string key = null)
          where TI : class
          where T : class, TI;

        void RegisterSingle<T>(Func<T> factory, string key = null)
          where T : class;

        void UnRegister<T>(string key = null);
    }
}
