using CorePeliculasIniciales.Data;
using CorePeliculasIniciales.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#region VISTAS
/*
 CREATE VIEW PeliculasPag
AS
	SELECT ROW_NUMBER() OVER (ORDER BY IdPelicula)
	AS POSICION
	, Peliculas.* FROM Peliculas
GO
 */
#endregion
#region PROCEDURES
/*
CREATE PROCEDURE PaginarPeliculas
(@POSICION INT, @REGISTROS INT OUT)
AS
	SELECT @REGISTROS = COUNT(IdPelicula)
	FROM PeliculasPag
	SELECT * FROM PeliculasPag
	WHERE POSICION >= @POSICION AND 
	POSICION < (@POSICION + 6)
GO

CREATE PROCEDURE PaginarPeliculasGenero
(@POSICION INT
, @GENERO INT
, @REGISTROS INT OUT)
AS
	SELECT @REGISTROS = COUNT(IdPelicula) FROM Peliculas WHERE IdGenero = @GENERO
	
	SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Titulo) AS POSICION, Peliculas.*
	FROM Peliculas WHERE IdGenero = @GENERO) as CONSULTA 
	WHERE (POSICION >= @POSICION AND POSICION < (@POSICION + 6))
GO
 */
#endregion
namespace CorePeliculasIniciales.Repositories {
    public class PeliculasRepository {

        PeliculasContext context;

        public PeliculasRepository(PeliculasContext context) {
            this.context = context;
        }

        public List<Genero> GetGeneros () {
            return this.context.Generos.ToList();
        } 

        public List<Pelicula> GetPeliculas () {
            return this.context.Peliculas.ToList();
        }

        public Genero GetGenero(int idGenero) {
            return this.context.Generos.Where(x => x.IdGenero == idGenero).FirstOrDefault();
        }

        public Pelicula GetPelicula(int idPelicula) {
            return this.context.Peliculas.Where(x => x.IdPelicula == idPelicula).FirstOrDefault();
        }

        public List<Pelicula> GetPeliculasPorGenero(int idGenero) {
            return this.context.Peliculas.Where(x => x.IdGenero == idGenero).ToList();
        }

        public List<PeliculasPag> GetPeliculasPags(int posicion, ref int numPels) {
            SqlParameter paramPosicion = new SqlParameter("@POSICION", posicion);
            SqlParameter paramRegistros = new SqlParameter("@REGISTROS", numPels);
            paramRegistros.Direction = System.Data.ParameterDirection.Output;
            List<PeliculasPag> peliculas = this.context.PeliculasPag.FromSqlRaw(
                "PaginarPeliculas @POSICION, @REGISTROS OUT", paramPosicion, paramRegistros).ToList();
            numPels = Convert.ToInt32(paramRegistros.Value);
            return peliculas;
        }

        public List<PeliculasPag> GetPeliculasPagsGeneros (int posicion, int idGenero, ref int numPels) {
            SqlParameter paramPosicion = new SqlParameter("@POSICION", posicion);
            SqlParameter paramGenero = new SqlParameter("@GENERO", idGenero);
            SqlParameter paramRegistros = new SqlParameter("@REGISTROS", numPels);
            paramRegistros.Direction = System.Data.ParameterDirection.Output;
            List<PeliculasPag> peliculas = this.context.PeliculasPag.FromSqlRaw(
                "PaginarPeliculasGenero @POSICION, @GENERO, @REGISTROS OUT", paramPosicion, paramGenero, paramRegistros).ToList();
            numPels = Convert.ToInt32(paramRegistros.Value);
            return peliculas;
        }

        public void ActualizarPelicula(int idPelicula, int precio, string foto) {
            Pelicula pelicula = this.GetPelicula(idPelicula);
            pelicula.Precio = precio;
            pelicula.Foto = foto;
            this.context.SaveChanges();
        }
    }
}
