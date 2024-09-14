using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UcareBackApp.Data;

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

            serviceCollection.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<UcareDbContext>();
        }
    }
}