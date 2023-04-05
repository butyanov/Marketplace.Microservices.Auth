using Auth.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Data.Interfaces;

public interface IDomainDbContext
{
    public DbSet<DomainUser> MarketUsers { get; set; }

    public Task<bool> SaveEntitiesAsync();
}