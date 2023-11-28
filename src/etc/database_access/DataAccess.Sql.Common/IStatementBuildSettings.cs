using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Sql.Common
{    
    public interface IStatementBuildSettings
    {
        string ParameterPrefixSign { get; }
        string LastInsertedIdFunctionCall { get; }
        string EqualityComparisonSign { get; }
    }
}