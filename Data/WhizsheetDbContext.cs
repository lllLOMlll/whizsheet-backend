
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Whizsheet.Api.Domain;

namespace Whizsheet.Api.Infrastructure
{
	public class WhizsheetDbContext : IdentityDbContext<ApplicationUser>
	{
		public WhizsheetDbContext(DbContextOptions<WhizsheetDbContext> options)
			: base(options)
		{
		}

		public DbSet<Character> Characters => Set<Character>();
		public DbSet<AbilityScores> AbilityScores => Set<AbilityScores>();
		public DbSet<CharacterClass> CharacterClasses => Set<CharacterClass>();
		public DbSet<HitPoints> HitPoints => Set<HitPoints>();
		public DbSet<HitDicePool> HitDicePools => Set<HitDicePool>();

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Character → User
			builder.Entity<Character>()
				.HasOne(character => character.User)
				.WithMany()
				.HasForeignKey(character => character.UserId)
				.IsRequired();

			// Character → AbilityScores (1–1)
			builder.Entity<Character>()
				.HasOne(character => character.AbilityScores)
				.WithOne(scores => scores.Character)
				.HasForeignKey<AbilityScores>(scores => scores.CharacterId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			// Character → CharacterClass (1–N)
			builder.Entity<Character>()
				.HasMany(character => character.Classes)
				.WithOne(characterClass => characterClass.Character)
				.HasForeignKey(characterClass => characterClass.CharacterId)
				.OnDelete(DeleteBehavior.Cascade);

			// CharacterClass constraints (métier)
			builder.Entity<CharacterClass>()
				.HasIndex(cc => new
				{
					cc.CharacterId,
					cc.ClassType,
					cc.CustomClassName
				})
				.IsUnique();

			// HitPoints (1-1)
			builder.Entity<Character>()
				.HasOne(character => character.HitPoints)
				.WithOne(hitpoints => hitpoints.Character)
				.HasForeignKey<HitPoints>(hitpoints => hitpoints.CharacterId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			// HitDicePools (1–N)
			builder.Entity<Character>()
				.HasMany(c => c.HitDicePools)
				.WithOne(p => p.Character)
				.HasForeignKey(p => p.CharacterId)
				.OnDelete(DeleteBehavior.Cascade);

			// Indique à EF d'utiliser le champ privé pour la collection
			builder.Entity<Character>()
				.Navigation(c => c.HitDicePools)
				.UsePropertyAccessMode(PropertyAccessMode.Field);

			// Contraintes sur HitDicePool
			builder.Entity<HitDicePool>()
				.Property(p => p.DiceSize)
				.IsRequired();

			builder.Entity<HitDicePool>()
				.Property(p => p.Total)
				.IsRequired();

			builder.Entity<HitDicePool>()
				.Property(p => p.Remaining)
				.IsRequired();






		}
	}

}
