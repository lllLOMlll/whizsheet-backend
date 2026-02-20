using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Security.Claims;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Dtos.HitPoints;
using Whizsheet.Api.Infrastructure;


namespace Whizsheet.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/v1/characters/{characterId:int}/hit-points")]
	public class HitPointsController : ControllerBase
	{
		private readonly WhizsheetDbContext _dbContext;


		public HitPointsController(WhizsheetDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpPost]
		public async Task<IActionResult> Create(int characterId, CreateHitPointsDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				return Unauthorized();
			}

			var character = await _dbContext.Characters
				.Include(c => c.HitPoints)
				.FirstOrDefaultAsync(c => 
					c.Id == characterId && 
					c.UserId == userId);

			if (character == null)
			{
				return NotFound();
			}

			if (character.HitPoints != null)
			{
				return Conflict("HitPoints already exist.");
			}

			character.CreateHitPoints(dto.TotalHitPoints);

			await _dbContext.SaveChangesAsync();	

			var result = new HitPointsDto
			{
				TotalHitPoints = character.HitPoints!.TotalHitPoints,
				CurrentHitPoints = character.HitPoints.CurrentHitPoints,
				TemporaryHitPoints = character.HitPoints.TemporaryHitPoints
			};
	
			return CreatedAtAction(
				nameof(Get),
				new { characterId },
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

			var character = await _dbContext.Characters
				.Include(c => c.HitPoints)
				.FirstOrDefaultAsync(c =>
					c.Id == characterId &&
					c.UserId == userId);

			if (character == null)
			{
				return NotFound(); 
			}

			var characterHitPoints = new HitPointsDto
			{
				TotalHitPoints = character.HitPoints.TotalHitPoints,
				CurrentHitPoints = character.HitPoints.CurrentHitPoints,
				TemporaryHitPoints = character.HitPoints.TemporaryHitPoints
			};
			
			return Ok(characterHitPoints);
		}
	}
}
