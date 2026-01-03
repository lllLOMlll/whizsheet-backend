using Microsoft.AspNetCore.Mvc;
using Whizsheet.Api.Domain;

namespace Whizsheet.Api.Controllers
{
	[ApiController]
	[Route("api/characters")]
	public class CharactersController : ControllerBase
	{
		private static readonly List<Character> Characters =
			[
			new Character { Id = 1, Name = "Aragorn", Class = "Ranger", Hp = 35 },
			new Character { Id = 2, Name = "Legolas", Class = "Archer", Hp = 27 }
			];

		[HttpGet]
		public IActionResult GetAll()
		{
			return Ok(Characters);
		}
	}
}
