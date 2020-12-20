using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.MVVM.Core.Ioc {
    public interface IMIocContainter {
        void RegisterScope<TI, T>()
            where TI : class
            where T : class, TI;

        void RegisterSingle<TI, T>()
           where TI : class
           where T : class, TI;

        void RegisterScope<T>()
           where T : class;

        void RegisterSingle<T>()
            where T : class;

        void RegisterSingle<TI, T>(T ins)
           where TI : class
           where T : class, TI;

        void RegisterSingle<TI, T>(T ins, string key)
           where TI : class
           where T : class, TI;
    }
}
