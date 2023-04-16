using Newtonsoft.Json;

namespace Auth.API.Dto.ResponseDtos.Auth.External;

public class GoogleGetUserInfoResponse : ExternalGetUserInfoResponse
{
    public string Name { get; set; }
    public string? Email { get; set; }
    [JsonProperty("verified_email")]
    public bool VerifiedEmail { get; set; }
    public string? Phone { get; set; }
    [JsonProperty("given_name")]
    public string? GivenName { get; set; }
    public string? Picture { get; set; }
    public string? Locale { get; set; }
}