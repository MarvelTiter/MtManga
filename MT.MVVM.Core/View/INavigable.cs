using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.MVVM.Core.View {
    public interface INavigable {
        void NavigateTo();
        void NavigateFrom();
    }
}
