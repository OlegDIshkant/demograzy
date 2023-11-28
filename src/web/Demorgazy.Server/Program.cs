// Create app logic object
using System.Text.Json;

var demograzyService = new Demograzy.BusinessLogic.MainService(
    new Demograzy.DataAccess.Sql.TransactionMeansFactory(
        new DataAccess.Sql.PostgreSql.SqlCommandBuilderFactory(
            new DataAccess.Sql.PostgreSql.BaseConnectionStringProvider("/etc/demograzy/db_connection_string"))));

// Create server
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet(
    "/", 
    () =>
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "text/html");

    });

app.MapPut(
    "/client/new", 
    async (context) =>
    {
        if (context.Request.Query.TryGetValue("name", out var name))
        {
            var clientId = await demograzyService.AddClientAsync(name);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 201; 
            await context.Response.WriteAsync(JsonSerializer.Serialize(clientId));
        }
        else
        {
            context.Response.StatusCode = 400;
        }

    });

app.Run();
