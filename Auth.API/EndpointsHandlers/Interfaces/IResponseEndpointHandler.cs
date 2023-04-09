namespace Auth.API.EndpointsHandlers.Interfaces;

public interface IResponseEndpointHandler<TRes>
{
    public Task<TRes> Handle();
}