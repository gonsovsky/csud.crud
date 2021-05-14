using System;
using Csud.Crud.Models.Maintenance;
using Csud.Crud.Models.Rules;
using Csud.Crud.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
            CsudService.StartUp(new Config(Configuration));

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
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<Account>), 
                typeof(EntityService<Account>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<AccountProvider>),
                typeof(EntityService<AccountProvider>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<Person>),
                typeof(EntityService<Person>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<ObjectX>),
                typeof(EntityService<ObjectX>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<Subject>),
                typeof(EntityService<Subject>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<AppImport>),
                typeof(EntityService<AppImport>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToManyService<Group, GroupAdd, Subject>), 
                typeof(OneToManyService<Group, GroupAdd, Subject>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToManyService<TaskX, TaskAdd, ObjectX>),
                typeof(OneToManyService<TaskX, TaskAdd, ObjectX>)));

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
