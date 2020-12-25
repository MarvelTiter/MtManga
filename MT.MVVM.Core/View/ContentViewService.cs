using MT.MVVM.Core.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.MVVM.Core.View {
    public class ContentViewService {
        private List<Type> _views = new List<Type>();

        public void Configura<T>() where T : class {
            var contentType = typeof(T);
            if (_views.Contains(contentType))
                throw new ArgumentException("This type is already configured");

            _views.Add(contentType);
        }

        public object GetView<T>() where T : class, new() {
            var contentType = typeof(T);
            if (!_views.Contains(contentType))
                throw new ArgumentException("This type is not configura: " + contentType.Name);
            var view = new T();
            return view;
        }
    }
}
