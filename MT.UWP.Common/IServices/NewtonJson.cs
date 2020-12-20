using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.UWP.Common.IServices {
    internal class NewtonJson : IJsonConvertService {
        public object DeserializeObject(string value) {
            return JsonConvert.DeserializeObject(value);
        }

        public T DeserializeObject<T>(string value) {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public string SerializeObject(object value) {
            return JsonConvert.SerializeObject(value);
        }
    }
}
