using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CorePeliculasIniciales.Helpers {
    public class UploadService {
        PathProvider PathProvider;

        public UploadService (PathProvider pathprovider) {
            this.PathProvider = pathprovider;
        }

        public async Task<String>
            UploadFileAsync (IFormFile formfile, Folders folder) {
            String filename = formfile.FileName;
            String path =
                this.PathProvider.MapPath(filename, Folders.Images);
            using (var stream = new FileStream(path, FileMode.Create)) {
                await formfile.CopyToAsync(stream);
            }
            return path;
        }
    }
}
