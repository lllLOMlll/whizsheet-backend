using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Domain.Extensions;
using Whizsheet.Api.Dtos.AbilityScores;
using Whizsheet.Api.Dtos.SavingThrows;
using Whizsheet.Api.Enum;
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

		//[HttpPost]
		//public async Task<IActionResult> Create(int characterId, CreateAbilityScoresDto dto)
		//{
		//	var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		//	if (userId == null)
		//		return Unauthorized();

		//	var character = await _db.Characters
		//		.Include(c => c.AbilityScores)
		//		.FirstOrDefaultAsync(c =>
		//			c.Id == characterId &&
		//			c.UserId == userId);

		//	if (character == null)
		//		return NotFound();

		//	if (character.AbilityScores != null)
		//		return Conflict("Ability scores already exist.");

		//	character.CreateAbilityScores(
		//		dto.Strength,
		//		dto.Dexterity,
		//		dto.Constitution,
		//		dto.Intelligence,
		//		dto.Wisdom,
		//		dto.Charisma);

		//	await _db.SaveChangesAsync();

		//	return CreatedAtAction(nameof(Get),
		//		new { characterId },
		//		new AbilityScoresDto
		//		{
		//			Strength = character.AbilityScores!.Strength,
		//			Dexterity = character.AbilityScores.Dexterity,
		//			Constitution = character.AbilityScores.Constitution,
		//			Intelligence = character.AbilityScores.Intelligence,
		//			Wisdom = character.AbilityScores.Wisdom,
		//			Charisma = character.AbilityScores.Charisma
		//		});
		//}

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
				.Include(c => c.Items)
					.ThenInclude(i => i.MagicItem)
						.ThenInclude(m => m.MagicItemEffects)
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

			if (abilityScores == null)
			{
				return NotFound();
			}

			var characterMainClass = character.GetCharacterMainClass();

			// Ability Scores Modifiers
			int strengthModifier = (int)Math.Floor(((character.AbilityScores.Strength + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Strength)) - 10) / 2.0);
			int dexterityModifier = (int)Math.Floor(((character.AbilityScores.Dexterity + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Dexterity)) - 10) / 2.0);
			int constitutionModifier = (int)Math.Floor(((character.AbilityScores.Constitution + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Constitution)) - 10) / 2.0);
			int intelligenceModifer = (int)Math.Floor(((character.AbilityScores.Intelligence + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Intelligence)) - 10) / 2.0);
			int wisdomModifier = (int)Math.Floor(((character.AbilityScores.Wisdom + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Wisdom)) - 10) / 2.0);
			int charismaModifier = (int)Math.Floor(((character.AbilityScores.Charisma + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Charisma)) - 10) / 2.0);

			var response = new AbilityUpdateResponseDto
			{
				Abilities = new UpdateAbilityScoresDto
				{
					Strength = character.AbilityScores.Strength + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Strength),
					StrengthModifier = strengthModifier,
					Dexterity = character.AbilityScores.Dexterity + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Dexterity),
					DexterityModifier = dexterityModifier,
					Constitution = character.AbilityScores.Constitution + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Constitution),
					ConstitutionModifier = constitutionModifier,
					Intelligence = character.AbilityScores.Intelligence + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Intelligence),
					IntelligenceModifer = intelligenceModifer,
					Wisdom = character.AbilityScores.Wisdom + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Wisdom),
					WisdomModifier = wisdomModifier,
					Charisma = character.AbilityScores.Charisma + character.GetMagicItemAbilityScoreBonus(AbilityScoreType.Charisma),
					CharismaModifier = charismaModifier,
				},

				SavingThrows = new SavingThrowsDto
				{
					Strength = strengthModifier + character.GetMagicItemSavingThrowBonus(SavingThrowType.Strength),
					IsStrengthProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Strength),
					Dexterity = dexterityModifier + character.GetMagicItemSavingThrowBonus(SavingThrowType.Dexterity),
					IsDexterityProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Dexterity),
					Constitution = constitutionModifier + character.GetMagicItemSavingThrowBonus(SavingThrowType.Constitution),
					IsConstitutionProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Constitution),
					Intelligence = intelligenceModifer + character.GetMagicItemSavingThrowBonus(SavingThrowType.Intelligence),
					IsIntelligenceProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Intelligence),
					Wisdom = wisdomModifier + character.GetMagicItemSavingThrowBonus(SavingThrowType.Wisdom),
					IsWisdomProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Wisdom),
					Charisma = charismaModifier + character.GetMagicItemSavingThrowBonus(SavingThrowType.Charisma),
					IsCharismaProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Charisma)
				},

				Skills = character.ToSkillsDto().Skills

			};
			

			return Ok(response);
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
				.Include(c => c.Classes)
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

			var characterMainClass = character.GetCharacterMainClass();

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
					IsStrengthProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Strength),
					Dexterity = character.AbilityScores.DexteritySavingThrowsModifier,
					IsDexterityProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Dexterity),
					Constitution = character.AbilityScores.ConstitutionSavingThrowsModifier,
					IsConstitutionProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Constitution),
					Intelligence = character.AbilityScores.IntelligenceSavingThrowsModifier,
					IsIntelligenceProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Intelligence),
					Wisdom = character.AbilityScores.WisdomSavingThrowsModifier,
					IsWisdomProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Wisdom),
					Charisma = character.AbilityScores.CharismaSavingThrowsModifier,
					IsCharismaProficient = CharacterClassTypeExtensions.IsProficientIn(characterMainClass, SavingThrowType.Charisma)
				},

				Skills = character.ToSkillsDto().Skills

			}; 

			return Ok(response);
		}
		



	}
}
