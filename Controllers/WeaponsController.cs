using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Whizsheet.Api.Domain.Items;
using Whizsheet.Api.Dtos.MagicItem;
using Whizsheet.Api.Dtos.Weapon;
using Whizsheet.Api.Infrastructure;

namespace Whizsheet.Api.Controllers
{
	[ApiController]
	[Route("api/v1/characters/{characterId:int}/items/weapons")]
	public class WeaponsController : ControllerBase
	{
		private readonly WhizsheetDbContext _db;

		public WeaponsController(WhizsheetDbContext db)
		{
			_db = db;
		}

		[HttpPost]
		public async Task<IActionResult> Create(
			int characterId,
			CreateCharacterWeaponDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
				return Unauthorized();

			var character = await _db.Characters
				.FirstOrDefaultAsync(c =>
					c.Id == characterId &&
					c.UserId == userId);

			if (character == null)
				return NotFound();

			var weapon = new Weapon
			{
				Id = Guid.NewGuid(),
				Name = dto.Name,
				Description = dto.Description,
				ItemRarity = dto.ItemRarity,
				Value = dto.Value,
				Weight = dto.Weight,

				AttackType = dto.AttackType,
				BonusAttackRollType = dto.BonusAttackRollType,
				DamageDiceType = dto.DamageDiceType,
				DamageType = dto.DamageType,
				RangeType = dto.RangeType,

				IsLight = dto.IsLight,
				IsFinesse = dto.IsFinesse,
				IsThrown = dto.IsThrown,
				IsVersatile = dto.IsVersatile,
				IsAmmunition = dto.IsAmmunition,
				IsHeavy = dto.IsHeavy,
				IsReach = dto.IsReach,
				IsTwoHanded = dto.IsTwoHanded,
				IsLoading = dto.IsLoading,
				IsSpecial = dto.IsSpecial
			};

			if (dto.MagicItem != null)
			{
				var magicItem = new MagicItem
				{
					Id = Guid.NewGuid(),
					RequiresAttunement = dto.MagicItem.RequiresAttunement
				};

				foreach (var effectDto in dto.MagicItem.Effects)
				{
					magicItem.MagicItemEffects.Add(new MagicItemEffect
					{
						Id = Guid.NewGuid(),
						EffectType = effectDto.EffectType,
						AbilityScore = effectDto.AbilityScore,
						SavingThrow = effectDto.SavingThrow,
						Skill = effectDto.Skill,
						Modifier = effectDto.Modifier,
						Description = effectDto.Description
					});
				}

				weapon.MagicItem = magicItem;
			}

			var characterItem = new CharacterItem
			{
				Id = Guid.NewGuid(),
				CharacterId = characterId,
				Item = weapon,
				Quantity = dto.Quantity,
				IsEquipped = dto.IsEquipped,
				IsAttuned = dto.IsAttuned,
				ChargesRemaining = dto.ChargesRemaining
			};

			_db.CharacterItems.Add(characterItem);

			await _db.SaveChangesAsync();

			return Ok(new { characterItem.Id });
		}

		[HttpGet]
		public async Task<IActionResult> Get(int characterId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
				return Unauthorized();

			var character = await _db.Characters
				.Include(c => c.Items)
					.ThenInclude(ci => ci.Item)
						.ThenInclude(i => i.MagicItem)
							.ThenInclude(mi => mi.MagicItemEffects)
				.FirstOrDefaultAsync(c =>
					c.Id == characterId &&
					c.UserId == userId);

			if (character == null)
				return NotFound();

			var result = character.Items
				.Where(ci => ci.Item is Weapon)
				.Select(ci => new CharacterWeaponDto
				{
					CharacterItemId = ci.Id,

					Quantity = ci.Quantity,
					IsEquipped = ci.IsEquipped,
					IsAttuned = ci.IsAttuned,
					ChargesRemaining = ci.ChargesRemaining,

					Weapon = ConvertWeaponDomainToDTO((Weapon)ci.Item)
				})
				.ToList();

			return Ok(result);
		}

		// MAP WEAPON
		private WeaponDto ConvertWeaponDomainToDTO(Weapon weapon)
		{
			return new WeaponDto
			{
				Id = weapon.Id,
				Name = weapon.Name,
				Description = weapon.Description,
				ItemRarity = weapon.ItemRarity,
				Value = weapon.Value,
				Weight = weapon.Weight,

				AttackType = weapon.AttackType,
				BonusAttackRollType = weapon.BonusAttackRollType,
				DamageDiceType = weapon.DamageDiceType,
				DamageType = weapon.DamageType,
				RangeType = weapon.RangeType,

				IsLight = weapon.IsLight,
				IsFinesse = weapon.IsFinesse,
				IsThrown = weapon.IsThrown,
				IsVersatile = weapon.IsVersatile,
				IsAmmunition = weapon.IsAmmunition,
				IsHeavy = weapon.IsHeavy,
				IsReach = weapon.IsReach,
				IsTwoHanded = weapon.IsTwoHanded,
				IsLoading = weapon.IsLoading,
				IsSpecial = weapon.IsSpecial,

				MagicItem = ConvertMagicItemDomainToDTO(weapon.MagicItem)
			};
		}

		// MAP MAGIC ITEM
		private MagicItemDto? ConvertMagicItemDomainToDTO(MagicItem? magicItem)
		{
			if (magicItem == null)
				return null;

			var dto = new MagicItemDto
			{
				RequiresAttunement = magicItem.RequiresAttunement
			};

			foreach (var effect in magicItem.MagicItemEffects)
			{
				dto.Effects.Add(new MagicItemEffectDto
				{
					EffectType = effect.EffectType,
					AbilityScore = effect.AbilityScore,
					SavingThrow = effect.SavingThrow,
					Skill = effect.Skill,
					Modifier = effect.Modifier,
					Description = effect.Description
				});
			}

			return dto;
		}
	}
}