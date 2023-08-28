using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using MyRestaurantProject.Authorization;
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

            services.AddSingleton(authSettings);
            
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

            services.AddAuthorization(options =>
            {
                // mozna dodac np posiadanie samego claima jak nizej
                // options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality"));
                
                // lub posiadanie claima jak i wartosc claima
                options.AddPolicy("HasNationality", builder => 
                    builder.RequireClaim("Nationality","German","Polish"));
                
                // customowa polityka autoryzacji
                options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
                
                options.AddPolicy("Atleast2RestaurantCreated", 
                    builder => builder.AddRequirements(new MinimumRestaurantCreatedRequirement(2)));
            });

            services.AddScoped<IAuthorizationHandler, MinimumAgeHandler>();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, MinimumRestaurantCreatedHandler>();
            services.AddDbContext<RestaurantDbContext>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
            services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();
            services.AddScoped<RestaurantSeeder>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(Configuration["AllowedOrigins"]); //adres apki frontendowej, w configu zapisane
                });
            });

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddControllers().AddJsonOptions(options => //Enumy jako string w swagerze
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantSeeder seeder)
        {
            app.UseStaticFiles();
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
            app.UseAuthorization(); // pomiedzy UseRouting a UserEndPoints musi byc
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
