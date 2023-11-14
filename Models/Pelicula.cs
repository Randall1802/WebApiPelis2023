using System.ComponentModel.DataAnnotations;

namespace WebApiPelis2023.Models
{
	public class Pelicula
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es rekerido")]
        [StringLength(maximumLength:50, ErrorMessage = "El campo {0} no debe de tener más de {1} caracteres")]
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public double Calificacion { get; set; }
        public double Duracion { get; set; }
        public string Imagen { get; set; }
        //public GeneroPelicula? GeneroPelicula { get; set; }  //uno a uno con generoPeli obligatorio
        public List<Genero> Generos { get; } = new();  //muchos a muchos con generos
		public ICollection<Opinion> Opiniones { get; } = new List<Opinion>(); //uno a varios con opinion oblígatorio
	}
}
