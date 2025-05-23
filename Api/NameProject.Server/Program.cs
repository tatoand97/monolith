using NameProject.Server.Configs;
using NameProject.Server.Handlers;
using NameProject.Server.Middlewares;
using NameProject.Server.ServiceCollections;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppConfiguration();

builder.Host.UseSerilogCustom();
builder.Services.AddProblemDetails();
builder.Services.SetupCors();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
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

app.UseMiddleware<LogContextTraceMiddleware>();

app.UseAzureAppConfiguration();



await app.RunAsync();