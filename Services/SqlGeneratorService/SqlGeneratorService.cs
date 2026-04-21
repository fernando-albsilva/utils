using System.Data;
using System.Globalization;

namespace Services.SqlGeneratorService
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
            Dictionary<string, ColumnDefinition> columns,
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
                var values = lines[i].Replace("\"","").Split(';');

                var formattedValues = new List<string>();

                for (int j = 0; j < columnNames.Count; j++)
                {
                    var column = columnNames[j];
                    var dbType = columns[column].DatabaseType;
                    var customType = columns[column].CustomSqlType;

                    ValidateType(columns[column], columnNames[j]);

                    string formattedValue = "";

                    if (dbType is not null)
                    {
                        var value = values[j];
                        formattedValue = FormatValue(value, dbType);
                    }

                    if (customType is not null)
                    {
                        var value = columns[column].CustomValue; 
                        formattedValue = FormatCustomValue(value, customType);
                    }

                    if (string.IsNullOrEmpty(formattedValue))
                    {
                        ConsoleService.WriteError($"Não foi possivel recuperar valor na coluna {column}");
                        throw new InvalidOperationException();
                    }


                    formattedValues.Add(formattedValue);
                }

                var sql = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", formattedValues)});";
                sqlLines.Add(sql);
            }

            return string.Join(Environment.NewLine, sqlLines);
        }

       

        private static void ValidateType(ColumnDefinition columnDefinition, string columnName)
        {
            var error = false;
            
            if (columnDefinition.DatabaseType is null && columnDefinition.CustomSqlType is null)
            {
                ConsoleService.WriteError($"Não foi possível recuperar tipo na coluna {columnName}");
                error = true;
            }

            if (columnDefinition.DatabaseType is not null && columnDefinition.CustomSqlType is not null)
            {
                ConsoleService.WriteError($"Não foi possível recuperar tipo na coluna {columnName}");
                error = true;
            }

            if (error)
            {
                throw new InvalidOperationException();
            }
        }

        private static string FormatValue(string value, DbType? type)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "NULL";

            return type switch
            {
                DbType.String => FormatString(value),
                DbType.Int16 => FormatInt16(value),
                DbType.Decimal => FormatDecimal(value),
                DbType.Date => FormatDate(value),
                DbType.DateTime => FormatDateTime(value),
                _ => FormatDefault(value)
            };
        }

        private static string FormatCustomValue(string value, string? customType)
        {
            return FormatDefault(value);
        }

        static string FormatString(string value)
        {
            var sanitized = value.Replace("'", "").Replace("\"", "");
            return $"'{sanitized}'";
        }

        private static string FormatInt16(string value)
        {
            return value;
        }

        private static string FormatDecimal(string value)
        {
            return decimal
                .Parse(value, CultureInfo.InvariantCulture)
                .ToString(CultureInfo.InvariantCulture);
        }

        private static string FormatDate(string value)
        {
            var date = DateTime.Parse(value);
            return $"'{date:yyyy-MM-dd}'";
        }

        private static string FormatDateTime(string value)
        {
            var date = DateTime.Parse(value);
            return $"'{date:yyyy-MM-dd HH:mm:ss}'";
        }

        private static string FormatDefault(string value)
        {
            return value;
        }
    }
}
