// Create app logic object
var actionsFactory = new Demograzy.BusinessLogic.TsFactory(
    new Demograzy.DataAccess.Sql.TransactionMeansFactory(
        new Demograzy.DataAccess.Sql.BaseConnectionStringProvider("/etc/demograzy/db_connection_string")));

// Create server
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
