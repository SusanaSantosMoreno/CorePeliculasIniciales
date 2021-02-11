using CorePeliculasIniciales.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePeliculasIniciales.Data {
    public class PeliculasContext : DbContext {

        public PeliculasContext (DbContextOptions<PeliculasContext> options) : base(options) { }

        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero> Generos { get; set; }

        public DbSet<PeliculasPag> PeliculasPag { get; set; }
    }
}
