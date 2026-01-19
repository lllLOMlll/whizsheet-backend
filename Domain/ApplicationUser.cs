using Microsoft.AspNetCore.Identity;

namespace Whizsheet.Api.Domain
{
	public class ApplicationUser : IdentityUser
	{
		/*
		Identity user already contains Id, UserName, Email
		EmailConfirmed, PasswordHas, SecurityStamp, LockoutEnabled, 
		AccesFailedCount, etc.
		*/
		// Extension future :
		// public string? DisplayName { get; set; }
	}
}
