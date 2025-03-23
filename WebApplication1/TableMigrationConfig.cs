namespace WebApplication1
{
    public class TableMigrationConfig
    {
        public string SourceTableName { get; set; } = string.Empty;
        public string TargetTableName { get; set; } = string.Empty;
        public Dictionary<string, string>? ColumnMappings { get; set; }
    }

}
