using Services.SqlGeneratorService;
using System.Data;

namespace CsvToSqlInsert
{
    public class ConfigArg
    {
        public string InputCsvPath { get; set; } = "C\\Repositorios";
        public string InputCsvFileName { get; set; } = "input-relatorio.csv";
        public string OutputCsvPath { get; set; } = "C\\Repositorios";
        public string OutputCsvFileName { get; set; } = "output-relatorio.sql";

        public string TableName { get; set; } = "PERSON";
        public Dictionary<string, ColumnDefinition> Collumns { get; set; } =
            new()
            {
                { "Name", new () { DatabaseType  = DbType.String } },
                { "Age", new () { DatabaseType  = DbType.Int16 } },
                { "Birth", new () { DatabaseType  = DbType.DateTime } },
                { "Graduation", new () { DatabaseType  = DbType.Date } },
                { "Salary", new () { DatabaseType  = DbType.Decimal } },
                { "Creation", new () { CustomSqlType  = "DateTimeNow", CustomValue = "CURRENT_TIMESTAMP" } },
            };

    }
}
