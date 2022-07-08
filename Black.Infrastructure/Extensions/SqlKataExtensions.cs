using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black.Infrastructure.Extensions;

public static class SqlKataExtensions
{
    /// <summary>
    /// Converts sqlkata query type to string type.
    /// </summary>
    /// <param name="query">Query</param>
    public static string ToQueryString(this Query query)
    {
        var compiler = new SqlServerCompiler();
        SqlResult compilerResult = compiler.Compile(query);
        string sqlQuery = compilerResult.Sql;
        return sqlQuery;
    }
}
