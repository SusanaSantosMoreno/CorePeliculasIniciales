using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CorePeliculasIniciales.Models {

    [Table("PeliculasPag")]
    public class PeliculasPag {

        [Key]
        [Column("IdPelicula")]
        public int IdPelicula { get; set; }
        [Column("IdGenero")]
        public int IdGenero { get; set; }
        [Column("Titulo")]
        public String Titulo { get; set; }
        [Column("Argumento")]
        public String Argumento { get; set; }
        [Column("Foto")]
        public String Foto { get; set; }
        [Column("Fecha_Estreno")]
        public DateTime FechaEstreno { get; set; }
        [Column("Actores")]
        public String Actores { get; set; }
        [Column("Director")]
        public String Director { get; set; }
        [Column("Duracion")]
        public int Duracion { get; set; }
        [Column("Precio")]
        public int Precio { get; set; }
        [Column("YouTube")]
        public String YouTube { get; set; }
        [Column("EnlaceVideo")]
        public String EnlaceVideo { get; set; }
        [Column("POSICION")]
        public long Posicion { get; set; }

        public PeliculasPag () { }

        public PeliculasPag (int idPelicula, int idGenero, 
            string titulo, string argumento, string foto, 
            DateTime fechaEstreno, string actores, string director, 
            int duracion, int precio, string youTube, string enlaceVideo, long posicion) {
            IdPelicula = idPelicula;
            IdGenero = idGenero;
            Titulo = titulo;
            Argumento = argumento;
            Foto = foto;
            FechaEstreno = fechaEstreno;
            Actores = actores;
            Director = director;
            Duracion = duracion;
            Precio = precio;
            YouTube = youTube;
            EnlaceVideo = enlaceVideo;
            Posicion = posicion;
        }
    }
}
