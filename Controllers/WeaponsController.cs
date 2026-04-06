using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Domain.Items;
using Whizsheet.Api.Dtos.MagicItem;
using Whizsheet.Api.Dtos.Weapon;
using Whizsheet.Api.Infrastructure;

namespace Whizsheet.Api.Controllers
{
	[Authorize]
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
		public async Task<IActionResult> Create(int characterId, [FromBody] CreateWeaponDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				return Unauthorized();
			}

			var character = await _db.Characters
				.FirstOrDefaultAsync(c =>
					c.Id == characterId &&
					c.UserId == userId);

			if (character == null)
			{
				return NotFound();
			}

			var weapon = new Weapon
			{
				Id = Guid.NewGuid(),
				CharacterId = characterId,

				Name = dto.Name,
				Description = dto.Description,
				ItemRarity = dto.ItemRarity,
				Value = dto.Value,
				Weight = dto.Weight,

				IsEquipped = dto.IsEquipped,
				IsAttuned = dto.IsAttuned,
				Quantity = dto.Quantity,

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
					RequiresAttunement = dto.MagicItem.RequiresAttunement,
					MagicEffectDescription = dto.MagicItem.MagicEffectDescription,
					MagicEffectMechanics = dto.MagicItem.MagicEffectMechanics,
					TotalCharges = dto.MagicItem.TotalCharges,
					ChargesRemaining = dto.MagicItem.ChargesRemaining,
					MagicRechargeRate = dto.MagicItem.MagicRechargeRate
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
						Modifier = effectDto.Modifier
					});
				}

				weapon.MagicItem = magicItem;
			}

			_db.Set<Weapon>().Add(weapon);

			await _db.SaveChangesAsync();

			var result = await _db.Set<Weapon>()
				.Include(w => w.MagicItem)
					.ThenInclude(mi => mi.MagicItemEffects)
				.FirstAsync(w => w.Id == weapon.Id);

			var weaponDto = ConvertWeaponToDto(result);

			return CreatedAtAction(
				nameof(GetById),
				new { characterId = characterId, weaponId = weapon.Id },
				weaponDto);
		}

		[HttpGet]
		public async Task<IActionResult> Get(int characterId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				return Unauthorized();
			}

			var characterExists = await _db.Characters
				.AnyAsync(c =>
					c.Id == characterId &&
					c.UserId == userId);

			if (!characterExists)
			{
				return NotFound();
			}

			var weapons = await _db.Set<Weapon>()
				.Include(w => w.MagicItem)
					.ThenInclude(mi => mi.MagicItemEffects)
				.Where(w =>
					w.CharacterId == characterId &&
					w.Character.UserId == userId)
				.ToListAsync();

			var weaponDtos = weapons
				.Select(ConvertWeaponToDto)
				.ToList();

			return Ok(weaponDtos);
		}

		[HttpGet("{weaponId:guid}")]
		public async Task<IActionResult> GetById(int characterId, Guid weaponId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				return Unauthorized();
			}

			var weapon = await _db.Set<Weapon>()
				.Include(w => w.MagicItem)
					.ThenInclude(mi => mi.MagicItemEffects)
				.FirstOrDefaultAsync(w =>
					w.Id == weaponId &&
					w.CharacterId == characterId &&
					w.Character.UserId == userId);

			if (weapon == null)
			{
				return NotFound();
			}

			var weaponDto = ConvertWeaponToDto(weapon);

			return Ok(weaponDto);
		}

		[HttpDelete("{weaponId:guid}")]
		public async Task<IActionResult> Delete(int characterId, Guid weaponId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
				return Unauthorized();

			var weapon = await _db.Weapons
				.FirstOrDefaultAsync(w =>
					w.Id == weaponId &&
					w.CharacterId == characterId &&
					w.Character.UserId == userId);

			if (weapon == null)
				return NotFound();


			_db.Weapons.Remove(weapon);

			await _db.SaveChangesAsync();

			return NoContent();
		}


		[HttpPatch("{weaponId:guid}/equip")]
		public async Task<IActionResult> ToggleEquip(int characterId, Guid weaponId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
				return Unauthorized();

			var weapon = await _db.Weapons
					.FirstOrDefaultAsync(w =>
						w.Id == weaponId &&
						w.CharacterId == characterId &&
						w.Character.UserId == userId);

			if (weapon == null)
				return NotFound();

			weapon.IsEquipped = !weapon.IsEquipped;

			await _db.SaveChangesAsync();

			return Ok(new { weaponId, weapon.IsEquipped });
		}

		private WeaponDto ConvertWeaponToDto(Weapon weapon)
		{
			return new WeaponDto
			{
				Id = weapon.Id,
				Name = weapon.Name,
				Description = weapon.Description,
				ItemRarity = weapon.ItemRarity,
				Value = weapon.Value,
				Weight = weapon.Weight,
				IsEquipped = weapon.IsEquipped,
				IsAttuned = weapon.IsAttuned,
				Quantity = weapon.Quantity,

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

				MagicItem = ConvertMagicItemToDto(weapon.MagicItem)
			};
		}

		private MagicItemDto? ConvertMagicItemToDto(MagicItem? magicItem)
		{
			if (magicItem == null)
			{
				return null;
			}

			var dto = new MagicItemDto
			{
				Id = magicItem.Id,
				RequiresAttunement = magicItem.RequiresAttunement,
				MagicEffectDescription = magicItem.MagicEffectDescription,
				MagicEffectMechanics = magicItem.MagicEffectMechanics,
				TotalCharges = magicItem.TotalCharges,
				ChargesRemaining = magicItem.ChargesRemaining,
				MagicRechargeRate = magicItem.MagicRechargeRate
			};

			foreach (var effect in magicItem.MagicItemEffects)
			{
				dto.Effects.Add(new MagicItemEffectDto
				{
					Id = effect.Id,
					EffectType = effect.EffectType,
					AbilityScore = effect.AbilityScore,
					SavingThrow = effect.SavingThrow,
					Skill = effect.Skill,
					Modifier = effect.Modifier
				});
			}

			return dto;
		}
	}
}