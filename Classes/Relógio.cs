using System.Data.SqlTypes;
using System.Net;
using System.Reflection;
using System.Security;

#pragma warning disable SYSLIB0006

public class Relógio
{
    public int tempo { get; set; }
    public int segundo { get { return tempo % 60; } set {; } }
    public int minuto { get { return tempo / 60; } set {; } }
    public bool desliga { get; set; }
    public bool pausa { get; set; }
    private bool encerrar;

    private string MostragemDeTempo()
    {
        string segundos = segundo > 9 ? segundo.ToString() : "0" + segundo.ToString();
        string minutos = minuto > 9 ? minuto.ToString() : "0" + minuto.ToString();

        return minutos + ":" + segundos;
    }

    private void SistemaDeControleDeExercução()
    {
            ConsoleKeyInfo comando = Console.ReadKey();

            if (comando.Key == ConsoleKey.Escape)
            {
                desliga = true;
            }
            else if (comando.Key == ConsoleKey.Enter)
            {
                pausa = !pausa;
            }
            else
                SistemaDeControleDeExercução();

            encerrar = true;
    }

    public static void Temporizador(int entrada)
    {
        Relógio relógio = new Relógio() { tempo = entrada, pausa = false, desliga = false };

        Console.CursorVisible = false;

        var cancela = new CancellationTokenSource();

        int linhaAtual = Console.CursorTop;
        do
        {
            Task inputTask = Task.Run(() =>
            {
                relógio.SistemaDeControleDeExercução();
            });
            do
            {
                Console.WriteLine(relógio.MostragemDeTempo());
                Console.SetCursorPosition(0, linhaAtual);

                relógio.tempo--;
                Thread.Sleep(1000);
            } while (relógio.tempo >= 0 && !relógio.pausa && !relógio.desliga);

            if (relógio.pausa)
            {
                relógio.SistemaDeControleDeExercução();
                continue;
            }

            if (relógio.desliga) return;

            cancela.Cancel();
        }
        while (relógio.tempo >= 0);

        for (int i = 5; i > -1; i--) Console.Beep();
        Console.CursorVisible = true;
    }
}

public class RelógioPomodoro : Relógio
{
    public static void Pomodoro()
    {
        Console.Clear();
        Console.WriteLine("Defina o numero de ciclos e tempo");
        SistemaDeSelecionaCiclo();
    }

    private static void SistemaDeSelecionaCiclo()
    {
        int linhaAtual = Console.CursorTop;
        int ciclo = 4;
        int tempoDeTrabalho = 20;

        do
        {
            Console.SetCursorPosition(0, linhaAtual);
            Console.WriteLine($"Ciclo:{ciclo}\nTempo:{tempoDeTrabalho} de trabalho" +
                            $"|{tempoDeTrabalho / 5} descanso|{30} descanso longo");
            Console.WriteLine("Seta \u2194 para diminuir e aumenta os ciclo" +
                            "\nSeta \u2195 para diminuir e aumenta tempo");

            ConsoleKeyInfo comando = Console.ReadKey();

            switch (comando.Key)
            {
                case ConsoleKey.UpArrow:
                    if (tempoDeTrabalho < 60) tempoDeTrabalho++;
                    continue;

                case ConsoleKey.DownArrow:
                    if (tempoDeTrabalho > 1) tempoDeTrabalho--;
                    continue;

                case ConsoleKey.LeftArrow:
                    if (ciclo < 9) ciclo++;
                    continue;

                case ConsoleKey.RightArrow:
                    if (ciclo > 1) ciclo--;
                    continue;

                case ConsoleKey.Enter:
                    break;

                case ConsoleKey.Escape:
                    return;

                default:
                    continue;
            }

            for (int i = 0; i < ciclo; i++)
            {
                Temporizador(tempoDeTrabalho);
            }

            return;
        }
        while (true);
    }
}