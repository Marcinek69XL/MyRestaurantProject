using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyRestaurantProject;
using MyRestaurantProject.Authorization;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Middleware;
using MyRestaurantProject.Models;
using MyRestaurantProject.Models.Validators;
using MyRestaurantProject.Services;
using MyRestaurantProject.Settings;
using NLog.Web;

var builder = WebApplication.CreateBuilder();

// configure services

// NLOG
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var authSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authSettings);
builder.Services.AddSingleton(authSettings);

builder.Services.AddAuthentication(options =>
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

builder.Services.AddAuthorization(options =>
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

builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinimumRestaurantCreatedHandler>();
builder.Services.AddDbContext<RestaurantDbContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();
builder.Services.AddScoped<RestaurantSeeder>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policyBuilder =>
    {
        policyBuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration["AllowedOrigins"]); //adres apki frontendowej, w configu zapisane
    });
});

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddControllers().AddJsonOptions(options => //Enumy jako string w swagerze
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// end configurating services

var app = builder.Build();

// configure HTTP pipeline

app.UseResponseCaching(); //celowo jako pierwszy
app.UseStaticFiles();

var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<RestaurantSeeder>().Seed();

if (app.Environment.IsDevelopment())
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

// END CONFIGURATING HTTP PIPELINE

app.Run();