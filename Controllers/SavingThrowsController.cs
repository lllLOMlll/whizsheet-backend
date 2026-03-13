using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Claims;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Domain.Extensions;
using Whizsheet.Api.Dtos.SavingThrows;
using Whizsheet.Api.Enum;
using Whizsheet.Api.Infrastructure;
using Whizsheet.Api.Migrations;

namespace Whizsheet.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/v1/characters/{characterId:int}/saving-throws")]
	public class SavingThrowsController : ControllerBase
	{
		private readonly WhizsheetDbContext _dbContext;

		public SavingThrowsController(WhizsheetDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<IActionResult> Get(int characterId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
				return Unauthorized();

			var character = await _dbContext.Characters
				.Include(c => c.SavingThrows)
				.Include(c => c.AbilityScores)
				.Include(c => c.Classes)
				.FirstOrDefaultAsync(c =>
					c.UserId == userId &&
					c.Id == characterId);

			if (character == null)
				return NotFound();

			var savingThrowsDto = new SavingTrowsListDto
			{

				SavingThrowsListDto = character.SavingThrows
				.Select(st => new SavingThrowDto
				{
					SavingThrowType = st.SavingThrowType,
					IsProficient = st.IsProficient,
					Modifier = character.getSavingThrowScore(st.SavingThrowType),
				}).ToList()
			};


			return Ok(savingThrowsDto);
		}

		
		[HttpPut]
		public async Task<IActionResult> Put(
			int characterId,
			[FromBody] SavingTrowsProficientListDto updateDto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null) 
				return Unauthorized();

			var character = await _dbContext.Characters
				.Include(c => c.SavingThrows)
				.Include(c => c.AbilityScores)
				.Include(c => c.Classes)
				.FirstOrDefaultAsync(c =>
					c.UserId == userId &&
					c.Id == characterId);
				
			if (character == null)
				return NotFound();

			foreach (var dto in updateDto.SavingThrows)
			{
				var existingtSavingThrow = character.SavingThrows
					.FirstOrDefault(c => c.SavingThrowType == dto.SavingThrowType);

				if (existingtSavingThrow != null)
				{
					existingtSavingThrow.SetProficiency(dto.IsProficient);
					
				}

			}

			await _dbContext.SaveChangesAsync();

			var updatedDto = new SavingTrowsListDto
			{

				SavingThrowsListDto = character.SavingThrows
					.Select(st => new SavingThrowDto
					{
						SavingThrowType = st.SavingThrowType,
						IsProficient = st.IsProficient,
						Modifier = character.getSavingThrowScore(st.SavingThrowType),
					}).ToList()
			};

			return Ok(updatedDto);
		}

		[HttpPut("first-update")]
		public async Task<IActionResult> FirstUpdate(int characterId)
		{
			
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{		
				return Unauthorized();
			}

			Console.WriteLine($"UserId: {userId}");

			var character = await _dbContext.Characters
				.Include(c => c.SavingThrows)
				.Include(c => c.AbilityScores)
				.Include(c => c.Classes)
				.FirstOrDefaultAsync(c =>
					c.UserId == userId &&
					c.Id == characterId);

			if (character == null)
			{
				return NotFound();
			}

			var mainCharacterClass = character.GetCharacterMainClass();
		

			foreach (SavingThrow s in character.SavingThrows)
			{
				var isProficient = CharacterClassTypeExtensions
					.IsProficientIn(mainCharacterClass, s.SavingThrowType);
	
				if (isProficient)
				{
					s.SetProficiency(true);
				}
			}

			await _dbContext.SaveChangesAsync();
	
			return Ok();
		}
	}
}
