using CorePeliculasIniciales.Models;
using CorePeliculasIniciales.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePeliculasIniciales.Controllers {
    public class GenerosController : Controller {

        PeliculasRepository repository;

        public GenerosController (PeliculasRepository repos) { this.repository = repos; }

        public IActionResult GetGeneros () {
            List<Genero> generos = this.repository.GetGeneros();
            return PartialView("_GenerosPartial", generos);
        }
    }
}
