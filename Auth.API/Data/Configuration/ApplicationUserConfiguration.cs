using Auth.API.Data.Configuration.Abstractions;
using Auth.API.Data.Configuration.Extensions;
using Auth.API.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Auth.API.Data.Configuration;

public class ApplicationUserConfiguration : DependencyInjectedEntityConfiguration<ApplicationUser>
{
    private readonly JsonOptions _jsonOptions;

    public ApplicationUserConfiguration(IOptions<JsonOptions> jsonOptions)
    {
        _jsonOptions = jsonOptions.Value;
    }
    
    public override void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.RefreshTokens)
            .HasJsonConversion<IReadOnlyList<RefreshToken>, List<RefreshToken>>(_jsonOptions.SerializerOptions);
    }
}