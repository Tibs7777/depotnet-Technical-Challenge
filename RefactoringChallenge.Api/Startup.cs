using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using Repository;
using Repository.Contracts;
using RefactoringChallenge.Extensions;
using Services;
using Services.Contracts;

//Given more time I would use extension methods to clean up the startup class

//For production I would:
//Add a rate limiter
//Use CORS
//Add Better logging
//Ensure the prod db connection string is behind any secret storing method, like env variables, an uncomitted secrets file, or Azure Key Vault
//Use useHsts for security
//Add Authentication (But this is more of a feature, and may not be needed in all apps)

namespace RefactoringChallenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection").EnableSensitiveDataLogging());

            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            services.AddSwaggerGen();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandler();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
