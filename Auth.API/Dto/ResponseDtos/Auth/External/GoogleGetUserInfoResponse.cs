using Newtonsoft.Json;

namespace Auth.API.Dto.ResponseDtos.Auth.External;

public class GoogleGetUserInfoResponse : ExternalGetUserInfoResponse
{
    [JsonProperty("given_name")]
    public string? GivenName { get; set; }
    [JsonProperty("verified_email")]
    public bool VerifiedEmail { get; set; }
    public string? Picture { get; set; }
    public string? Locale { get; set; }
}