using System.Security.Claims;

namespace MarketPlace.Helpers;

public static class ClaimPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
