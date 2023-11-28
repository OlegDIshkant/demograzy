using DataAccess.Sql.Common;

namespace DataAccess.Sql.PostgreSql
{    
    public struct StatementBuildSettings : IStatementBuildSettings
    {
        public string ParameterPrefixSign => "@";

        public string LastInsertedIdFunctionCall => "LASTVAL()";

        public string EqualityComparisonSign => "=";
    }
}