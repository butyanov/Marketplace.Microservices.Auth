using Auth.API.Data.Configuration.Abstractions;
using Auth.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.API.Data.Configuration;

public class DomainUserConfiguration : BaseConfiguration<DomainUser>
{
    public override void ConfigureChild(EntityTypeBuilder<DomainUser> typeBuilder)
    {
        typeBuilder.HasIndex(x => x.IdentityUserId);
        typeBuilder.HasIndex(x => x.Email).IsUnique();
        typeBuilder.HasIndex(x => x.PhoneNumber).IsUnique();
    }
    
}