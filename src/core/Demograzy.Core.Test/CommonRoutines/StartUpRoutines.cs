using System.Diagnostics;
using DataAccess.Sql.SQLite;
using Demograzy.BusinessLogic;
using SQLitePCL;
using SQLiteCommandBuilderFactory = DataAccess.Sql.SQLite.SqlCommandBuilderFactory;

namespace Demograzy.Core.Test.CommonRoutines
{
    internal static class StartUpRoutines
    {

        internal static MainService PrepareMainService()
        {
#if SQLITE
            return SqliteMainServiceProvider.Provide();
#elif POSTGRESQL
            return PostgresqlMainServiceProvider.Provide();
#endif
            throw new NotImplementedException();
        }
        


    }
}