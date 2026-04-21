using System.Text;

namespace Services
{
    public class ConsoleService
    {
        /// <summary>
        /// Escreve uma mensagem informativa no console com o prefixo "[INFO]:" em azul.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteInfo(string text)
        {
            Write("[INFO]: ", ConsoleColor.Blue);
            WriteLine(text);
        }

        /// <summary>
        /// Escreve uma mensagem de aviso no console com o prefixo "[WARN]:" em amarelo.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteWarning(string text)
        {
            Write("[WARN]: ", ConsoleColor.Yellow);
            WriteLine(text);
        }

        /// <summary>
        /// Escreve uma mensagem de erro no console com o prefixo "[ERROR]:" em vermelho.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteError(string text)
        {
            Write("[ERROR]: ", ConsoleColor.Red);
            WriteLine(text);
        }

        /// <summary>
        /// Escreve uma mensagem de sucesso no console com o prefixo "[SUCCESS]:" em verde.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteSuccess(string text)
        {
            Write("[SUCESS]: ", ConsoleColor.Green);
            WriteLine(text);
        }

        /// <summary>
        /// Exibe uma mensagem de resultado no console, precedida pelo rótulo "Resultado:" 
        /// em amarelo escuro. O texto informado é exibido logo em seguida na cor padrão do console.
        /// </summary>
        /// <param name="text">O texto a ser exibido após o rótulo "Resultado:". Caso não seja informado,  nada é exibido.</param>
        public static void WriteResult(string text = "")
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Resultado: ");
            Console.ForegroundColor = previousColor;
            Write(text);
        }

        /// <summary>
        /// Exibe um título de formatado dentro de uma caixa com bordas no console.
        /// </summary>
        /// <param name="title"> texto que será exibido como título.</param>
        public static void WriteHeader(string text)
        {
            var title = text.ToUpper().Trim();

            Console.WriteLine();

            var lineBuilder = new StringBuilder();

            for (int i = 0; i < title.Length + 10; i++)
            {
                lineBuilder.Append("─");
            }

            var titleAligment = "     ";

            var layoutLine = lineBuilder.ToString();
            var header = $"{titleAligment}{title}{titleAligment}";

            WriteLine($"┌{layoutLine}┐", ConsoleColor.DarkYellow);
            WriteLine($"│{header}│", ConsoleColor.DarkYellow);
            WriteLine($"└{layoutLine}┘", ConsoleColor.DarkYellow);

            Console.WriteLine();
        }

        /// <summary>
        /// Exibe um título de formatado dentro de uma caixa com bordas no console.
        /// </summary>
        /// <param name="text"> texto que será exibido como título.</param>
        public static void WriteSubHeader(string text, ConsoleColor color = ConsoleColor.Yellow)
        {
            var title = text.ToUpper().Trim();
            WriteLine();
            WriteLine($"* {title}", color);
            WriteLine();
        }

        /// <summary>
        /// Escreve uma linha de texto colorida no console e move o cursor para a próxima linha.
        /// </summary>
        /// <param name="text">Texto a ser exibido.</param>
        /// <param name="color">Cor do texto. Padrão: branco.</param>
        public static void WriteLine(string text = "", ConsoleColor color = ConsoleColor.White)
        {
            var previousColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(text);

            Console.ForegroundColor = previousColor;
        }

        /// <summary>
        /// Escreve um texto colorido no console sem quebrar linha.
        /// </summary>
        /// <param name="text">Texto a ser exibido.</param>
        /// <param name="color">Cor do texto. Padrão: branco.</param>
        public static void Write(string text, ConsoleColor color = ConsoleColor.White)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = previousColor;
        }

        /// <summary>
        /// Escreve um texto adicionando $ na cor verde.
        /// </summary>
        /// <param name="text">Texto a ser exibido.</param>
        public static void WriteCommand(string text, ConsoleColor color = ConsoleColor.DarkGreen)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"$ {text}");
            Console.ForegroundColor = previousColor;
        }


        /// <summary>
        /// Escreve um texto no console e espera o input do usuário
        /// </summary>
        /// <param name="message">Texto a ser exibido.</param>
        public static string AskForInput(string message, ConsoleColor color = ConsoleColor.Yellow)
        {
            Write($"{message}: ", ConsoleColor.Yellow);
            return Console.ReadLine() ?? string.Empty;
        }


    }
}
