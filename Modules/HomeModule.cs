namespace MyCarterApp.Modules;

using Carter.ModelBinding;
using Carter.Request;
using Carter.Response;
using FluentValidation;
using Microsoft.AspNetCore.Http;

public class HomeModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => "Hello from Carter!");
        app.MapGet("/qs", (HttpRequest req) =>
         {
             var ids = req.Query.AsMultiple<int>("ids");
             return $"It's {string.Join(", ", ids)}";
         });

        app.MapGet("/conneg", (HttpResponse res) =>
         {
             res.Negotiate(new { name = "Dave" });
         });

        app.MapPost("/validation", HandlePost);
    }

    private IResult HandlePost(HttpContext context, Person person, IDatabase database)
    {
        var result = context.Request.Validate(person);
        if (!result.IsValid)
        {
            return Results.UnprocessableEntity(result.GetFormattedErrors());
        }
        var id = database.StorePerson(person);

        context.Response.Headers.Location = $"/{id}";

        return Results.StatusCode(201);
    }
    
}
