public class Program
{
    static void Main()
    {   
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine($"Painel de comando" +
        "\n\nopções de funções:" +
        "\n     Temporizador: Clique X" + 
        "\n     Pomodoro    : Clique P");

        ComandoDoPainelInicial();
    }

    static void ComandoDoPainelInicial()
    {
        ConsoleKeyInfo tecla = Console.ReadKey();

        switch (tecla.Key)
        {
            case ConsoleKey.X:
                Console.Clear();
                Console.WriteLine("insirar os segundos");

                int.TryParse(Console.ReadLine(), out int entrada);

                Console.Clear();
                Console.WriteLine
                ("Enter: pausa Esq: encerrar");

                Relógio relógio = new Relógio{tempo = entrada, encerrar = false};

                Task trabalho = Task.Run(relógio.SistemaDeControleDeExercução);
                
                relógio.Temporizador();
                relógio.encerrar = true;
                break;

                case ConsoleKey.P:
                    Relógio	pomodoro = new RelógioPomodoro();
                    
                    pomodoro.Funcionalidade();
                    break;

            case ConsoleKey.Escape:
                return;

            default:
                ComandoDoPainelInicial();
                break;
        }

    }
}