using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Kafka.Infra.CrossCutting.Identity.Configurations;
using Kafka.Services.API.Configuration;
using Kafka.Services.API.Configurations;

namespace Kafka.Services.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables().Build();
            //Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            IdentityModelEventSource.ShowPII = true;

            services.AddControllers(options =>
            {
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                //options.Filters.Add(new ServiceFilterAttribute(typeof(GlobalActionLogger)));
            }).AddNewtonsoftJson(options =>
                            {
                                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                options.SerializerSettings.ContractResolver
                                    = new CamelCasePropertyNamesContractResolver();

                                options.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
                                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.Configure<MvcNewtonsoftJsonOptions>(config =>
            {
                config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMvcSecurity(Configuration);
            services.AddSwaggerConfig();
            services.AddDIConfiguration();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Kafka v1.0");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseRequestLocalization();
            app.UseHttpsRedirection();
        }
    }
}
