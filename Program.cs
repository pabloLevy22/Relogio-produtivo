public class Program
{
    static void Main()
    {
        Console.WriteLine($"Painel de comando" +
        "\n\nopções de funções:" +
        "\n     Temporizador: Clique X");

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

                Relógio.Temporizador(entrada);
                break;

            case ConsoleKey.Escape:
                return;

            default:
                ComandoDoPainelInicial();
                break;
        }

    }
}