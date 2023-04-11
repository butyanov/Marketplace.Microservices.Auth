using Auth.API.Models;

namespace Auth.API.EndpointsHandlers.Interfaces;

public interface IRequestResponseEndpointHandler<TReq, TRes> 
{
    public Task<TRes> Handle(TReq request);
}