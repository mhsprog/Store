using Application.Core;
using Application.Interfaces;
using Application.Products;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        });

        services.AddDbContext<DataContext>(
            option => option.UseSqlServer(config.GetConnectionString("DefaultConnection")));


        services.AddMediatR(typeof(Create.Handler).Assembly);
        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddScoped<IUserAccessor, UserAccessor>();

        return services;
    }
}
