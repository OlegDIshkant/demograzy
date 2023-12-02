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

app.MapGet(
    "/vote_screen.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "vote_screen.js");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "application/javascript");
    });

app.MapGet(
    "/winner_screen.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "winner_screen.js");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "application/javascript");
    });

app.MapGet(
    "/join_room_screen.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "join_room_screen.js");
        var fileContent = System.IO.File.ReadAllText(filePath);
        return Results.Content(fileContent, "application/javascript");
    });

app.MapGet(
    "/requests.js", 
    () => 
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "requests.js");
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

app.MapGet(
    "/room/{roomId}/voting_started",
    async (int roomId, HttpContext context) =>
    {        
        var roomInfo = await demograzyService.GetRoomInfoAsync(roomId);


        if (roomInfo.HasValue)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200; 
            await context.Response.WriteAsync(JsonSerializer.Serialize(roomInfo.Value.votingStarted));
        }
        else
        {
            context.Response.StatusCode = 400;
        }

    });

app.MapPut(
    "/room/{roomId}/members/new",
    async (int roomId, HttpContext context) =>
    {        
        if (context.Request.Query.TryGetValue("memberId", out var memberIdString) &&
            int.TryParse(memberIdString, out var memberId) &&
            await demograzyService.AddMember(roomId, memberId))
        {
            context.Response.StatusCode = 200; 
        }
        else
        {
            context.Response.StatusCode = 400;
        }        
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

app.MapGet(
    "/room/{roomId}/winner",
    async (int roomId, HttpContext context) =>
    {        
        var winnerId = await demograzyService.GetWinnerAsync(roomId);
        
        if (winnerId.HasValue)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200; 
            await context.Response.WriteAsync(JsonSerializer.Serialize(winnerId.Value));
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
    
app.MapGet(
    "/versus/{versusId}/candidates",
    async (int versusId, HttpContext context) =>
    {
        var versusInfo = await demograzyService.GetVersusInfoAsync(versusId);
        
        if (versusInfo.HasValue)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200; 
            var candidates = new List<int>()
            {
                versusInfo.Value.firstCandidateId,
                versusInfo.Value.secondCandidateId
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(candidates));
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    });
    
app.MapPost(
    "/versus/{versusId}/vote",
    async (int versusId, HttpContext context) =>
    {
        if (context.Request.Query.TryGetValue("voter", out var voterIdString) &&
            context.Request.Query.TryGetValue("voteForFirst", out var voteForFirstString) &&
            int.TryParse(voterIdString, out var voterId) &&
            bool.TryParse(voteForFirstString, out var voteForFirst) &&
            await demograzyService.VoteAsync(versusId, voterId, voteForFirst))
        {
            context.Response.StatusCode = 200;
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    });


app.MapGet(
    "/candidate/{candidateId}/name",
    async (int candidateId, HttpContext context) =>
    {        
        var candidateInfo = await demograzyService.GetCandidateInfo(candidateId);
        
        if (candidateInfo.HasValue)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200; 
            await context.Response.WriteAsync(candidateInfo.Value.name);
        }
        else
        {
            context.Response.StatusCode = 400;
        }

    });

app.MapGet(
    "/room/{roomId}/members/{clientId}/active_verses",
    async (int roomId, int clientId, HttpContext context) =>
    {        
        var memberIds = await demograzyService.GetActiveVersesAsync(roomId, clientId);

        if (memberIds != null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200; 
            await context.Response.WriteAsync(JsonSerializer.Serialize(memberIds));
            return;
        }

        context.Response.StatusCode = 400;
    });

app.Run();
