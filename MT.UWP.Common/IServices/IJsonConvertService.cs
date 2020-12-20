using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.UWP.Common.IServices {
    internal interface IJsonConvertService {
        string SerializeObject(object value);
        object DeserializeObject(string value);
        T DeserializeObject<T>(string value);
    }
}
