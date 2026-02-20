using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Dtos.Characters;
using Whizsheet.Api.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Whizsheet.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/v1/characters")]
	public class CharactersController : ControllerBase
	{
		private readonly WhizsheetDbContext _db;

		public CharactersController(WhizsheetDbContext db)
		{
			_db = db;
		}


		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			
			var characters = await _db.Characters
				.Where(c => c.UserId == userId)
				.Select(c => new CharacterDto
				{
					Id = c.Id,
					Name = c.Name,								
				}).ToListAsync();

			return Ok(characters);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateCharacterDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			
			if (userId == null)
			{
				return Unauthorized();
			}

			var characterCount = await _db.Characters
					.CountAsync(c => c.UserId == userId);

			if (characterCount >= 5)
			{
				return BadRequest(new
				{
					error = "CHARACTER_LIMIT_REACHED",
					message = "You can only create up to 2 characters."
				});
			}

			var character = new Character(dto.Name, userId);

			_db.Characters.Add(character);
			await _db.SaveChangesAsync();

			var result = new CharacterDto
			{
				Id = character.Id,
				Name = character.Name,		
			};
		
			return CreatedAtAction(
				nameof(GetById),
				new { id = character.Id },
				result
			);
		}

			[HttpDelete("{id:int}")]
			public async Task<IActionResult> Delete(int id)
			{
				var character = await _db.Characters.FindAsync(id);

				if (character == null)
					return NotFound();

				_db.Characters.Remove(character);
				await _db.SaveChangesAsync();

				return NoContent(); // 204
			}

			[HttpGet("{id:int}")]
			public async Task<IActionResult> GetById(int id)
			{
				var character = await _db.Characters.FindAsync(id);

				if (character is null)
					return NotFound();

				var dto = new CharacterDto
				{
					Id = character.Id,
					Name = character.Name,			
				};

				return Ok(dto);
			}

			[HttpPut("{id:int}")]
			public async Task<IActionResult> Update(int id, UpdateCharacterDto dto)
			{
				var character = await _db.Characters.FindAsync(id);

				if (character is null)
					return NotFound();

				character.Name = dto.Name;		

				await _db.SaveChangesAsync();

				return NoContent(); // 204
			}

		}
	}
