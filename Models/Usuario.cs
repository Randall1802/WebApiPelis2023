using System.ComponentModel.DataAnnotations;

namespace WebApiPelis2023.Models
{
	public class Usuario
	{
        public int Id { get; set; }

		[Required(ErrorMessage = "El campo {0} es rekerido")]
		public string Nombre { get; set; }
        public string Apellido { get; set; }
		public int Edad { get; set; }
		public string Contrasena { get; set;}
		public ICollection<Opinion> Opiniones { get; } = new List<Opinion>(); //uno a muchos con opinion obligatorio
	}
}
