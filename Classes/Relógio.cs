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
            desliga = false;
        }
        else if (comando.Key == ConsoleKey.Enter)
        {
            if (pausa == false)
            {
                pausa = true;
                return;
            }

            pausa = false;
        }
        else
            SistemaDeControleDeExercução();
    }

    public static void Temporizador(int entrada)
    {
        Relógio relógio = new Relógio() { tempo = entrada, pausa = true, desliga = true };

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
            } while (relógio.tempo >= 0 && relógio.pausa == true && relógio.desliga == true);

            if (relógio.desliga == true)
                relógio.SistemaDeControleDeExercução();
        }
        while (relógio.desliga);

        for (int i = 5; i > -1; i--) Console.Beep();
        Console.CursorVisible = true;
    }
}