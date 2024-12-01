using BusinessCardManagerAPI.Services;
using BusinessCardManagerAPI.Services.Interfaces;

namespace BusinessCardManagerAPI.Extensions
{
    public static class DIContainer
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IBusinessCardService, BusinessCardService>();

            return services;
        }
    }
}
