public class RelógioCronometro : Relógio
{

    public static object consoleLock = new object();

    public event EventHandler? eventoRelógio;

    public override int tempo { get { return _tempo; } set { _tempo = value; OnPropriedadeAtualizada(); } }
    protected override bool pausa { get { return _pausa; } set { _pausa = value; OnPropriedadeAtualizada(); } }

    protected virtual void OnPropriedadeAtualizada()
    {
        eventoRelógio?.Invoke(this, EventArgs.Empty);
    }

    protected override void SistemaDeControleDeExercução()
    {
        ConsoleKeyInfo comando;

        do
        {
            comando = new ConsoleKeyInfo();

            if (Console.KeyAvailable)
            {
                comando = Console.ReadKey(true);
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

    protected async void Cronometro()
    {
        pausa = false;
        desliga = false;

        do
        {
            while (pausa)
            {
                if (desliga) return;
            }

            tempo++;

            if (desliga) return;

            await Task.Delay(1000);
        }
        while (true);
    }

    public override void Funcionalidade()
    {
        int linhaAtual = Console.CursorTop;

        eventoRelógio += (sender, args) =>
        {
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, linhaAtual);
                Console.WriteLine($"Enter: {(pausa ? "Despausa" : "Pausa").PadRight(9)}Esq: Encerrar".PadRight(10));
                Console.WriteLine(MostragemDeTempo().PadRight(10));
            }
        };

        Cronometro();
        SistemaDeControleDeExercução();
    }
}