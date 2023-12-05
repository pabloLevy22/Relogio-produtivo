public class Program
{
    static void Main()
    {
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine($"Painel de comando" +
        "\n\nopções de funções:" +
        "\n     Temporizador: Clique T" +
        "\n     Cronometro  : Clique C" +
        "\n     Pomodoro    : Clique P" +
        "\n     Desempenho  : Clique D");

        ComandoDoPainelInicial();
    }

    static void ComandoDoPainelInicial()
    {
        ConsoleKeyInfo tecla = Console.ReadKey(true);

        switch (tecla.Key)
        {
            case ConsoleKey.T:
                Console.Clear();
                Console.WriteLine("insirar os segundos");

                int.TryParse(Console.ReadLine(), out int entrada);

                Console.Clear();

                Relógio relógio = new RelógioTemporizador {};

                relógio.Funcionalidade();
                break;

            case ConsoleKey.P:
                Relógio pomodoro = new RelógioTemporizadorPomodoro();

                pomodoro.Funcionalidade();
                break;

            case ConsoleKey.C:
                Console.Clear();

                Relógio cronometro = new RelógioCronometro();

                cronometro.Funcionalidade();
                break;
            
            case ConsoleKey.D:
                break;

            case ConsoleKey.Escape:
                return;

            default:
                ComandoDoPainelInicial();
                break;
        }

    }
}