using Microsoft.Extensions.Logging;

using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ISRORCert.Database
{
    internal class SqlDbAdapter : DbAdapter
    {
        public static string ToSqlConnectionString(string odbcConnectionString)
        {
            var result = new StringBuilder(256);
            foreach (var argument in odbcConnectionString.Split(';'))
            {
                var kvpArgument = argument.Split('=');
                var key = kvpArgument[0].ToUpperInvariant();
                var value = kvpArgument[1];
                switch (key)
                {
                    case "SERVER":
                        result.Append($"Data Source={value};");
                        break;

                    case "UID":
                        result.Append($"User ID={value};");
                        break;

                    case "PWD":
                        result.Append($"Password={value};");
                        break;

                    case "DATABASE":
                        result.Append($"Initial Catalog={value};");
                        break;
                }
            }
            //result.Append("MultipleActiveResultSets=True");
            return result.ToString();
        }

        public SqlDbAdapter(ILogger<SqlDbAdapter> logger) : base(logger, SqlClientFactory.Instance)
        {
        }

        public SqlDbAdapter(ILogger<SqlDbAdapter> logger, string connectionString) : base(logger, SqlClientFactory.Instance, connectionString)
        {
        }
    }
}
