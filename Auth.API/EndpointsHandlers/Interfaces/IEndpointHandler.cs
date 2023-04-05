namespace Auth.API.EndpointsHandlers.Interfaces;

public interface IEndpointHandler<TReq, TRes>
{
    public Task<TRes> Handle(TReq request);
}