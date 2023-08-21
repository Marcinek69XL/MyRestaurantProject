using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Text;
using System.Text.Unicode;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Middleware;
using MyRestaurantProject.Models;
using MyRestaurantProject.Models.Validators;
using MyRestaurantProject.Services;
using MyRestaurantProject.Settings;

namespace MyRestaurantProject
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
            var authSettings = new AuthenticationSettings();
            Configuration.GetSection("Authentication").Bind(authSettings);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(ctg =>
            {
                ctg.RequireHttpsMetadata = false;
                ctg.SaveToken = true;
                ctg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = authSettings.JwtIssuer, // kto generuje token, no nasza apka
                    ValidAudience = authSettings.JwtIssuer, // kto uzywa token, tez nasza apka
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.JwtKey))
                };
            });
            
            services.AddDbContext<RestaurantDbContext>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
            services.AddScoped<RestaurantSeeder>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();
            services.AddSwaggerGen();

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantSeeder seeder)
        {
            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantApi");
            });
            
            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
