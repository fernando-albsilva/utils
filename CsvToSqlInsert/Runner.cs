using Services;

namespace CsvToSqlInsert
{
    public class Runner
    {
        private readonly JsonService JsonService = new JsonService();

        private ConfigArg Configuration = new ConfigArg();
        private static string ConfigFilePath => AppContext.BaseDirectory;
        private static string ConfigFileName => $"{nameof(ConfigArg)}.json";

        public void Run()
        {

            ConsoleService.WriteHeader("Csv-To-Sql-Insert");

            ConsoleService.WriteInfo($"Verificando existência de arquivo de configuração {ConfigFileName} no diretório {ConfigFilePath}");

            if (!ExistConfigFile())
            {
                ConsoleService.WriteInfo("Arquivo não encontrado");
                ConsoleService.WriteInfo($"Criando arquivo de configuração {ConfigFileName} com informações padrões no diretório {ConfigFilePath}");

                CreateConfigFile();
            
            } else
            {
                ConsoleService.WriteInfo($"Arquivo encontrado");
                ConsoleService.WriteInfo($"Caregando arquivo de configuração");
                SetConfiguration();            
            }

            PrintConfig();

            var userChoice = AskForContinue();

            ValidateUserChoice(userChoice);

            Generate();

            ConsoleService.WriteSuccess("Arquivo de script gerado com sucesso");
            ConsoleService.WriteInfo("Encerrando ...");
        }

        private bool ExistConfigFile()
        {
            return JsonService.ExistJsonFile(ConfigFilePath, nameof(ConfigArg));
        }

        private void CreateConfigFile()
        {
            JsonService.CreateJsonFile(ConfigFilePath, nameof(ConfigArg), Configuration);
        }

        private void PrintConfig()
        {
            ConsoleService.WriteSubHeader("Configurações carregadas");
            ConsoleService.WriteLine($"{nameof(ConfigArg.InputCsvPath)}:  {Configuration.InputCsvPath}");
            ConsoleService.WriteLine();
            ConsoleService.WriteLine($"{nameof(ConfigArg.InputCsvFileName)}: {Configuration.InputCsvFileName}");
            ConsoleService.WriteLine();
            ConsoleService.WriteLine($"{nameof(ConfigArg.OutputCsvPath)}: {Configuration.OutputCsvPath}");
            ConsoleService.WriteLine();
            ConsoleService.WriteLine($"{nameof(ConfigArg.OutputCsvFileName)}: {Configuration.OutputCsvFileName}");
            ConsoleService.WriteLine();
        }

        private string AskForContinue()
        {
            return ConsoleService.AskForInput("Tecle (1) para executar ou qualquer tecla para encerrar");
        }


        private void SetConfiguration()
        {
            Configuration = JsonService.ReadJsonFile<ConfigArg>(ConfigFilePath, nameof(ConfigArg))!;
        }

        private void ValidateUserChoice(string userChoice)
        {
           if (userChoice != "1")
            {
                ConsoleService.WriteInfo("Encerrando ...");
                Environment.Exit(0);
            }
        }

        private void Generate()
        {
            ConsoleService.WriteInfo($"Gerando script de inserts baseado no csv");
            var sql = 
                SqlGeneratorService.GenerateInsertFromCsv(
                    Configuration.TableName,
                    Configuration.Collumns,
                    Configuration.InputCsvPath,
                    Configuration.InputCsvFileName
                );

            ConsoleService.WriteInfo($"Escrevendo script gerado em {Path.Combine(Configuration.OutputCsvPath, Configuration.OutputCsvFileName)}");

            FileService.WriteFile(Configuration.OutputCsvPath, Configuration.OutputCsvFileName, sql);
        }
    }
}
