using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Service;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> userManager;

    public CustomProfileService(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await userManager.GetUserAsync(context.Subject);
        var existingClaims = await userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new("username", user.UserName)
        };

        context.IssuedClaims.AddRange(claims);
        context.IssuedClaims.Add(existingClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name));
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}
