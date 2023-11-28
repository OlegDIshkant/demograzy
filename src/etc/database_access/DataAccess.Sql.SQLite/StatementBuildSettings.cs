using DataAccess.Sql.Common;

namespace DataAccess.Sql.SQLite
{    
    public struct StatementBuildSettings : IStatementBuildSettings
    {
        public string ParameterPrefixSign => "$";

        public string LastInsertedIdFunctionCall => "last_insert_rowid()";

        public string EqualityComparisonSign => "==";
    }
}