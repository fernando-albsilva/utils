
using System.Data;
using System.Globalization;

namespace Services
{
    public class SqlGeneratorService
    {
        /// <summary>
        /// Gera comandos SQL INSERT a partir de um arquivo CSV.
        /// </summary>
        /// <param name="TableName">
        /// Nome da tabela que será utilizada nos comandos INSERT.
        /// </param>
        /// <param name="columns">
        /// Dicionário contendo o nome das colunas e seus respectivos tipos (<see cref="DbType"/>).
        /// A ordem das colunas será utilizada para mapear os valores do CSV.
        /// </param>
        /// <param name="filePath">
        /// Caminho do diretório onde o arquivo CSV está localizado.
        /// </param>
        /// <param name="fileName">
        /// Nome do arquivo CSV que será processado.
        /// </param>
        /// <returns>
        /// Uma string contendo todos os comandos SQL INSERT gerados,
        /// separados por quebra de linha.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// Lançada quando o arquivo CSV não é encontrado no caminho informado.
        /// </exception>
        /// <exception cref="Exception">
        /// Lançada quando o arquivo CSV está vazio ou não contém registros.
        /// </exception>
        public static string GenerateInsertFromCsv(
            string tableName,
            Dictionary<string, DbType> columns,
            string filePath,
            string fileName
         )
        {
            var fullPath = Path.Combine(filePath, fileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Arquivo não encontrado: {fullPath}");

            var lines = File.ReadAllLines(fullPath);

            if (lines.Length <= 1)
                throw new Exception($"Arquivo vazio: {fullPath}");
            

            var columnNames = columns.Keys.ToList();
            var sqlLines = new List<string>();

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(';');

                var formattedValues = new List<string>();

                for (int j = 0; j < columnNames.Count; j++)
                {
                    var column = columnNames[j];
                    var type = columns[column];
                    var value = values[j];

                    formattedValues.Add(FormatValue(value, type));
                }

                var sql = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", formattedValues)});";
                sqlLines.Add(sql);
            }

            return string.Join(Environment.NewLine, sqlLines);
        }

        private static string FormatValue(string value, DbType type)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "NULL";

            return type switch
            {
                DbType.String => $"'{value.Replace("'", "''")}'",
                DbType.Int16 => value,
                DbType.Decimal => decimal.Parse(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture),
                DbType.Date => $"'{DateTime.Parse(value).ToString("yyyy-MM-dd")}'",
                DbType.DateTime => $"'{DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss")}'",
                _ => $"'{value}'"
            };
        }
    }
}
