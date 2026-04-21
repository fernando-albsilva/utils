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
        public Dictionary<string, DbType> Collumns { get; set; } =
            new()
            {
                { "Name", DbType.String },
                { "Age", DbType.Int16 },
                { "Birth", DbType.DateTime },
                { "Graduation", DbType.Date },
                { "Salary", DbType.Decimal }
            };

    }
}
