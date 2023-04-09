namespace Auth.API.Dto.RequestDtos.Auth;

public class SignupRequest
{
    public required string Ticket { get; set; }
    
    public required string Name { get; set; }

    public required string Password { get; set; }
}
