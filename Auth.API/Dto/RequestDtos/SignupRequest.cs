namespace Auth.API.Dto.RequestDtos;

public class SignupRequest
{
    public required string Name { get; set; }

    public required string Password { get; set; }
    
    public required string Ticket { get; set; }
}
