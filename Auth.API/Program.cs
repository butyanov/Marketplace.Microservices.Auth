using Auth.API.Configuration;
using Auth.API.Data;
using Auth.API.Data.Interfaces;
using Auth.API.Endpoints;
using Auth.API.Middleware;
using Auth.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddCustomLogging();

var services = builder.Services;

services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AuthDbContext>();
services.AddDbContext<IDomainDbContext,AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
services.AddCustomEntitiesConfiguration();

services
    .AddCustomServicesConfiguration()
    .AddRedisConfiguration(builder.Configuration)
    .AddCustomSwaggerConfiguration(builder.Configuration)
    .AddCustomAuthentication(builder.Configuration)
    .AddCustomAuthorization()
    .AddCustomValidation()
    .AddCustomApiConfiguration()
    .AddCustomEndpointHandlersConfiguration();
    

var app = builder.Build();

app.UseCustomMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

foreach (var endpointsRoot in app.Services.GetServices<IEndpointsRoot>())
    endpointsRoot.MapEndpoints(app);

app.Run();