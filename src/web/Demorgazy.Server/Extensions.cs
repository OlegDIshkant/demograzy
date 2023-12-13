using Sql = DataAccess.Sql.PostgreSql;


namespace Demograzy.Server
{
    internal static class Extensions
    {
        static public void AddDemograzyService(this IServiceCollection services, string pathToConnectionStringFile)
        {
            if (pathToConnectionStringFile == null ||
                pathToConnectionStringFile.Length <= 0)
            {
                throw new ArgumentException("No path to database connection string file.");
            }

            services.AddSingleton(
                new Demograzy.BusinessLogic.MainService(
                    new Demograzy.DataAccess.Sql.TransactionMeansFactory(
                        new Sql.SqlCommandBuilderFactory(
                            new Sql.BaseConnectionStringProvider(pathToConnectionStringFile))))
            );
        }
    }
}