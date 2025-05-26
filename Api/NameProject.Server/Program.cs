using NameProject.Server.Configs;
using NameProject.Server.Middlewares;
using NameProject.Server.ServiceCollections;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppConfiguration();

builder.Host.UseSerilogCustom();

builder.Services.SetupCors();
builder.Services.SetupExceptions();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration();

builder.Services.ConfigureOpenTelemetry();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseRouting();
app.UseCors(CorsConfig.MyPolicy);

app.UseHttpsRedirection();
app.UseExceptionHandler();

app.UseCustomMiddlewares();

app.UseAzureAppConfiguration();



await app.RunAsync();