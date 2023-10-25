using System.ComponentModel.Design;

public class Relógio
{
    public int tempo { get; set; }
    public int segundo { get { return tempo % 60; } set {; } }
    public int minuto { get { return tempo / 60; } set {; } }
    public bool desliga { get; set; }
    public bool pausa { get; set; }
    public bool encerrar { get; set; }

    public virtual void Funcionalidade()
    {}

    private string MostragemDeTempo()
    {
        string segundos = segundo > 9 ? segundo.ToString() : "0" + segundo.ToString();
        string minutos = minuto > 9 ? minuto.ToString() : "0" + minuto.ToString();

        return minutos + ":" + segundos;
    }

public virtual void SistemaDeControleDeExercução()
{
    pausa = false;
    desliga = false;
    encerrar = false;

    ConsoleKeyInfo comando;

    do
    {
        comando = new ConsoleKeyInfo();

        if (Console.KeyAvailable)
        {
            comando = Console.ReadKey();
        }

        if (comando.Key == ConsoleKey.Escape)
        {
            desliga = true;
        }
        else if (comando.Key == ConsoleKey.Enter)
        {
            pausa = !pausa;
        }
    } while (true);
}


    public void Temporizador()
    {
        int linhaAtual = Console.CursorTop;
        do
        {
            do
            {
                Console.SetCursorPosition(0, linhaAtual);
                Console.WriteLine(MostragemDeTempo().PadRight(20));

                tempo--;
                Thread.Sleep(1000);
            }
            while (tempo >= 0 && !pausa && !desliga);

            if (pausa)
            {
                do
                {
                    if (desliga) return;
                }
                while (pausa);

                continue;
            }

            if (desliga) return;
        }
        while (tempo >= 0);

        for (int i = 5; i > -1; i--) Console.Beep();
    }
}