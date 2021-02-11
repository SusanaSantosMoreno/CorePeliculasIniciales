using CorePeliculasIniciales.Helpers;
using CorePeliculasIniciales.Models;
using CorePeliculasIniciales.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePeliculasIniciales.Controllers {
    public class PeliculasController : Controller {

        IMemoryCache memoryCache;
        PeliculasRepository repository;
        UploadService uploadService;

        public PeliculasController (PeliculasRepository repos, IMemoryCache cache, UploadService service) { 
            this.repository = repos;
            this.memoryCache = cache;
            this.uploadService = service;
        }

        public IActionResult Index (int? posicion, int? idGenero, int? peliculaComprada) {
            List<PeliculasPag> peliculas = new List<PeliculasPag>();
            int registros = 0;
            posicion = posicion == null ? 1 : posicion;
            if (idGenero != null) {
                peliculas = this.repository.GetPeliculasPagsGeneros(posicion.Value, (int)idGenero, ref registros);
                ViewData["Genero"] = idGenero;
            } else {
                peliculas = this.repository.GetPeliculasPags(posicion.Value, ref registros);
            }

            ViewData["registros"] = registros;
            if (peliculaComprada != null) {

                /*SESSION*/
                Pelicula pelicula = this.repository.GetPelicula((int)peliculaComprada);
                if (HttpContext.Session.GetString("PeliculasCompradas") == null) {
                    HttpContext.Session.SetString("PeliculasCompradas", "1");
                } else {
                    String peliculasCompradas = HttpContext.Session.GetString("PeliculasCompradas");
                    int pelCom = int.Parse(peliculasCompradas) + 1;
                    HttpContext.Session.SetString("PeliculasCompradas", pelCom.ToString());
                }

                if (HttpContext.Session.GetString("PrecioTotal") == null) {
                    HttpContext.Session.SetString("PrecioTotal", pelicula.Precio.ToString());
                } else {
                    String precio = HttpContext.Session.GetString("PrecioTotal");
                    int total = int.Parse(precio) + pelicula.Precio;
                    HttpContext.Session.SetString("PrecioTotal", total.ToString());
                }

                /*CACHE*/
                List<Pelicula> listaPeliculasCompradas = new List<Pelicula>();
                if (this.memoryCache.Get("PeliculasCompradas") == null) {
                    listaPeliculasCompradas.Add(this.repository.GetPelicula((int)peliculaComprada));
                    this.memoryCache.Set("PeliculasCompradas", ToolkitService.SerializeJsonObject(listaPeliculasCompradas));
                } else {
                    listaPeliculasCompradas = ToolkitService.DeserializeJsonObject<List<Pelicula>>
                        (this.memoryCache.Get("PeliculasCompradas").ToString());
                    listaPeliculasCompradas.Add(this.repository.GetPelicula((int)peliculaComprada));
                    this.memoryCache.Set("PeliculasCompradas", ToolkitService.SerializeJsonObject(listaPeliculasCompradas));
                }
            }
            return View(peliculas);
        }

        public IActionResult DetallePelicula (int idPelicula) {
            Pelicula pelicula = this.repository.GetPelicula(idPelicula);
            return View(pelicula);
        }

        public IActionResult FinalizarCompra (int? eliminado) {

            List<Pelicula> peliculasCompradas = new List<Pelicula>();
            if (this.memoryCache.Get("PeliculasCompradas") != null) {
                peliculasCompradas = ToolkitService.DeserializeJsonObject<List<Pelicula>>
                       (this.memoryCache.Get("PeliculasCompradas").ToString());
                if (eliminado != null) {
                    peliculasCompradas.RemoveAll(x => x.IdPelicula == eliminado);
                    this.memoryCache.Set("PeliculasCompradas", ToolkitService.SerializeJsonObject(peliculasCompradas));
                }
            }
            String precioFinal = "0";
            if(HttpContext.Session.GetString("PrecioTotal") != null) {
                precioFinal = HttpContext.Session.GetString("PrecioTotal");
                if(eliminado != null) {
                    Pelicula peliculaEliminada = this.repository.GetPelicula((int)eliminado);
                    int precio = 0;
                    if (int.Parse(precioFinal) > 0) {
                        precio = int.Parse(precioFinal) - peliculaEliminada.Precio;
                    }
                    HttpContext.Session.SetString("PrecioTotal", precio.ToString());
                }
            }
            ViewData["PrecioFinal"] = precioFinal;
            return View(peliculasCompradas);
        }

        public IActionResult EditPelicula (int? idPelicula) {
            return View(this.repository.GetPelicula((int)idPelicula));
        }

        [HttpPost]
        public async Task<IActionResult> EditPelicula (int? idPelicula, IFormFile imagen, int precio) {
            if(imagen != null) {
                await this.uploadService.UploadFileAsync(imagen, Folders.Images);
                this.repository.ActualizarPelicula((int)idPelicula, precio, imagen.FileName);
            } else {
                Pelicula pelicula = this.repository.GetPelicula((int)idPelicula);
                this.repository.ActualizarPelicula((int)idPelicula, precio, pelicula.Foto);
            }
            return RedirectToAction("Index");
        }
    }
}
