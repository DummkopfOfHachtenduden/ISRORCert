using System;
using System.Data.Common;
using System.Linq;

namespace ISRORCert.Database
{
    internal static class DbDataReaderExtensions
    {
        public static T? GetNullable<T>(this DbDataReader reader, string columnName)
            where T : struct
        {
            return reader.GetNullable<T>(reader.GetOrdinal(columnName));
        }

        public static T? GetNullable<T>(this DbDataReader reader, int ordinal)
            where T : struct
        {
            if (reader.IsDBNull(ordinal))
                return default;

            return reader.GetFieldValue<T>(ordinal);
        }

        public static string? GetNullableString(this DbDataReader reader, string columnName)
        {
            return reader.GetNullableString(reader.GetOrdinal(columnName));
        }

        public static string? GetNullableString(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetFieldValue<string>(ordinal);
        }

        public static string GetNullableStringAsEmpty(this DbDataReader reader, string columnName)
        {
            return reader.GetNullableStringAsEmpty(reader.GetOrdinal(columnName));
        }

        public static string GetNullableStringAsEmpty(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return string.Empty;

            return reader.GetFieldValue<string>(ordinal);
        }

        public static Task<T?> GetNullableAsync<T>(this DbDataReader reader, string columnName, CancellationToken cancellationToken = default)
            where T : struct
        {
            return reader.GetNullableAsync<T>(reader.GetOrdinal(columnName), cancellationToken);
        }

        public static async Task<T?> GetNullableAsync<T>(this DbDataReader reader, int ordinal, CancellationToken cancellationToken = default)
            where T : struct
        {
            if (await reader.IsDBNullAsync(ordinal, cancellationToken).ConfigureAwait(false))
                return default;

            return await reader.GetFieldValueAsync<T>(ordinal, cancellationToken).ConfigureAwait(false);
        }
    }
}
