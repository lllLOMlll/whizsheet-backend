
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

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Character>()
				.HasOne(c => c.User)
				.WithMany()
				.HasForeignKey(c => c.UserId)
				.IsRequired();
		}
	}
}
