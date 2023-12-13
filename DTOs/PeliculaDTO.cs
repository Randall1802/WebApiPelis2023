using System.ComponentModel.DataAnnotations;

namespace WebApiPelis2023.DTOs
{
	public class PeliculaDTO
	{
        public int Id { get; set; }
		[Required(ErrorMessage = "El campo {0} es requerido")]
		[StringLength(maximumLength: 50, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
		public string Titulo { get; set; }
		public string Descripcion { get; set; }
		public double Calificacion { get; set; }
		public double Duracion { get; set; }
		public string Imagen { get; set; }
		public List<string> Generos { get; set; } = new List<string>();
		public List<OpinionDTO> Opiniones { get; set; } = new List<OpinionDTO>();

	}
}
