namespace IronCrusade.Client.Models;

public class UserInfo
{
    public ClientPrincipal? ClientPrincipal { get; set; }
}

public class ClientPrincipal
{
    public string? IdentityProvider { get; set; }
    public string? UserId { get; set; }
    public string? UserDetails { get; set; }
    public string[]? UserRoles { get; set; }
}