using Auth.API.Data.Configuration.Abstractions;
using Auth.API.Data.Interfaces;
using Auth.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IDomainDbContext
{
    private readonly IEnumerable<DependencyInjectedEntityConfiguration> _configurations;
    public DbSet<ApplicationUser> IdentityUsers { get; set; }
    public DbSet<DomainUser> MarketUsers { get; set; }
    public DbSet<PermissionsModel> Permissions { get; set; }
    
    public AuthDbContext(
        DbContextOptions<AuthDbContext> options, IEnumerable<DependencyInjectedEntityConfiguration> configurations
    ) : base(options)
    {
        _configurations = configurations;
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        foreach (var entityTypeConfiguration in _configurations)
            entityTypeConfiguration.Configure(builder);
    }
    public async Task<bool> SaveEntitiesAsync()
    {
        await base.SaveChangesAsync();
        return true;
    }
}
