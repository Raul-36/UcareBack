using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UcareBackApp.Data;
using UcareBackApp.Identity.Entities;

namespace UcareBackApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void InitAspnetIdentity(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<UcareDbContext>(options =>
            {
                var connectinoString = configuration.GetConnectionString("psqlDb");
                options.UseNpgsql(connectinoString);
            });

            serviceCollection.AddIdentity<UcareUser, UcareRole>()
                .AddEntityFrameworkStores<UcareDbContext>();
        }
        public static void InitSwagger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Cliently Business Info Service",
                    Version = "v1",
                });

                options.AddSecurityDefinition("cookieAuth", new OpenApiSecurityScheme
                {
                    Name = ".AspNetCore.Cookies",       
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Cookie,
                    Description = "Cookie-based authentication for ASP.NET Identity"
                });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement() {
                        {
                            new OpenApiSecurityScheme
                            {
                                Name = ".AspNetCore.Cookies",
                                Type = SecuritySchemeType.ApiKey,
                                In = ParameterLocation.Cookie,
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "cookieAuth"
                                }
                            },
                            new string[]{}
                        }
                    }
                );
            });
        }
    }
}