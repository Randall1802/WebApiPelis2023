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
    }
}
