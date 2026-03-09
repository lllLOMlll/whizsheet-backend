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

			var savingThrowsdDto = new SavingTrowsListDto
			{
				SavingThrowsListDto = character.SavingThrows
					.Select(st => new SavingThrowDto
			{
				SavingThrowType = st.SavingThrowType,
				IsProficient = st.IsProficient,
			}).ToList()
			};


			return Ok(savingThrowsdDto);
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
				Console.WriteLine("dto.SavingThrowType = " + dto.SavingThrowType);
				Console.WriteLine("existingSavingThrow = " + existingtSavingThrow + " = " + "characer.SavingThrows = " + character.SavingThrows);

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
			Console.WriteLine("=== FIRST UPDATE START ===");
			Console.WriteLine($"CharacterId reçu: {characterId}");

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				Console.WriteLine("UserId NULL -> Unauthorized");
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
				Console.WriteLine("Character NOT FOUND");
				return NotFound();
			}

			Console.WriteLine($"Character trouvé: {character.Name}");
			Console.WriteLine($"Nombre de classes: {character.Classes.Count}");
			Console.WriteLine($"Nombre de saving throws: {character.SavingThrows.Count}");

			var mainCharacterClass = character.GetCharacterMainClass();
			Console.WriteLine($"Main class: {mainCharacterClass}");

			foreach (SavingThrow s in character.SavingThrows)
			{
				var isProficient = CharacterClassTypeExtensions
					.IsProficientIn(mainCharacterClass, s.SavingThrowType);

				Console.WriteLine(
					$"SavingThrow: {s.SavingThrowType} | Avant: {s.IsProficient} | Devrait être prof?: {isProficient}"
				);

				if (isProficient)
				{
					s.SetProficiency(true);
					Console.WriteLine($" --> Proficiency SET TRUE pour {s.SavingThrowType}");
				}
			}

			Console.WriteLine("Sauvegarde en base...");
			await _dbContext.SaveChangesAsync();
			Console.WriteLine("SaveChangesAsync terminé.");

			Console.WriteLine("=== FIRST UPDATE END ===");

			return Ok();
		}
	}
}
