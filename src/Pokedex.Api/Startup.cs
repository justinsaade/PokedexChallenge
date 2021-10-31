using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pokedex.Api.Clients.PokeApi;
using Pokedex.Api.Clients.TranslatorApi;
using Pokedex.Api.Exceptions;
using Pokedex.Api.Services;
using System;

namespace Pokedex.Api
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pokedex.Api", Version = "v1" });
            });

            RegisterExternalApis(services);

            services.AddScoped<IPokedexService, PokedexService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokedex.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterExternalApis(IServiceCollection services)
        {
            services.AddHttpClient<IPokeApiClient, PokeApiClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration["ExternalApis:PokeApi"]);
            });

            services.AddHttpClient<ITranslatorApiClient, TranslatorApiClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration["ExternalApis:FunTranslations"]);
            });
        }
    }
}