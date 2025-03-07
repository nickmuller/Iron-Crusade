using System.Security.Claims;
using System.Text;
using System.Text.Json;
using IronCrusade.Shared;
using Microsoft.Azure.Functions.Worker.Http;

namespace Api.Authentication;

public static class StaticWebAppsAuth
{
    private const string ClientPrincipalHeader = "x-ms-client-principal";

    public static ClaimsPrincipal Parse(HttpRequestData req)
    {
        var principal = new ClientPrincipal();

        if (req.Headers.TryGetValues(ClientPrincipalHeader, out var headers))
        {
            var header = headers.First();
            var decoded = Convert.FromBase64String(header);
            var json = Encoding.UTF8.GetString(decoded);
            principal = JsonSerializer.Deserialize<ClientPrincipal>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
        
        principal.UserRoles = principal.UserRoles?.Except(["anonymous"], StringComparer.CurrentCultureIgnoreCase).ToArray();
        
        if (!principal.UserRoles?.Any() ?? true)
        {
            return new ClaimsPrincipal();
        }

        var identity = new ClaimsIdentity(principal.IdentityProvider);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId!));
        identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails!));
        identity.AddClaims(principal.UserRoles!.Select(r => new Claim(ClaimTypes.Role, r)));

        return new ClaimsPrincipal(identity);
    }
}