#if POSTGRESQL

using System.Diagnostics;
using DataAccess.Sql.PostgreSql;
using Demograzy.BusinessLogic;
using SQLitePCL;
using SQLiteCommandBuilderFactory = DataAccess.Sql.PostgreSql.SqlCommandBuilderFactory;

namespace Demograzy.Core.Test.CommonRoutines
{
    internal static class PostgresqlMainServiceProvider
    {

        internal static MainService Provide()
        {
            var testableService = 
                new Demograzy.BusinessLogic.MainService(
                    new Demograzy.DataAccess.Sql.TransactionMeansFactory(
                       new SQLiteCommandBuilderFactory(
                        new BaseConnectionStringProvider("/etc/demograzy_dev/pg_connection_string"))));


            return testableService;
        }

        


    }
}
#endif