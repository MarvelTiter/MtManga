using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace MT.UWP.Common {
    public class KeyEventRunner {
        Dictionary<VirtualKey, Action> _events = new Dictionary<VirtualKey, Action>();

        public void RegisterAction(VirtualKey key, Action action) {
            if (_events.ContainsKey(key))
                throw new CustomException("this key is already registed: " + key.ToString());
            _events.Add(key, action);
        }

        public Action this[VirtualKey key] {
            get {
                if (_events.ContainsKey(key))
                    return _events[key];
                return null;
            }
        }
    }
}
