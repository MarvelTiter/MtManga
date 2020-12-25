using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.MVVM.Core.View {
    public class NavigationServiceList {

        private readonly Dictionary<string, INavigationService> _services = new Dictionary<string, INavigationService>();
        private static NavigationServiceList _instance;
        public static NavigationServiceList Instance => _instance ?? (_instance = new NavigationServiceList());

        public INavigationService this[string key] => _services[key];

        public void Register(string key, INavigationService nav) {
            if (IsRegistered(key))
                throw new ArgumentException("This key is already used:" + key);
            _services.Add(key, nav);
        }
        public void Unregister(string key) {
            if (IsRegistered(key))
                _services.Remove(key);
        }

        public bool IsRegistered(string key) {
            return _services.ContainsKey(key);
        }
    }
}
