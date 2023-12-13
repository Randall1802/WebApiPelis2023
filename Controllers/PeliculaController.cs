using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPelis2023.DTOs;
using WebApiPelis2023.Models;
using System.Data;

namespace WebApiPelis2023.Controllers
{
	[ApiController]
	[Route("api/[controller]")] //por si cambiamos el nombre de la clase
	public class PeliculaController : Controller
    
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

        public PeliculaController(ApplicationDbContext context, IMapper mapper)
        {
			_context = context;
            _mapper = mapper;
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

        //con automapper
        [HttpPost("RegistrarPeliMapper")]
        public async Task<ActionResult> RegistrarPeliMapper(PeliculaDTO peliculaDTO)
        {
            var existePelicula = await _context.Peliculas.AsNoTracking().ProjectTo<PeliculaDTO>
				(_mapper.ConfigurationProvider).AnyAsync(x => x.Titulo== peliculaDTO.Titulo);
           if (existePelicula)
            {
                return BadRequest($"El genero {peliculaDTO.Titulo} ya existe");
            }
            _context.Add(peliculaDTO);
            await _context.SaveChangesAsync(); //si hubo cambios guarda de forma asíncrona.
            return Ok(peliculaDTO);
        }

        [HttpGet("ListarPeliculas")]
		public async Task<ActionResult<List<PeliculaDTO>>> ListarPeliculas()
		{
			var peliculas = await _context.Peliculas.Include(p=>p.Generos).ToListAsync();
			//return Ok(peliculas);
			var peliculasDTO = peliculas.Select(p=>new PeliculaDTO
			{
				Id=p.Id,
				Titulo = p.Titulo,
				Descripcion = p.Descripcion,
				Calificacion = p.Calificacion,
				Duracion = p.Duracion,
				Imagen = p.Imagen,
				Generos = p.Generos.Select(g => g.Nombre).ToList(), // Obtener solo los nombres de los géneros
				Opiniones = p.Opiniones.Select(o => new OpinionDTO
				{
					// opinionDTOPropiedad = o.Propiedad
				}).ToList()
			}).ToList();
			return Ok(peliculasDTO);
		}

		//mismo método de arriba pero con Mapper
		[HttpGet("ListaPelisMapper")]
		public async Task<ActionResult<List<PeliculaDTO>>> ListaPelisMapper() =>
			await _context.Peliculas
			.AsNoTracking()
			.ProjectTo<PeliculaDTO>(_mapper.ConfigurationProvider)
			.ToListAsync();

		[HttpGet("PeliculaEspecifica/{titulo}")]
		public async Task <ActionResult<Pelicula>> PeliculaEspecifica(string titulo)
		{
			var pelicula = await _context.Peliculas.FirstOrDefaultAsync(s=>s.Titulo==titulo);
			if (pelicula == null) return NotFound();
			return Ok(pelicula);
		}

		//con automapper
        [HttpGet("PeliEspecificaMapper/{titulo}")]
        public async Task<ActionResult<PeliculaDTO>> PeliculaEspecificaMapper(string titulo)
        {
			var pelicula = await _context.Peliculas.AsNoTracking()
				.ProjectTo<PeliculaDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(s=>s.Titulo==titulo);
            if (pelicula == null) return NotFound();
            return Ok(pelicula);
        }

        [HttpPut("ModificarPelicula/{id:int}")]
		public async Task<ActionResult> ModificarPelicula(int id, Pelicula pelicula)
		{
			//any async pa palabras	
			var existe = await _context.Peliculas.AnyAsync(x => x.Id == id);
			if (!existe) return NotFound("La peli no existe");
			_context.Update(pelicula);
			await _context.SaveChangesAsync();
			return Ok(pelicula);
		}

		[HttpDelete("EliminarPelicula/{id:int}")]
		public async Task<ActionResult> EliminarPelicula(int id)
		{
			var existe = await _context.Peliculas.AnyAsync(z => z.Id == id);
			if (!existe) return NotFound();
			_context.Remove(new Pelicula() { Id = id }); //instancia de película creada con id específico
			await _context.SaveChangesAsync();
			return Ok();
		}

		//agregar género a pelicula, mediante sus ids
		[HttpPost("AgregarGeneroAPelicula/{idPelicula}/{idGenero}")]
		public async Task <ActionResult> AgregarGeneroAPelicula(int idPelicula, int idGenero)
		{
			var pelicula = await _context.Peliculas.FindAsync(idPelicula);
			if (pelicula == null)
			{
				return NotFound(); // Si la película no existe, retorna un error 404
			}

			var genero = await _context.Generos.FindAsync(idGenero);
			if (genero == null)
			{
				return NotFound(); 
			}

			pelicula.Generos.Add(genero);

			await _context.SaveChangesAsync();
			return Ok();
		}

	}
}
