using Auth.API.Models.Abstractions;

namespace Auth.API.Models;

public class DomainUser : BaseEntity
{
    public required string Name { get; set; }

    public required string? PhoneNumber { get; set; }
    
    public required string? Email { get; set; }
    
    public string? Country { get; set; }
    
    public string? PostalIndex { get; set; }

    public required Guid IdentityUserId { get; set; }

}


