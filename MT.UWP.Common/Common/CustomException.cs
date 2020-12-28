using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.UWP.Common {
    public class CustomException : Exception {
        public CustomException(string msg) : base(msg) {

        }
    }
}
