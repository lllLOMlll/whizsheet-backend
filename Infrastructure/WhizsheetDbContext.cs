using Microsoft.EntityFrameworkCore;
using Whizsheet.Api.Domain;

namespace Whizsheet.Api.Infrastructure
{
	public class WhizsheetDbContext : DbContext
	{
		public WhizsheetDbContext(DbContextOptions<WhizsheetDbContext> options) 
			: base(options) 
		{ 
		}

		public DbSet<Character> Characters => Set<Character>();
	}
}
