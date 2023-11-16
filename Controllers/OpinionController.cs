using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPelis2023.Models;

namespace WebApiPelis2023.Controllers
{
	public class OpinionController : Controller
	{
		private readonly ApplicationDbContext _context;

        public OpinionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("NuevaOpinion")]
        public async Task <ActionResult> NuevaOpinion(Opinion opinion)
        {
			_context.Add(opinion);
			await _context.SaveChangesAsync(); //si hubo cambios guarda de forma asíncrona.
			return Ok(opinion);
		}

        [HttpGet("ListarOpiniones")]
        public async Task <ActionResult<Opinion>> ListarOpiniones()
        {
			var opiniones = await _context.Opiniones.ToListAsync();
			return Ok(opiniones);
		}

		[HttpPut("ModificarOpinion/{id:int}")]
		public async Task<ActionResult> ModificarOpinion(int id, Opinion opinion)
		{
			//any async pa palabras	
			var existe = await _context.Opiniones.AnyAsync(x => x.Id == id);
			if (!existe) return NotFound("La opinión no existe");
			_context.Update(opinion);
			await _context.SaveChangesAsync();
			return Ok(opinion);
		}

		[HttpDelete("EliminarOpinion/{id:int}")]
		public async Task<ActionResult> EliminarOpinion(int id)
		{
			var existe = await _context.Opiniones.AnyAsync(z => z.Id == id);
			if (!existe) return NotFound();
			_context.Remove(new Opinion() { Id = id }); //instancia de género creada con id específico
			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}
