using System.Net;
using System.Reflection;
using System.Security;

public class Relógio
{
    public int tempo { get; set; }
    public int segundo { get { return tempo % 60; } set {; } }
    public int minuto { get { return tempo / 60; } set {; } }
    public bool desliga { get; set; }
    public bool pausa { get; set; }

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
    }

    public static void Temporizador(int entrada)
    {
        Relógio relógio = new Relógio() { tempo = entrada, pausa = false, desliga = false};

        Console.CursorVisible = false;

        int linhaAtual = Console.CursorTop;
        do
        {
            Thread ComandosDeControle = new Thread(relógio.SistemaDeControleDeExercução);
            ComandosDeControle.Start();
            do
            {
                Console.WriteLine(relógio.MostragemDeTempo());
                Console.SetCursorPosition(0, linhaAtual);

                relógio.tempo--;
                Thread.Sleep(1000);
            } while (relógio.tempo >= 0 && !relógio.pausa && !relógio.desliga);

            if (relógio.pausa)
                relógio.SistemaDeControleDeExercução();               
            if (relógio.desliga)
                return;
        }
        while (relógio.tempo >= 0);

        for (int i = 5; i > -1; i--) Console.Beep();
        Console.CursorVisible = true;
    }
}