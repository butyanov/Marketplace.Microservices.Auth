using System.Net;

namespace Auth.API.Exceptions;

public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message, (int)HttpStatusCode.Conflict)
    {
    }
}