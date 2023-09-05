using Microsoft.Extensions.Options;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<SampleService>();

// v1 -> weryfikacja w serwisach
// builder.Services.Configure<ServiceConfiguration>
//     (builder.Configuration.GetSection(nameof(ServiceConfiguration)));

// v2
// builder.Services.AddOptions<ServiceConfiguration>()
//     .Configure<IConfiguration>((serviceConf, configuration) =>
//     {
//         var configSection = configuration.GetSection(nameof(ServiceConfiguration));
//         configSection.Bind(serviceConf);
//         
//         // walidacja ponizej...
//
//         if (string.IsNullOrEmpty(serviceConf.ApiKey))
//         {
//             throw new ArgumentNullException("ServiceConf ApiKey is missing");
//         }
//     });

// v3
// builder.Services.AddOptions<ServiceConfiguration>()
//     .Bind(builder.Configuration.GetSection(nameof(ServiceConfiguration)))
//     .ValidateDataAnnotations() // walidacja na podstawie adnotacji na modelu
//     .Validate((configuration) => configuration.LowestPriority < configuration.HighestPriority,
//         "Heighest priority should be higher than lower priority!!!");

//v4
builder.Services.AddOptions<ServiceConfiguration>()
    .Bind(builder.Configuration.GetSection(nameof(ServiceConfiguration)))
    .ValidateOnStart(); // na start apki, a nie podczas trwania requesta

builder.Services.AddSingleton<IValidateOptions<ServiceConfiguration>, ServiceConfigurationValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();