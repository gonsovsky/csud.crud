using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Csud.Crud.RestApi
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
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "ЦСУД API",
                        Version = "v1",
                        Description = "API для администрирования ЦСУД",
                        TermsOfService = new Uri("https://gnivc.ru"),
                        Contact = new OpenApiContact
                        {
                            Name = "info@gnivc.ru",
                            Email = "info@gnivc.ru",
                            Url = new Uri("https://gnivc.ru"),
                        }
                    });
                  
                }

            );
            services.AddSingleton(typeof(IConfiguration), Configuration);

            services.TryAdd(ServiceDescriptor.Singleton(typeof(Config),
                typeof(Config)));

            CsudService.ConfigureServices(services);

            services.AddControllers();
        }

       

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            // app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ЦСУД API V1"); });
        }
    }
}
