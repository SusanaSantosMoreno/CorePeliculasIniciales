using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CorePeliculasIniciales.Models {
    [Table("Generos")]
    public class Genero {

        [Key]
        [Column("IdGenero")]
        public int IdGenero { get; set; }
        [Column("Genero")]
        public String Descripcion { get; set; }

        public Genero () {}

        public Genero (int idGenero, string descripcion) {
            IdGenero = idGenero;
            Descripcion = descripcion;
        }
    }
}
