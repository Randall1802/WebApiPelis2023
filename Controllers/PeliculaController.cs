using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPelis2023.Models;

namespace WebApiPelis2023.Controllers
{
	[ApiController]
	[Route("api/[controller]")] //por si cambiamos el nombre de la clase
	public class PeliculaController : Controller
	{
		private readonly ApplicationDbContext _context;

        public PeliculaController(ApplicationDbContext context)
        {
			_context = context;
		}

		[HttpPost("RegistrarPelicula")]
		public async Task <ActionResult> RegistrarPelicula(Pelicula pelicula)
		{
			var existePelicula = await _context.Peliculas.AnyAsync(x => x.Titulo
			== pelicula.Titulo); //haciendo consulta pero no en T-SQL, sino en lenguaje de EntityFramework
			if (existePelicula)
			{
				return BadRequest($"El genero {pelicula.Titulo} ya existe");
			}
			_context.Add(pelicula);
			await _context.SaveChangesAsync(); //si hubo cambios guarda de forma asíncrona.
			return Ok(pelicula);
		}

		[HttpGet("ListarPeliculas")]
		public async Task<ActionResult<List<Pelicula>>> ListarPeliculas()
		{
			var peliculas = await _context.Peliculas.ToListAsync();
			return Ok(peliculas);
		}

		[HttpPut("ModificarPelicula/{id:int}")]
		public async Task<ActionResult> ModificarPelicula(int id, Pelicula pelicula)
		{
			//any async pa palabras	
			var existe = await _context.Peliculas.AnyAsync(x => x.Id == id);
			if (!existe) return NotFound("El género no existe");
			_context.Update(pelicula);
			await _context.SaveChangesAsync();
			return Ok(pelicula);
		}

		[HttpDelete("EliminarPelicula/{id:int}")]
		public async Task<ActionResult> EliminarPelicula(int id)
		{
			var existe = await _context.Peliculas.AnyAsync(z => z.Id == id);
			if (!existe) return NotFound();
			_context.Remove(new Pelicula() { Id = id }); //instancia de género creada con id específico
			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}
