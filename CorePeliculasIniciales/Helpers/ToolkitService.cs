using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePeliculasIniciales.Helpers {
    public class ToolkitService {

        public static String SerializeJsonObject(object obj) {
            return JsonConvert.SerializeObject(obj);
        }

        public static T DeserializeJsonObject<T>(String json) {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
