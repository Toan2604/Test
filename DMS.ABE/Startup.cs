using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Handlers.Configuration;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using DMS.ABE.Rpc;
using DMS.ABE.Services;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using Newtonsoft.Json;
using OfficeOpenXml;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Thinktecture;
using Z.EntityFramework.Extensions;
using System.Reflection;
using Nest;

namespace DMS.ABE
{
    public class MyDesignTimeService : DesignTimeService { }

    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            LicenseManager.AddLicense("2456;100-FPT", "3f0586d1-0216-5005-8b7a-9080b0bedb5e");
            string licenseErrorMessage;
            if (!LicenseManager.ValidateLicense(out licenseErrorMessage))
                throw new Exception(licenseErrorMessage);
            //DocumentUltimateConfiguration.Current.LicenseKey = "QTSHF7N4NU-MFTCJGT46Z-CPSL9AZA1Q-U4GPFSM1CG-DYHAPW27KX-K2FU3HUSX2-KE37RR548K-Z8JF";
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _ = DataEntity.InformationResource;
            _ = DataEntity.WarningResource;
            _ = DataEntity.ErrorResource;
            services.AddControllers().AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK";
                });

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
            services.AddSingleton<IRabbitManager, RabbitManager>();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DataContext"), sqlOptions =>
                {
                    sqlOptions.AddTempTableSupport();
                });
                options.AddInterceptors(new HintCommandInterceptor());
            });

            EntityFrameworkManager.ContextFactory = context =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DataContext"), sqlOptions =>
                {
                    sqlOptions.AddTempTableSupport();
                });
                DataContext DataContext = new DataContext(optionsBuilder.Options);
                return DataContext;
            };

            var settings = new ConnectionSettings(new Uri(Configuration["ElasticConfig:Hostname"]))
                           .BasicAuthentication(Configuration["ElasticConfig:Username"], Configuration["ElasticConfig:Password"])
                           .DefaultFieldNameInferrer(p => p)
                           .DisableDirectStreaming();
            IElasticClient ElasticClient = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(ElasticClient);

            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("IgnoreExtraElementsConvention", pack, t => true);

            Assembly[] assemblies = new[] {
                Assembly.GetAssembly(typeof(IServiceScoped)),
                Assembly.GetAssembly(typeof(Startup))
            };
            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IServiceScoped>())
                     .AsImplementedInterfaces()
                     .WithScopedLifetime());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["Token"];
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKeyResolver = (token, secutiryToken, kid, validationParameters) =>
                    {
                        var secretKey = Configuration["Config:SecretKey"];
                        var key = Encoding.ASCII.GetBytes(secretKey);
                        SecurityKey issuerSigningKey = new SymmetricSecurityKey(key);
                        return new List<SecurityKey>() { issuerSigningKey };
                    }
                };
            });

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", policy =>
                    policy.Requirements.Add(new PermissionRequirement()));
            });

            services.AddScoped<IAuthorizationHandler, RpcSimpleHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RpcSimple", policy =>
                    policy.Requirements.Add(new RpcSimpleRequirement()));
            });

            services.AddScoped<IAuthorizationHandler, SimpleHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Simple", policy =>
                    policy.Requirements.Add(new SimpleRequirement()));
            });


            Action onChange = () =>
            {
                InternalServices.UTILS = Configuration["InternalServices:UTILS"];
                InternalServices.ES = Configuration["InternalServices:ES"];
                InternalServices.Converter = Configuration["InternalServices:Converter"];
            };
            onChange();
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), onChange);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "rpc/dms-abe/swagger/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/rpc/dms-abe/swagger/v1/swagger.json", "dms abe API");
                c.RoutePrefix = "rpc/dms-abe/swagger";
            });
            app.UseDeveloperExceptionPage();
        }
    }
}
