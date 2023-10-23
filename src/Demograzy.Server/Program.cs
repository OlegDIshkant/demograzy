// Create app logic object
var actionsFactory = new Demograzy.BusinessLogic.TsFactory(
    new Demograzy.DataAccess.TransactionMeansFactory(
        new Demograzy.DataAccess.BaseConnectionStringProvider("/etc/demograzy/db_connection_string")));

// Create server
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
