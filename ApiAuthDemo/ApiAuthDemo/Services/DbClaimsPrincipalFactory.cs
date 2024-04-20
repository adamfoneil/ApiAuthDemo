using ApiAuthDemo.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ApiAuthDemo.Services;

public class DbClaimsPrincipalFactory(
	UserManager<ApplicationUser> userManager,
	IOptions<IdentityOptions> optionsAccessor) : UserClaimsPrincipalFactory<ApplicationUser>(userManager, optionsAccessor)
{
	protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
	{		
		var identity = await base.GenerateClaimsAsync(user);
		identity.AddClaim(new Claim(nameof(ApplicationUser.TimeZoneId), user.TimeZoneId ?? string.Empty));
		return identity;
	}
}
