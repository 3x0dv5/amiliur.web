using System.Text.Json;

namespace amiliur.security.shared.Services;

public class ClaimService
{
    protected virtual List<string> AdminRoles { get; } = new();
    
    public bool HasClaimValue(System.Security.Claims.ClaimsPrincipal user, string claimType, string claimValue)
    {
        if (claimType != "role" && AdminRoles.Select(m=> HasClaimValue( user,"role", m)).Any())
            return true;

        var c = user.Claims.SingleOrDefault(m => m.Type == claimType);
        if (c == null)
            return false;
        try
        {
            var cValues = JsonSerializer.Deserialize<List<string>>(c.Value);
            return cValues != null && cValues.Contains(claimValue);
        }
        catch (Exception)
        {
            // ignored
        }

        return c.Value == claimValue;
    }
}