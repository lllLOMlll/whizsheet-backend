using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Infrastructure;

namespace Whizsheet.Api.Controllers
{
	[ApiController]
	[Route("api/characters")]
	public class CharactersController : ControllerBase
	{
		private readonly WhizsheetDbContext _db;

		public CharactersController(WhizsheetDbContext db)
		{
			_db = db;
		}
		

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var characters = await _db.Characters.ToListAsync();
			return Ok(characters);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Character character)
		{
			_db.Characters.Add(character);
			await _db.SaveChangesAsync();

			return CreatedAtAction(
				nameof(GetAll),
				new { id = character.Id },
				character
				);
		}
	}
}
