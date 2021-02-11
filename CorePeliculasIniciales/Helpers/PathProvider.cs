using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CorePeliculasIniciales.Helpers {
    public enum Folders {
        Images = 0
    }

    public class PathProvider {

        IWebHostEnvironment environment;

        public PathProvider (IWebHostEnvironment env) {
            this.environment = env;
        }

        //METODO PARA DEVOLVER EL MAPEO A RUTAS DE FICHEROS
        public String MapPath (String fileName, Folders folders) {
            //El nombre tiene que ser igual al del servidor
            String folder = folders.ToString().ToLower();
            String path = Path.Combine(this.environment.WebRootPath, folder, fileName);
            return path;
        }
    }
}
