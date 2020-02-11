using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RelayCalculator.Services;
using RelayCalculator.Services.Interfaces;
using Swashbuckle.AspNetCore.Swagger;


namespace RelayCalculator.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddSingleton<IPermutationService, PermutationService>();
            services.AddSingleton<IGroupService, GroupService>();
            services.AddSingleton<IBestTeamCalculationService, Freestyle200Relay>();
            services.AddSingleton<ISwimTimeService, SwimTimeService>();
            services.AddSingleton<IHtmlDocumentService, HtmlDocumentService>();
            services.AddSingleton<ISearchSwimmerService, SearchSwimmersService>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });


            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Version = "v1",
                        Title = "RelaySwim API",
                        Description = "The API endpoints for the RelaySwim Application.",
                    });

                 // Set the comments path for the Swagger JSON and UI.
                 var xmlPath = Path.Combine(AppContext.BaseDirectory, "RelayCalculator.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors("CorsPolicy");

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RelaySwim Api V1");
            });

            //app.UseMvc();
        }
    }
}
