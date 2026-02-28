using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Dtos.CharacterClasses;
using Whizsheet.Api.Enum;
using Whizsheet.Api.Infrastructure;

namespace Whizsheet.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/v1/characters/{characterId:int}/classes")]
	public class CharacterClassesController : ControllerBase
	{
		private readonly WhizsheetDbContext _dbContext;

		public CharacterClassesController(WhizsheetDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		//[HttpPost]
		//public async Task<IActionResult> Create(
		//	int characterId, 
		//	[FromBody] List<CreateCharacterClassDto> dtos)
		//{
		//	var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

		//	if (userId == null)
		//	{
		//		return Unauthorized();
		//	}

		//	var character = await _dbContext.Characters
		//		.Include(c => c.Classes)
		//		.FirstOrDefaultAsync(c =>
		//			c.Id == characterId &&
		//			c.UserId == userId);

		//	if (character == null)
		//	{
		//		return NotFound();
		//	}

		//	if (character.Classes.Any())
		//	{
		//		return Conflict("Classes already existe for this chraracter.");
		//	}

		//	var newClasses = new List<CharacterClass>();


		//	int totalLevel = 0;
		//	foreach (var dto in dtos)
		//	{
		//		totalLevel += dto.Level;

		//		if (totalLevel > Character.MaxTotalLevel)
		//		{
		//			return BadRequest($"The sum of all levels must be {Character.MaxTotalLevel} or under");
		//		}
				
		//		if (dto.ClassType == Enum.CharacterClassType.Other &&
		//			string.IsNullOrWhiteSpace(dto.CustomClassName))
		//		{
		//			return BadRequest("CustomClassName is required when ClassType is Other.");
		//		}

		//		if (dto.ClassType != Enum.CharacterClassType.Other &&
		//			!string.IsNullOrWhiteSpace(dto.CustomClassName))
		//		{
		//			return BadRequest("CustomClassName must be empty for official classes.");
		//		}
		
		//		newClasses.Add(new CharacterClass
		//		{
		//			ClassType = dto.ClassType,
		//			CustomClassName = dto.ClassType == CharacterClassType.Other
		//				? dto.CustomClassName!.Trim()
		//				: null,
		//			Level = dto.Level,
		//			CharacterId = character.Id
		//		});	
		//	}
			
		//	_dbContext.CharacterClasses.AddRange(newClasses);
		//	await _dbContext.SaveChangesAsync();

		//	var resultDto = newClasses.Select(cc => new CharacterClassDto
		//	{
		//		ClassType = cc.ClassType,
		//		Level = cc.Level,
		//		CustomClassName = cc.CustomClassName,
		//	}).ToList();



		//	return CreatedAtAction(
		//		nameof(GetAll),
		//		new { characterId },
		//		resultDto
		//	);

		//}

		[HttpPut]
		public async Task<IActionResult> Update(
			int characterId,
			[FromBody] List<CreateCharacterClassDto> dtos)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null) return Unauthorized();

			var character = await _dbContext.Characters
				.Include(c => c.Classes)
				.FirstOrDefaultAsync(c => c.Id == characterId && c.UserId == userId);

			if (character == null) return NotFound();

			int totalLevel = 0;
			foreach (var dto in dtos)
			{
				totalLevel += dto.Level;
				if (totalLevel > Character.MaxTotalLevel)
					return BadRequest($"The sum of all levels must be {Character.MaxTotalLevel} or under");

				if (dto.ClassType == CharacterClassType.Other && string.IsNullOrWhiteSpace(dto.CustomClassName))
					return BadRequest("CustomClassName is required when ClassType is Other.");
			}

			_dbContext.CharacterClasses.RemoveRange(character.Classes);

			var newClasses = dtos.Select(dto => new CharacterClass(
				characterId,
				dto.ClassType,				
				dto.Level,
				dto.CustomClassName
				)
			{
				CharacterId = characterId
			}).ToList();

			_dbContext.CharacterClasses.AddRange(newClasses);

			await _dbContext.SaveChangesAsync();


			var resultDto = newClasses.Select(cc => new CharacterClassDto
			{
				Id = cc.Id,
				ClassType = cc.ClassType,
				Level = cc.Level,
				DisplayName = cc.DisplayName,
				CustomClassName = cc.CustomClassName
			}).ToList();

			return Ok(resultDto);
		}

		[HttpGet]
		public async Task<IActionResult> GetAll(int characterId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				return Unauthorized();
			}

		var classes = await _dbContext.Characters
			.Where(c => c.Id == characterId && c.UserId == userId)
			.SelectMany(c => c.Classes)
			.Select(cc => new CharacterClassDto
			{
				Id = cc.Id,
				ClassType = cc.ClassType,
				CustomClassName = cc.CustomClassName,
				Level = cc.Level,
				DisplayName = cc.DisplayName
			})
			.ToListAsync();

			return Ok(classes);
		}



	}
}
