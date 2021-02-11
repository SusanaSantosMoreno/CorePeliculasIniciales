using CorePeliculasIniciales.Models;
using CorePeliculasIniciales.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePeliculasIniciales.Controllers {
    public class CarritoController : Controller {

        PeliculasRepository repository;

        public CarritoController(PeliculasRepository repos) { this.repository = repos; }

        public IActionResult GetCarrito () {
            return PartialView("_CarritoPartial");
        }
    }
}
