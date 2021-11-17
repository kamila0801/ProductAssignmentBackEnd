using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductAssignment.Core;
using ProductAssignment.Core.Security;
using ProductAssignment.DataAccess;
using ProductAssignment.DataAccess.Entities;
using ProductAssignment.DataAccess.Repositories;
using ProductAssignment.DataAccess.Test;
using ProductAssignment.Domain.IRepositories;
using ProductAssignment.Domain.Services;
using ProductAssignment.Security;

namespace ProductAssignment.WebApi
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
            Byte[] secretBytes = new byte[40];
            // Create a byte array with random values. This byte array is used
            // to generate a key for signing JWT tokens.
            using (var rngCsp = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(secretBytes);
            }

            //Add JWT authentication
            //The settings below match the settings when we create our TOKEN:
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    //ValidAudience = "CoMetaApiClient",
                    ValidateIssuer = false,
                    //ValidIssuer = "CoMetaApi",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                    ValidateLifetime = true, //validate the expiration and not before values in the token
                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ProductAssignment.WebApi", Version = "v1"});
            });
            //setting up Dependency Injection
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            //setting DB Information
            //explain what database we want
            services.AddDbContext<MainDbContext>(options =>
            {
                options.UseSqlite("Data Source=main.db");
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Dev-cors", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            
            services.AddDbContext<SecurityContext>(opt => opt.UseInMemoryDatabase("Security"));
            //services.AddTransient<ISecurityContextInitializer, SecurityMemoryInitializer>();
            
            /*
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("OwnerPolicy",
                    policy => { policy.Requirements.Add(new ResourceOwnerRequirement()); });
            });
            services
                .AddSingleton<IAuthorizationHandler,
                    UserResourceOwnerAuthorizationService>(); //Adding the handler for the "OwnerPolicy"
            */

            //I add the Authentication helper as a SINGLETON that uses the SECRET symmetric key:
            //The key is used for digitally signing the JWT tokens - keeping them secure from tampering
            //The SINGLETON is to ensure that we are using the same authenticator, with the same SECRET:
            services.AddSingleton<IAuthenticationHelper>(new AuthenticationHelper(secretBytes));
            services.AddScoped<IUserAuthenticator, UserAuthenticator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MainDbContext context, SecurityContext securityContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductAssignment.WebApi v1"));
                app.UseCors("Dev-cors");
                new DbSeeder(context).SeedDevelopment();
                new SecurityMemoryInitializer().Initialize(securityContext);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}