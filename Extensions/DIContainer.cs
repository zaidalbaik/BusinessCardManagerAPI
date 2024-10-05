namespace BusinessCardManagerAPI.Extensions
{
    public static class DIContainer
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            //services.AddScoped<IService, Service>();  

            return services;
        }
    }
}
