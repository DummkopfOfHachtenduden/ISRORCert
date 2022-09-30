using System;
using System.Data.Common;
using System.Linq;

namespace ISRORCert.Database
{
    internal static class DbHelper
    {
        public static T? ReadData<T>(DbDataReader reader)
          where T : class, IDbEntity, new()
        {
            if (!reader.Read())
                return null;

            var entity = new T();
            entity.Read(reader);
            return entity;
        }

        public static async Task<T?> ReadDataAsync<T>(DbDataReader reader, CancellationToken cancellationToken = default)
            where T : class, IDbEntity, new()
        {
            if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                return null;

            var entity = new T();
            entity.Read(reader);
            return entity;
        }

        public static IEnumerable<T> ReadDataTable<T>(DbDataReader reader)
            where T : class, IDbEntity, new()
        {
            while (reader.Read())
            {
                var entity = new T();
                entity.Read(reader);
                yield return entity;
            }
        }

        public static void ReadDataTable<T>(DbDataReader reader, ICollection<T> collection)
            where T : class, IDbEntity, new()
        {
            while (reader.Read())
            {
                var entity = new T();
                entity.Read(reader);
                collection.Add(entity);
            }
        }

        public static async Task ReadDataTableAsync<T>(DbDataReader reader, ICollection<T> collection, CancellationToken cancellationToken = default)
            where T : class, IDbEntity, new()
        {
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                var entity = new T();
                entity.Read(reader);
                collection.Add(entity);
            }
        }

        public static object ToDbNull<T>(T value) => value != null ? value : DBNull.Value;
    }
}
