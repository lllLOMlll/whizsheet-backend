using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using System.Reflection.Metadata.Ecma335;
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

		[HttpPut]
		public async Task<IActionResult> Update(
			int characterId,
			[FromBody] CreateAbilityScoresDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null) return Unauthorized();

			var character = await _db.Characters
				.Include(c => c.AbilityScores)
				.FirstOrDefaultAsync(c => c.Id == characterId && c.UserId == userId);

			if (character == null) return NotFound("Character not found");


			character.AbilityScores.Strength = dto.Strength;
			character.AbilityScores.Dexterity = dto.Dexterity;
			character.AbilityScores.Constitution = dto.Constitution;
			character.AbilityScores.Intelligence = dto.Intelligence;
			character.AbilityScores.Wisdom = dto.Wisdom;
			character.AbilityScores.Charisma = dto.Charisma;

			await _db.SaveChangesAsync();

			var updateDto = new UpdateAbilityScoresDto
			{
				Strength = character.AbilityScores.Strength,
				StrengthModifier = character.AbilityScores.StrengthModifier,
				Dexterity = character.AbilityScores.Dexterity,
				DexterityModifier = character.AbilityScores.DexterityModifier,
				Constitution = character.AbilityScores.Constitution,
				ConstitutionModifier = character.AbilityScores.ConstitutionModifier,
				Intelligence = character.AbilityScores.Intelligence,
				IntelligenceModifer = character.AbilityScores.IntelligenceModifer,
				Wisdom = character.AbilityScores.Wisdom,
				WisdomModifier = character.AbilityScores.WisdomModifier,
				Charisma = character.AbilityScores.Charisma,
				CharismaModifier = character.AbilityScores.CharismaModifier,
			};

			return Ok(updateDto);
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
