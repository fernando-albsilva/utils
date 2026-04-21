using System.Data;

namespace Services.SqlGeneratorService
{
    public class ColumnDefinition
    {
        public DbType? DatabaseType { get; set; }

        public string? CustomSqlType { get; set; }

        public string? CustomValue { get; set; }
    }
}
