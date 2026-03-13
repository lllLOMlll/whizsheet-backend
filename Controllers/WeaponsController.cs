using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Domain.Items;
using Whizsheet.Api.Dtos.MagicItem;
using Whizsheet.Api.Dtos.Weapon;
using Whizsheet.Api.Enum;
using Whizsheet.Api.Enum.Item;
using Whizsheet.Api.Enum.Weapon;
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
				.FirstOrDefaultAsync(c =>
					c.Id == characterId &&
					c.UserId == userId);

			if (character == null) return NotFound();

			var weapons = character.Items
				.Select(ci => ci.Item)
				.OfType<Weapon>()
				.ToList();

			if (!weapons.Any()) return NotFound();

			var weaponListDto = new List<WeaponDto>();
			foreach (var weapon in weapons)
			{
				var weaponDto = new WeaponDto
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
					MagicItem = ConvertMagicItemWeaponDomainToDTO(weapon)
				};

				weaponListDto.Add(weaponDto);
			}


			return Ok(weaponListDto);
		}


		public MagicItemDto? ConvertMagicItemWeaponDomainToDTO(Weapon weapon)
		{
			if (weapon.MagicItem == null)
				return null;

			var magicItemDto = new MagicItemDto
			{
				RequiresAttunement = weapon.MagicItem.RequiresAttunement
			};

			foreach (var effect in weapon.MagicItem.MagicItemEffects)
			{
				magicItemDto.Effects.Add(new MagicItemEffectDto
				{
					EffectType = effect.EffectType,
					AbilityScore = effect.AbilityScore,
					SavingThrow = effect.SavingThrow,
					Skill = effect.Skill,
					Modifier = effect.Modifier,
					Description = effect.Description
				});
			}

			return magicItemDto;
		}




	}
}
