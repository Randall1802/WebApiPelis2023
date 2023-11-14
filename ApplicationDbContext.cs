using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiPelis2023.Models;

namespace WebApiPelis2023
{
	public class ApplicationDbContext : IdentityDbContext
	{
		//se manejará todo el orm
		//mapear mis modelos a mis entidades de bd, establecer las entidades en la bd (cuáles), fk, etc...
		public ApplicationDbContext(DbContextOptions options) : base(options) 
		{ 
		}
		//para decirle a dbcontext que estas serán las tablas
        public DbSet<Pelicula> Peliculas { get; set; }
		public DbSet<Genero> Generos { get; set; }
		public DbSet<GeneroPelicula> GeneroPeliculas { get; set; }
		public DbSet<Opinion> Opiniones { get; set; }
		public DbSet<Usuario> Usuarios { get; set; }


	}
}
