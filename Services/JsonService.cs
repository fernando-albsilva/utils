using System.Text.Json;

namespace Services;

public class JsonService
{
    private readonly JsonSerializerOptions _options;

    public JsonService()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Cria um novo arquivo JSON no caminho especificado.
    /// Se já existir, sobrescreve.
    /// </summary>
    public string CreateJsonFile(string filePath, string FileName, object data)
    {
        var json = JsonSerializer.Serialize(data, _options);
        var fullPath = CreateFullPath(filePath, FileName);
        ConsoleService.WriteInfo($"Escrevendo no arquivo: {fullPath}");
        File.WriteAllText(fullPath, json);
        return fullPath;
    }


    /// <summary>
    /// Lê um arquivo JSON e desserializa para um objeto.
    /// </summary>
    public T? ReadJsonFile<T>(string filePath, string FileName)
    {
        var fullPath = CreateFullPath(filePath, FileName);
        if (!File.Exists(fullPath))
        {
            ConsoleService.WriteWarning($"Arquivo {FileName}.json não encontrado: {filePath}");
            return default;
        }

        var json = File.ReadAllText(fullPath);
        return JsonSerializer.Deserialize<T>(json, _options);
    }

    /// <summary>
    /// Escreve (sobrescreve) um objeto em um arquivo JSON existente.
    /// Se não existir, cria.
    /// </summary>
    public void WriteJsonFile<T>(string filePath, T data)
    {
        var json = JsonSerializer.Serialize(data, _options);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Verifica se um arquivo JSON existe.
    /// </summary>
    public bool ExistJsonFile(string filePath)
    {
        return File.Exists(filePath);
    }


    /// <summary>
    /// Verifica se um arquivo JSON existe.
    /// </summary>
    public bool ExistJsonFile(string filePath, string FileName)
    {
        var fullPath = CreateFullPath(filePath, FileName);
        return File.Exists(fullPath);
    }


    public static string CreateFullPath(string filePath, string fileName)
    {
        return Path.Combine(filePath, $"{fileName}.json");
    }
}
