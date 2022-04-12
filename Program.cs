using Carter.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MyCarterApp.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDatabase, Database>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyCarterApp API",
        Version = "v1",
        Description = ""
    });

    options.DocInclusionPredicate((s, description) =>
    {
        foreach (var metaData in description.ActionDescriptor.EndpointMetadata)
        {
            if (metaData is IIncludeOpenApi)
            {
                return true;
            }
        }
        return false;
    });
});

builder.Services.AddCarter();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapCarter();

app.Run();