using BusinessCardManagerAPI.Constants;
using BusinessCardManagerAPI.Data;
using BusinessCardManagerAPI.Data.Interceptors;
using BusinessCardManagerAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BusinessCardManagerAPI
{
    public class Program
    {
        const string policyName = "AllowSpecificOrigin";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
               
            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            //Register DbContext
            builder.Services.AddDbContext<BusinessCardDbContext>(options => options
             .UseSqlServer(builder.Configuration.GetConnectionString(AppConstants.ConnectionStringDebugKey))
             .AddInterceptors(new SoftDeleteInterceptor()));

            //Register Services
            builder.Services.RegisterServices()
                            .AddEndpointsApiExplorer()
                            .AddSwaggerGen();

            //Cross-Origin Resource Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder => builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()); 
            });
             
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles(); 

            app.UseAuthorization();

            app.UseCors(policyName);

            app.MapControllers();

            app.Run();
        }
    }
}
