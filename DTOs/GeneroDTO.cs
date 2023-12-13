using AutoMapper;
using System.ComponentModel.DataAnnotations;
using WebApiPelis2023.Models;

namespace WebApiPelis2023.DTOs
{
	public class GeneroDTO
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El campo {0} es rekerido")]
		[StringLength(maximumLength: 50, ErrorMessage = "El campo {0} no debe de tener más de {1} caracteres")]
		public string Nombre { get; set; }
		//public GeneroPelicula? GeneroPelicula { get; set; } //uno a uno con generoPeli obligatorio
		public List<string> Peliculas { get; set; } = new List<string>();  //muchos a muchos con pelicula
	}
}
