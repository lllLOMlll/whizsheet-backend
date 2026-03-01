using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Whizsheet.Api.Infrastructure;
using Whizsheet.Api.Dtos.SavingThrows;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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
				.FirstOrDefaultAsync(c =>
					c.UserId == userId &&
					c.Id == characterId);
				
			if (character == null)
				return NotFound();

			foreach (var dto in updateDto.SavingThrows)
			{
				var exisintSavingThrow = character.SavingThrows
					.FirstOrDefault(c => c.SavingThrowType == dto.SavingThrowType);

				if (exisintSavingThrow != null)
				{
					exisintSavingThrow.SetProficiency(dto.IsProficient);
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
					}).ToList()
			};

			return Ok(updatedDto);
		}
	}
}
