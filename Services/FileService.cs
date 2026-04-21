
namespace Services
{
    public class FileService
    {
        public static void WriteFile(string directoryPath, string fileName, string content)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var filePath = Path.Combine(directoryPath, fileName);

            File.WriteAllText(filePath, content);
        }

        public static string ReadFile(string directoryPath, string fileName)
        {
            var filePath = Path.Combine(directoryPath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo não encontrado: {filePath}");

            return File.ReadAllText(filePath);
        }
    }
}
