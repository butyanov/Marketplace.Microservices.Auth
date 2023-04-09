namespace Auth.API.EndpointsHandlers.Interfaces;

public interface IRequestEndpointHandler<TReq>
{
    public Task Handle(TReq request);
}