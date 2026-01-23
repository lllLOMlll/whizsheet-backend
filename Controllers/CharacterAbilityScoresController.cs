using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Dtos.AbilityScores;
using Whizsheet.Api.Infrastructure;


namespace Whizsheet.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/v1/characters/{characterId:int}/ability-scores")]

	public class CharacterAbilityScoresController : ControllerBase
	{
		private readonly WhizsheetDbContext _db;

		public CharacterAbilityScoresController(WhizsheetDbContext db)
		{
			_db = db;
		}

		[HttpPost]
		public async Task<IActionResult> Create(int characterId, CreateAbilityScoresDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				return Unauthorized();
			}

			var character = await _db.Characters
				.Include(c => c.AbilityScores)
				.FirstOrDefaultAsync(c =>
					c.Id == characterId &&
					c.UserId == userId);

			if (character == null)
			{
				return NotFound();
			}

			if (character.AbilityScores != null)
			{
				return Conflict("Ability scores already exist for this character.");
			}

			var abilityScores = new AbilityScores
			{
				Strength = dto.Strength,
				Dexterity = dto.Dexterity,
				Constitution = dto.Constitution,
				Intelligence = dto.Intelligence,
				Wisdom = dto.Wisdom,
				Charisma = dto.Charisma,
				CharacterId = characterId,
			};

			_db.Add(abilityScores);
			await _db.SaveChangesAsync();

			var result = new AbilityScoresDto
			{
				Strength = abilityScores.Strength,
				Dexterity = abilityScores.Dexterity,
				Constitution = abilityScores.Constitution,
				Intelligence= abilityScores.Intelligence,
				Wisdom= abilityScores.Wisdom,	
				Charisma= abilityScores.Charisma,		
			};


			return CreatedAtAction(
				nameof(Get),
				new { characterId = character.Id},
				result
				);
		}

		[HttpGet]
		public async Task<IActionResult> Get(int characterId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				return Unauthorized();
			}

			var abilityScores = await _db.AbilityScores
				.Where(a => 
					a.CharacterId == characterId &&
					a.Character.UserId == userId)
				.Select(a => new AbilityScoresDto
				{
					Strength = a.Strength,
					Dexterity = a.Dexterity,
					Constitution = a.Constitution,
					Intelligence = a.Intelligence,
					Wisdom = a.Wisdom,
					Charisma = a.Charisma
				}
				).FirstOrDefaultAsync();

			if ( abilityScores == null )
			{
				return NotFound();
			}
			
			return Ok(abilityScores);
		}

	}
}
