public partial class Relógio
{
    private int _tempo;
    private bool _desliga;
    private bool _pausa;
    private bool _encerrar;

    public Relógio()
    {

    }

    public int tempo { get { return _tempo; } set { _tempo = value; OnPropriedadeAtualizada(); } }
    protected int segundo { get { return tempo % 60; } set {; } }
    protected int minuto { get { return tempo / 60; } set {; } }
    protected virtual bool desliga { get { return _desliga; } set { _desliga = value; } }
    protected virtual bool pausa { get { return _pausa; } set { _pausa = value; OnPropriedadeAtualizada(); } }
    protected virtual bool encerrar { get { return _encerrar; } set { _encerrar = value; } }

    public event EventHandler? PropriedadeAtualizada;
    public static object consoleLock = new object();

    protected virtual void OnPropriedadeAtualizada()
    {
        PropriedadeAtualizada?.Invoke(this, EventArgs.Empty);
    }

    protected string MostragemDeTempo()
    {
        string segundos = segundo > 9 ? segundo.ToString() : "0" + segundo.ToString();
        string minutos = minuto > 9 ? minuto.ToString() : "0" + minuto.ToString();

        return minutos + ":" + segundos;
    }

    protected virtual void SistemaDeControleDeExercução()
    {
        pausa = false;
        desliga = false;

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

    protected void Temporizador()
    {
        do
        {
            while (pausa)
            {
                if (desliga) return;
            }
            
            if (desliga) return;

            tempo--;
            Thread.Sleep(1000);
        }
        while (tempo >= 0);

        for (int i = 5; i > -1; i--) Console.Beep();
    }

    public virtual void Funcionalidade()
    {
        int linhaAtual = Console.CursorTop;

        Task ControleDeExecução = Task.Run(SistemaDeControleDeExercução);

        PropriedadeAtualizada += (sender, args) =>
        {
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, linhaAtual);
                Console.WriteLine($"Enter: {(pausa ? "Despausa" : "Pausa").PadRight(10)}Esq: Encerrar".PadRight(10) + "\n" + MostragemDeTempo().PadRight(10));
            }
        };

        Temporizador();
    }
}