using Auth.API.Data.Configuration.Abstractions;
using Auth.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.API.Data.Configuration;

public class PermissionsModelConfiguration : DependencyInjectedEntityConfiguration<PermissionsModel>
{
    public override void Configure(EntityTypeBuilder<PermissionsModel> builder)
    {
        builder.HasOne<DomainUser>()
            .WithOne()
            .HasForeignKey<PermissionsModel>(p => p.Id);
        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<PermissionsModel>(p => p.Id);
    }
}