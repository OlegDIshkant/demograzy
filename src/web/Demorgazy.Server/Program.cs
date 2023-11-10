// Create app logic object
var demograzyService = new Demograzy.BusinessLogic.MainService(
    new Demograzy.DataAccess.Sql.TransactionMeansFactory(
        new DataAccess.Sql.PostgreSql.SqlCommandBuilderFactory(
            new DataAccess.Sql.PostgreSql.BaseConnectionStringProvider("/etc/demograzy/db_connection_string"))));

// Create server
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
