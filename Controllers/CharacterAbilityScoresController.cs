using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Dtos.AbilityScores;
using Whizsheet.Api.Dtos.SavingThrows;
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
				return Unauthorized();

			var character = await _db.Characters
				.Include(c => c.AbilityScores)
				.FirstOrDefaultAsync(c =>
					c.Id == characterId &&
					c.UserId == userId);

			if (character == null)
				return NotFound();

			if (character.AbilityScores != null)
				return Conflict("Ability scores already exist.");

			character.CreateAbilityScores(
				dto.Strength,
				dto.Dexterity,
				dto.Constitution,
				dto.Intelligence,
				dto.Wisdom,
				dto.Charisma);

			await _db.SaveChangesAsync();

			return CreatedAtAction(nameof(Get),
				new { characterId },
				new AbilityScoresDto
				{
					Strength = character.AbilityScores!.Strength,
					Dexterity = character.AbilityScores.Dexterity,
					Constitution = character.AbilityScores.Constitution,
					Intelligence = character.AbilityScores.Intelligence,
					Wisdom = character.AbilityScores.Wisdom,
					Charisma = character.AbilityScores.Charisma
				});
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
				.Include(c => c.Skills)
				.FirstOrDefaultAsync(c => c.Id == characterId && c.UserId == userId);

			if (character == null) return NotFound("Character not found");

			if (character.AbilityScores == null)
			{
				return NotFound("Ability scores not found for this character");
			}
		
			character.AbilityScores.Strength = dto.Strength;
			character.AbilityScores.Dexterity = dto.Dexterity;
			character.AbilityScores.Constitution = dto.Constitution;
			character.AbilityScores.Intelligence = dto.Intelligence;
			character.AbilityScores.Wisdom = dto.Wisdom;
			character.AbilityScores.Charisma = dto.Charisma;

			await _db.SaveChangesAsync();

			var response = new AbilityUpdateResponseDto
			{
				Abilities = new UpdateAbilityScoresDto
				{
					Strength = character.AbilityScores.Strength,
					StrengthModifier = character.AbilityScores.StrengthModifier,
					Dexterity = character.AbilityScores.Dexterity,
					DexterityModifier = character.AbilityScores.DexterityModifier,
					Constitution = character.AbilityScores.Constitution,
					ConstitutionModifier = character.AbilityScores.ConstitutionModifier,
					Intelligence = character.AbilityScores.Intelligence,
					IntelligenceModifer = character.AbilityScores.IntelligenceModifier,
					Wisdom = character.AbilityScores.Wisdom,
					WisdomModifier = character.AbilityScores.WisdomModifier,
					Charisma = character.AbilityScores.Charisma,
					CharismaModifier = character.AbilityScores.CharismaModifier,
				},

				SavingThrows = new SavingThrowsDto
				{
					Strength = character.AbilityScores.StrengthSavingThrowsModifier,
					Dexterity = character.AbilityScores.DexteritySavingThrowsModifier,
					Constitution = character.AbilityScores.ConstitutionSavingThrowsModifier,
					Intelligence = character.AbilityScores.IntelligenceSavingThrowsModifier,
					Wisdom = character.AbilityScores.WisdomSavingThrowsModifier,
					Charisma = character.AbilityScores.CharismaSavingThrowsModifier
				},

				Skills = character.ToSkillsDto().Skills

			};

			return Ok(response);
		}


		[HttpGet]
		public async Task<IActionResult> Get(int characterId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				return Unauthorized();
			}

			var character = await _db.Characters
				.Include(c => c.AbilityScores)
				.Include(c => c.Skills)
				.Include(c => c.Classes)
				.FirstOrDefaultAsync(c => c.Id == characterId && c.UserId == userId);

			if (character == null) 
				return NotFound("Character not found");

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

			var response = new AbilityUpdateResponseDto
			{
				Abilities = new UpdateAbilityScoresDto
				{
					Strength = character.AbilityScores.Strength,
					StrengthModifier = character.AbilityScores.StrengthModifier,
					Dexterity = character.AbilityScores.Dexterity,
					DexterityModifier = character.AbilityScores.DexterityModifier,
					Constitution = character.AbilityScores.Constitution,
					ConstitutionModifier = character.AbilityScores.ConstitutionModifier,
					Intelligence = character.AbilityScores.Intelligence,
					IntelligenceModifer = character.AbilityScores.IntelligenceModifier,
					Wisdom = character.AbilityScores.Wisdom,
					WisdomModifier = character.AbilityScores.WisdomModifier,
					Charisma = character.AbilityScores.Charisma,
					CharismaModifier = character.AbilityScores.CharismaModifier,
				},

				SavingThrows = new SavingThrowsDto
				{
					Strength = character.AbilityScores.StrengthSavingThrowsModifier,
					Dexterity = character.AbilityScores.DexteritySavingThrowsModifier,
					Constitution = character.AbilityScores.ConstitutionSavingThrowsModifier,
					Intelligence = character.AbilityScores.IntelligenceSavingThrowsModifier,
					Wisdom = character.AbilityScores.WisdomSavingThrowsModifier,
					Charisma = character.AbilityScores.CharismaSavingThrowsModifier
				},

				Skills = character.ToSkillsDto().Skills

			};


			return Ok(response);
		}

	}
}
