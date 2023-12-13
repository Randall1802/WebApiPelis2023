using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using WebApiPelis2023.DTOs;
using WebApiPelis2023.Models;

namespace WebApiPelis2023.Controllers
{
	[ApiController]
	[Route("api/[controller]")] //por si cambiamos el nombre de la clase
	public class GeneroController : Controller
	{
		private readonly ApplicationDbContext _context;
		public GeneroController(ApplicationDbContext context)
		{
			//con el this hago referencia al de fuera. 
			_context = context;
		}

		//Endpoint dar de alta género
		[HttpPost("RegistrarGenero")] //pa dar de alta
									  //el task es para mandar la info q de verdad necesito mandar. 
									  //async es para que no se quede bloqueada la app. forma de trabajar es asíncrona. mando petición, 
									  //el programa sigue funcionando. el await es el método que mandarás llamar dentro del async.
		public async Task<ActionResult> RegistrarGenero(Genero genero)  //adentro va el modelo. no se debe de exponer. se usa dtos para no exponer. es un patrón de diseño
		{
			//primero validamos si ya existe
			var existeGenero = await _context.Generos.AnyAsync(x => x.Nombre
			== genero.Nombre); //haciendo consulta pero no en T-SQL, sino en lenguaje de EntityFramework
			if (existeGenero)
			{
				return BadRequest($"El genero {genero.Nombre} ya existe");
			}
			_context.Add(genero);
			await _context.SaveChangesAsync(); //si hubo cambios guarda de forma asíncrona.
			return Ok(genero);
		}

		//endpoint para listar todas las categorías
		[HttpGet("ListarGeneros")]
		public async Task<ActionResult<List<GeneroDTO>>> ListarGeneros()
		{
			var generos = await _context.Generos.Include(p=>p.Peliculas).ToListAsync();
			var generosDTO = generos.Select(p => new GeneroDTO 
			{
				Id = p.Id,
				Nombre = p.Nombre,
				Peliculas = p.Peliculas.Select(g => g.Titulo).ToList(),
			}).ToList();
			return Ok(generosDTO);
		}

		//endpoint para buscar la info de un género en específico
		[HttpGet("GeneroEspecifico/{nombre}")] //especifico q al get le debo pasar parámetro
		public async Task<ActionResult<Genero>> GeneroEspecifico(string nombre)
		{
			var genero = await _context.Generos.FirstOrDefaultAsync(x => x.Nombre == nombre);
			if (genero == null)
			{
				return NotFound();
			}
			return Ok(genero);
		}

		[HttpPut("ActualizaGenero/")]  //put actualiza todo
		public async Task<ActionResult<Genero>> ActualizarGenero(string nombreActual, string nombreNuevo)
		{
			var genero = await _context.Generos.FirstOrDefaultAsync(x => x.Nombre == nombreActual);
			if (genero == null)
			{
				return NotFound();
			}
			if (genero.Nombre == nombreNuevo)
			{
				return BadRequest($"El genero {genero.Nombre} tiene el mismo nombre");

			}
			genero.Nombre = nombreNuevo;
			_context.Generos.Update(genero);
			await _context.SaveChangesAsync();
			return Ok(genero);
		}

		//endpoint put del profe pa actualizar todo el género
		[HttpPut("ModificarGenero/{id:int}")]
		public async Task<ActionResult> ModificarGenero(int id, Genero genero)
		{
			//any async pa palabras	
			var existe = await _context.Generos.AnyAsync(x => x.Id == id);
			if (!existe) return NotFound("El género no existe");
			_context.Update(genero);
			await _context.SaveChangesAsync();
			return Ok(genero);
		}
		


		//endpoint del profe pa eliminar un genero
		[HttpDelete("EliminarGenero/{id:int}")]
		public async Task<ActionResult> EliminarCategoria(int id)
		{
			var existe = await _context.Generos.AnyAsync(z => z.Id == id);
			if (!existe) return NotFound();
			_context.Remove(new Genero() { Id = id }); //instancia de género creada con id específico
			await _context.SaveChangesAsync();
			return Ok();
		}

    }
}
