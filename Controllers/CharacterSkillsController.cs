using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Whizsheet.Api.Infrastructure;
using Whizsheet.Api.Dtos.Skills;

namespace Whizsheet.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/v1/characters/${characterId:int}/skills")]
	public class CharacterSkillsController : ControllerBase
	{
		private readonly WhizsheetDbContext _dbContext;
		public  CharacterSkillsController(WhizsheetDbContext db) 
		{
			_dbContext = db;
		}

		[HttpPost]
		public async Task<IActionResult> Create(int characterId)
		{
			
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
				return Unauthorized();

			var character = await _dbContext.Characters
				.Include(c => c.Skills)
				.FirstOrDefaultAsync(c =>
					c.UserId == userId &&
					c.Id == characterId
				);

			if (character == null)
				return NotFound("Character not found");


			character.CreateSkills();

			await _dbContext.SaveChangesAsync();

			var skillsDto = new SkillsDto()
			{
				isProficientAcrobatics = character.Skills.isProficientAcrobatics,
				isProficientAnimalHandling = character.Skills.isProficientAnimalHandling,
				isProficientArcana = character.Skills.isProficientArcana,
				isProficientAthletics = character.Skills.isProficientAthletics,
				isProficientDeception = character.Skills.isProficientDeception,
				isProficientHistory = character.Skills.isProficientHistory,
				isProficientInsight = character.Skills.isProficientInsight,
				isProficientIntimidation = character.Skills.isProficientIntimidation,
				isProficientInvestigation = character.Skills.isProficientInvestigation,
				isProficientMedecine = character.Skills.isProficientMedecine,
				isProficientNature = character.Skills.isProficientNature,
				isProficientPerception = character.Skills.isProficientPerception,
				isProficientPerformance = character.Skills.isProficientPerformance,
				isProficientPersuasion = character.Skills.isProficientPersuasion,
				isProficientReligion = character.Skills.isProficientReligion,
				isProficientSleighOfHand = character.Skills.isProficientSleighOfHand,
				isProficientStealth = character.Skills.isProficientStealth,
				isProficientSurvival = character.Skills.isProficientSurvival
			};

			
			return CreatedAtAction(
				nameof(Get),
				new { characterId },
				skillsDto
				);
		}

		[HttpGet]
		public async Task<IActionResult> Get(int characterId)
		{

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
				return Unauthorized();

			var character = await _dbContext.Characters
				.Include(c => c.Skills)
				.FirstOrDefaultAsync(c =>
					c.Id == characterId &&
					c.UserId == userId
				);

			if (character == null)
				return NotFound("Character not found");

			var skillsDto = new SkillsDto()
			{
				isProficientAcrobatics = character.Skills.isProficientAcrobatics,
				isProficientAnimalHandling = character.Skills.isProficientAnimalHandling,
				isProficientArcana = character.Skills.isProficientArcana,
				isProficientAthletics = character.Skills.isProficientAthletics,
				isProficientDeception = character.Skills.isProficientDeception,
				isProficientHistory = character.Skills.isProficientHistory,
				isProficientInsight = character.Skills.isProficientInsight,
				isProficientIntimidation = character.Skills.isProficientIntimidation,
				isProficientInvestigation = character.Skills.isProficientInvestigation,
				isProficientMedecine = character.Skills.isProficientMedecine,
				isProficientNature = character.Skills.isProficientNature,
				isProficientPerception = character.Skills.isProficientPerception,
				isProficientPerformance = character.Skills.isProficientPerformance,
				isProficientPersuasion = character.Skills.isProficientPersuasion,
				isProficientReligion = character.Skills.isProficientReligion,
				isProficientSleighOfHand = character.Skills.isProficientSleighOfHand,
				isProficientStealth = character.Skills.isProficientStealth,
				isProficientSurvival = character.Skills.isProficientSurvival
			};
				
			

			return Ok(skillsDto);
		}
	}
}
