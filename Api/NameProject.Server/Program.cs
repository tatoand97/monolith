using Common.Presentation.Config;
using NameProject.Server.Configs;
using NameProject.Server.ServiceCollections;
using Wolverine;
using Wolverine.FluentValidation;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddModulesConfiguration();

builder.ConfigureAppConfiguration();

builder.Host.UseSerilogCustom();

builder.Host.UseWolverine(options =>
{
    options.UseFluentValidation(RegistrationBehavior.ExplicitRegistration);
});

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