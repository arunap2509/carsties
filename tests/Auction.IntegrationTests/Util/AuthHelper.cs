using System.Security.Claims;

namespace Auction.IntegrationTests.Util;

public class AuthHelper
{
    public static Dictionary<string, object> GetBearerForUser(string username)
    {
        return new Dictionary<string, object> { { ClaimTypes.Name, username } };
    }
}
