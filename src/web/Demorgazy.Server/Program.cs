using Demograzy.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDemograzyService(builder.Configuration["path_to_connection_string_file"]);
builder.Services.AddControllers();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
