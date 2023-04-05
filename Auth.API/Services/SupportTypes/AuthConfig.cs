namespace Auth.API.Services.SupportTypes;

public static class AuthConfig
{
    public const string ConfigSectionName = "AuthConfig";

    public static readonly int TokenLifetime = 10 * 60;
    
    public static readonly int RefreshTokenLifetime = 7 * 24 * 60 * 60;
    
    public static readonly int TicketLifetime = 2 * 60;

    public static readonly int TicketRequestLifetime = 5 * 60;

    public static readonly int TicketCooldown = 2 * 60;
}
