using Auth.API.Configuration;
using Auth.API.Data;
using Auth.API.Data.Interfaces;
using Auth.API.Endpoints;
using Auth.API.EndpointsHandlers.Auth;
using Auth.API.Middleware;
using Auth.API.Models;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddCustomLogging();

var services = builder.Services;

services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<AuthDbContext>();
services.AddDbContext<IDomainDbContext, AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
services.AddCustomEntitiesConfiguration();

services
    .AddHttpContextAccessor()
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
app.UseCustomMiddleware<DbTransactionsMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

foreach (var endpointsRoot in app.Services.GetServices<IEndpointsRoot>())
    endpointsRoot.MapEndpoints(app);



app.Run();