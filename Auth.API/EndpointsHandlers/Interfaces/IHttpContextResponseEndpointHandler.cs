namespace Auth.API.EndpointsHandlers.Interfaces;

public interface IHttpContextResponseEndpointHandler<TRes> : IRequestResponseEndpointHandler<HttpContext, TRes>
{
}