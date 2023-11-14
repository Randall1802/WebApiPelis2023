namespace WebApiPelis2023.Models
{
	public class Opinion
	{
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public int UsuarioId { get; set; }
        public string Comentario { get; set; }
        public double Calificacion { get; set; }
        public DateTime Fecha { get; set; }
        public Pelicula Pelicula { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}
