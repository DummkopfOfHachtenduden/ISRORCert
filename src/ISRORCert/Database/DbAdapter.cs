using Microsoft.Extensions.Logging;

using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace ISRORCert.Database
{
    internal abstract class DbAdapter : IDbAdapter
    {
        // TODO: Create param0, param1, param2 variants of params stuff while params Span<T> is not ready.
        // TODO: Replace params with params Span<T> (see https://github.com/dotnet/csharplang/issues/1757)

        private readonly ILogger _logger;

        public string ConnectionString { get; set; } = string.Empty;

        private readonly DbProviderFactory _factory;

        protected DbAdapter(ILogger logger, DbProviderFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        protected DbAdapter(ILogger logger, DbProviderFactory factory, string connectionString)
        {
            _logger = logger;
            _factory = factory;
            ConnectionString = connectionString;
        }

        private DbConnection GetConnection()
        {
            var connection = _factory.CreateConnection();
            ArgumentNullException.ThrowIfNull(connection);

            connection.ConnectionString = ConnectionString;
            connection.Open();
            return connection;
        }

        private async Task<DbConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            var connection = _factory.CreateConnection();
            ArgumentNullException.ThrowIfNull(connection);

            connection.ConnectionString = ConnectionString;
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            return connection;
        }

        private DbCommand GetCommand(DbConnection connection, string cmdText, params DbParameter[] parameters)
        {
            var command = _factory.CreateCommand();
            ArgumentNullException.ThrowIfNull(command);

            command.Connection = connection;
            command.CommandText = cmdText;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);
            return command;
        }

        public DbParameter GetReturnParameter(DbType type)
        {
            var parameter = _factory.CreateParameter();
            ArgumentNullException.ThrowIfNull(parameter);

            parameter.Direction = ParameterDirection.ReturnValue;
            parameter.DbType = type;
            return parameter;
        }

        public DbParameter GetInputParameter(string name, object? inputValue)
        {
            var parameter = _factory.CreateParameter();
            ArgumentNullException.ThrowIfNull(parameter);

            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = name;
            parameter.Value = inputValue/*?? DBNull.Value*/;
            return parameter;
        }

        public DbParameter GetOutputParameter(string paramName, DbType outputType)
        {
            var parameter = _factory.CreateParameter();
            ArgumentNullException.ThrowIfNull(parameter);

            parameter.Direction = ParameterDirection.Output;
            parameter.ParameterName = paramName;
            parameter.DbType = outputType;
            return parameter;
        }

        public DbParameter GetInputOutputParameter(string paramName, object? inputValue, DbType inputOutputType)
        {
            var parameter = _factory.CreateParameter();
            ArgumentNullException.ThrowIfNull(parameter);

            parameter.Direction = ParameterDirection.InputOutput;
            parameter.ParameterName = paramName;
            parameter.DbType = inputOutputType;
            parameter.Value = inputValue;
            return parameter;
        }

        public int Execute(string cmdText, params DbParameter[] parameters)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = GetCommand(connection, cmdText, parameters))
                    return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbAdapter error");
                return -1;
            }
        }

        public T? GetScalar<T>(string cmdText, params DbParameter[] parameters) where T : unmanaged
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = GetCommand(connection, cmdText, parameters))
                    return command.ExecuteScalar() is T scalar ? scalar : default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbAdapter error");
                return default;
            }
        }

        public T? GetData<T>(string cmdText, params DbParameter[] parameters) where T : class, IDbEntity, new()
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = GetCommand(connection, cmdText, parameters))
                using (var reader = command.ExecuteReader())
                    return DbHelper.ReadData<T>(reader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbAdapter error");
                return null;
            }
        }

        public IEnumerable<T>? GetDataTable<T>(string cmdText, params DbParameter[] parameters)
            where T : class, IDbEntity, new()
        {
            var collection = new List<T>();
            if (!GetDataTable(collection, cmdText, parameters))
                return null;

            return collection;
        }

        public bool GetDataTable<T>(ICollection<T> collection, string cmdText, params DbParameter[] parameters)
            where T : class, IDbEntity, new()
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = GetCommand(connection, cmdText, parameters))
                using (var reader = command.ExecuteReader())
                    DbHelper.ReadDataTable(reader, collection);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbAdapter error");
                return false;
            }
        }

        public async Task<int> ExecuteAsync(
            string cmdText,
            CancellationToken cancellationToken = default,
            params DbParameter[] parameters)
        {
            try
            {
                using (var connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false))
                using (var command = GetCommand(connection, cmdText, parameters))
                    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbAdapter error");
                return -1;
            }
        }

        public async Task<T?> GetScalarAsync<T>(
            string cmdText,
            CancellationToken cancellationToken = default,
            params DbParameter[] parameters)
            where T : unmanaged
        {
            try
            {
                using (var connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false))
                using (var command = GetCommand(connection, cmdText, parameters))
                    return await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false) is T actual
                        ? actual
                        : default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbAdapter error");
                return default;
            }
        }

        public async Task<T?> GetDataAsync<T>(
            string cmdText,
            CancellationToken cancellationToken = default,
            params DbParameter[] parameters)
            where T : class, IDbEntity, new()
        {
            try
            {
                using (var connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false))
                using (var command = GetCommand(connection, cmdText, parameters))
                using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                    return await DbHelper.ReadDataAsync<T>(reader, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbAdapter error");
                return null;
            }
        }

        public async Task<IList<T>?> GetDataTableAsync<T>(
            string cmdText,
            CancellationToken cancellationToken = default,
            params DbParameter[] parameters)
            where T : class, IDbEntity, new()
        {
            var collection = new List<T>();
            if (!await GetDataTableAsync(collection, cmdText, cancellationToken, parameters).ConfigureAwait(false))
                return null;

            return collection;
        }

        public async Task<bool> GetDataTableAsync<T>(
            ICollection<T> collection,
            string cmdText,
            CancellationToken cancellationToken = default,
            params DbParameter[] parameters)
            where T : class, IDbEntity, new()
        {
            try
            {
                using (var connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false))
                using (var command = GetCommand(connection, cmdText, parameters))
                using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                    await DbHelper.ReadDataTableAsync(reader, collection, cancellationToken)
                        .ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbAdapter error");
                return false;
            }
        }
    }
}
