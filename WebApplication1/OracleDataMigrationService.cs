using System.Data;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;

namespace WebApplication1
{
    public class OracleDataMigrationService
    {
        private readonly string _db1ConnStr;
        private readonly string _db2ConnStr;

        public OracleDataMigrationService(IConfiguration configuration)
        {
            _db1ConnStr = configuration.GetConnectionString("Db1Connection") ?? throw new ArgumentNullException("Db1Connection");
            _db2ConnStr = configuration.GetConnectionString("Db2Connection") ?? throw new ArgumentNullException("Db2Connection");
        }

        public async Task MigrateTableWithUpsertAsync(string sourceTableName, string targetTableName, string primaryKeyColumn, Dictionary<string, string>? columnMappings = null)
        {
            Console.WriteLine($"➡ Migrating {sourceTableName} ➡ {targetTableName}");

            var dataTable = await GetDataFromSourceDb(sourceTableName);

            if (columnMappings == null)
            {
                columnMappings = dataTable.Columns
                    .Cast<DataColumn>()
                    .ToDictionary(c => c.ColumnName, c => c.ColumnName);
            }

            using var targetConn = new OracleConnection(_db2ConnStr);
            await targetConn.OpenAsync();

            foreach (DataRow row in dataTable.Rows)
            {
                var primaryKeyValue = row[primaryKeyColumn];

                // Check if record exists
                bool exists = await RecordExistsAsync(targetConn, targetTableName, primaryKeyColumn, primaryKeyValue);

                if (exists)
                {
                    // Build update statement
                    var setClause = string.Join(", ", columnMappings.Where(c => c.Key != primaryKeyColumn)
                        .Select(c => $"{c.Value} = :{c.Value}"));

                    var updateSql = $"UPDATE {targetTableName} SET {setClause} WHERE {primaryKeyColumn} = :{primaryKeyColumn}";

                    using var updateCmd = new OracleCommand(updateSql, targetConn);
                    foreach (var mapping in columnMappings.Where(c => c.Key != primaryKeyColumn))
                    {
                        var value = row[mapping.Key];
                        if (mapping.Key.Equals("Active", StringComparison.OrdinalIgnoreCase) && value != DBNull.Value)
                            value = (Convert.ToInt32(value) == 1) ? 1 : 0;

                        updateCmd.Parameters.Add(new OracleParameter($":{mapping.Value}", value ?? DBNull.Value));
                    }
                    updateCmd.Parameters.Add(new OracleParameter($":{primaryKeyColumn}", primaryKeyValue));

                    await updateCmd.ExecuteNonQueryAsync();
                }
                else
                {
                    // Build insert statement
                    var targetColumns = string.Join(",", columnMappings.Values);
                    var parameterNames = string.Join(",", columnMappings.Values.Select(c => $":{c}"));
                    var insertSql = $"INSERT INTO {targetTableName} ({targetColumns}) VALUES ({parameterNames})";

                    using var insertCmd = new OracleCommand(insertSql, targetConn);
                    foreach (var mapping in columnMappings)
                    {
                        object? value = row[mapping.Key];
                        if (mapping.Key.Equals("Active", StringComparison.OrdinalIgnoreCase) && value != DBNull.Value)
                            value = (Convert.ToInt32(value) == 1) ? 1 : 0;

                        insertCmd.Parameters.Add(new OracleParameter($":{mapping.Value}", value ?? DBNull.Value));
                    }
                    await insertCmd.ExecuteNonQueryAsync();
                }
            }

            Console.WriteLine($"✅ Done migrating (upsert) {sourceTableName}: {dataTable.Rows.Count} rows.\n");
        }

        private async Task<bool> RecordExistsAsync(OracleConnection conn, string tableName, string keyColumn, object keyValue)
        {
            using var cmd = new OracleCommand($"SELECT COUNT(1) FROM {tableName} WHERE {keyColumn} = :key", conn);
            cmd.Parameters.Add(new OracleParameter(":key", keyValue));
            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }

        private async Task<DataTable> GetDataFromSourceDb(string sourceTableName)
        {
            using var sourceConn = new OracleConnection(_db1ConnStr);
            await sourceConn.OpenAsync();

            using var cmd = new OracleCommand($"SELECT * FROM {sourceTableName}", sourceConn);
            using var adapter = new OracleDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
}
