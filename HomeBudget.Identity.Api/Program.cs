using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using HomeBudget.Components.Users.Configuration;
using HomeBudget.Identity.Api.Configuration;
using HomeBudget.Identity.Api.Extensions;
using HomeBudget.Identity.Domain.Configuration;

var webAppBuilder = WebApplication.CreateBuilder(args);

var services = webAppBuilder.Services;
var environment = webAppBuilder.Environment;

services
    .RegisterApiIoCDependency()
    .RegisterUsersIoCDependency();

var configuration = webAppBuilder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
    .Build();

services.AddControllers();
services.AddJwt(configuration);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.SetUpDi(configuration);

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity", Version = "v1" });
});

var app = webAppBuilder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User identity V1");
});

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync().ConfigureAwait(false);