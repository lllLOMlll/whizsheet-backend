using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Whizsheet.Api.Infrastructure;
using Whizsheet.Api.Dtos.Skills;
using Whizsheet.Api.Domain;
using Microsoft.JSInterop.Infrastructure;

namespace Whizsheet.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/v1/characters/{characterId:int}/skills")]
	public class CharacterSkillsController : ControllerBase
	{
		private readonly WhizsheetDbContext _dbContext;

		public CharacterSkillsController(WhizsheetDbContext db)
		{
			_dbContext = db;
		}

		//[HttpPost]
		//public async Task<IActionResult> Create(int characterId)
		//{
		//	var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

		//	if (userId == null)
		//		return Unauthorized();

		//	var character = await _dbContext.Characters
		//		.Include(c => c.Skills)
		//		.Include(c => c.AbilityScores)
		//		.FirstOrDefaultAsync(c =>
		//			c.Id == characterId &&
		//			c.UserId == userId
		//		);

		//	if (character == null)
		//		return NotFound("Character not found");

		//	if (character.Skills.Any())
		//		return BadRequest("Skills already created.");

		//	character.CreateSkills();

		//	await _dbContext.SaveChangesAsync();

		//	return CreatedAtAction(
		//		nameof(Get),
		//		new { characterId },
		//		(character.ToSkillsDto())
		//	);
		//}

		[HttpGet]
		public async Task<IActionResult> Get(int characterId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
				return Unauthorized();

			var character = await _dbContext.Characters
				.Include(c => c.Skills)
				.Include(c => c.AbilityScores)
				.FirstOrDefaultAsync(c =>
					c.Id == characterId &&
					c.UserId == userId
				);

			if (character == null)
				return NotFound("Character not found");

			if (!character.Skills.Any())
				return NotFound("Skills not created.");

			return Ok(character.ToSkillsDto());
		}

		[HttpPut]
		public async Task<IActionResult> Put(
			int characterId,
			[FromBody] SkillsDtoWithModifiers dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
				return Unauthorized();

			var character = await _dbContext.Characters
				.Include(c => c.Skills)
				.Include (c => c.AbilityScores)
				.FirstOrDefaultAsync(c =>
					c.Id == characterId && 
					c.UserId == userId);

			if (character == null)
				return NotFound("Character not found");

			if (character.Skills == null)
				throw new InvalidOperationException("Character is in a invalid state: Skills are missing");

			foreach (var skillDto in dto.Skills)
			{
				var existingSkill = character.Skills.
					FirstOrDefault(s => s.Type == skillDto.Type);

				if (existingSkill != null)
				{
					existingSkill.SetProficiency(skillDto.IsProficient);										
				}
			}

			await _dbContext.SaveChangesAsync();


			return Ok(character.ToSkillsDto());
		}
		
	}
}
