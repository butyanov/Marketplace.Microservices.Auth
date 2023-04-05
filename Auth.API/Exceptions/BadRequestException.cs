using System.Net;

namespace Auth.API.Exceptions;

public class BadRequestException<T> : DomainException
{
    public BadRequestException() : base(
        ErrorCodes.NotFoundError, (int)HttpStatusCode.BadRequest)
    {
        PlaceholderData.Add("EntityName", typeof(T).Name);
    }
}