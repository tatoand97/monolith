using Common.Presentation.Config;
using NameProject.Server.Configs;
using NameProject.Server.ServiceCollections;
using NameProject.Server.Utils.Validation;
using User.Application.Commands.CreateUser;
using User.Presentation;
using Wolverine;
using Wolverine.FluentValidation;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddModulesConfiguration();

//builder.ConfigureAppConfiguration();

builder.Host.UseSerilogCustom();

builder.Services.SetupUserModule(builder.Configuration);

builder.Host.UseWolverine(options =>
{
    options.Durability.Mode = DurabilityMode.MediatorOnly;
    options.UseFluentValidation(RegistrationBehavior.ExplicitRegistration);
    options.Services.AddSingleton(typeof(IFailureAction<>), typeof(CustomFailureAction<>));
    options.Discovery.IncludeAssembly(User.Application.AssemblyReference.Assembly);
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

app.MapModuleEndpoints();

app.UseRouting();
app.UseCors(CorsConfig.MyPolicy);

app.UseHttpsRedirection();
app.UseExceptionHandler();

app.UseCustomMiddlewares();

//app.UseAzureAppConfiguration();



await app.RunAsync();