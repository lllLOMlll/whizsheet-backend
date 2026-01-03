using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
	}
}
