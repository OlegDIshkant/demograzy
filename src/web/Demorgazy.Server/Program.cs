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

app.MapGet(
    "/create_client_screen.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "create_client_screen.js");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "application/javascript");
    });

app.MapGet(
    "/client_menu_screen.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client_menu_screen.js");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "application/javascript");
    });

app.MapGet(
    "/create_room_screen.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "create_room_screen.js");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "application/javascript");
    });

app.MapGet(
    "/create_room_fail_screen.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "create_room_fail_screen.js");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "application/javascript");
    });

app.MapGet(
    "/create_client_failed_screen.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "create_client_failed_screen.js");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "application/javascript");
    });

app.MapGet(
    "/create_candidate_screen.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "create_candidate_screen.js");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "application/javascript");
    });

app.MapGet(
    "/room_lobby_screen.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "room_lobby_screen.js");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "application/javascript");
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

app.MapPut(
    "/client/{clientId}/room/new",
    async (int clientId, HttpContext context) =>
    {
        if (context.Request.Query.TryGetValue("title", out var title) &&
            context.Request.Query.TryGetValue("passphrase", out var passphrase))
        {
            var roomId = await demograzyService.AddRoomAsync(clientId, title, passphrase);
            
            if (roomId.HasValue)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 201; 
                await context.Response.WriteAsync(JsonSerializer.Serialize(roomId));
                return;
            }
        }

        context.Response.StatusCode = 400;
        

    });

app.MapGet(
    "/room/{roomId}/members",
    async (int roomId, HttpContext context) =>
    {        
        var memberIds = await demograzyService.GetMembers(roomId);

        if (memberIds != null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200; 
            await context.Response.WriteAsync(JsonSerializer.Serialize(memberIds));
            return;
        }

        context.Response.StatusCode = 400;
    });

app.MapGet(
    "/room/{roomId}/candidates",
    async (int roomId, HttpContext context) =>
    {        
        var candidateIds = await demograzyService.GetCandidatesAsync(roomId);

        if (candidateIds != null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200; 
            await context.Response.WriteAsync(JsonSerializer.Serialize(candidateIds));
            return;
        }

        context.Response.StatusCode = 400;
    });

app.MapPost(
    "/room/{roomId}/start_voting",
    async (int roomId, HttpContext context) =>
    {        
        if (await demograzyService.StartVotingAsync(roomId))
        {
            context.Response.StatusCode = 200; 
        }
        else
        {
            context.Response.StatusCode = 400;
        }

    });
    
app.MapPut(
    "/room/{roomId}/candidates/new",
    async (int roomId, HttpContext context) =>
    {

        if (context.Request.Query.TryGetValue("name", out var name))
        {
            var candidateId = await demograzyService.AddCandidateAsync(roomId, name);
            
            if (candidateId.HasValue)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 201; 
                await context.Response.WriteAsync(JsonSerializer.Serialize(candidateId));
                return;
            }
        }

        context.Response.StatusCode = 400;
        

    });

app.Run();
