namespace WebApiPelis2023.Models
{
	public class Opinion
	{
        public int Id { get; set; }
        public int IdPelicula { get; set; }
        public int IdUsuario { get; set; }
        public string Comentario { get; set; }
        public double Calificacion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
