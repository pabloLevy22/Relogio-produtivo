using System.Net;
using System.Runtime.CompilerServices;

public class RelógioTemporizadorPomodoro : RelógioTemporizador
{
    private int _cicloTrabalho;
    private int _cicloDescanso;
    private int _cicloDescansoLongo;
    private bool _automatico;

    public event EventHandler? eventoInterfase;

    private int cicloTrabalho { get { return _cicloTrabalho; } set { _cicloTrabalho = value; OnPropriedadeAtualizadaInterfase(); } }
    private int cicloDescanso { get { return _cicloDescanso; } set { _cicloDescanso = value; OnPropriedadeAtualizadaInterfase(); } }
    private int cicloDescansoLongo { get { return _cicloDescansoLongo; } set { _cicloDescansoLongo = value; OnPropriedadeAtualizadaInterfase(); } }
    private bool automatico { get { return _automatico; } set { _automatico = value; OnPropriedadeAtualizadaInterfase(); } }
    private bool Encerrar { get; set; }

    public override int tempo { get { return _tempo; } set { _tempo = value; OnPropriedadeAtualizadaInterfase(); } }

    protected virtual void OnPropriedadeAtualizadaInterfase()
    {
        eventoInterfase?.Invoke(this, EventArgs.Empty);
    }

    public RelógioTemporizadorPomodoro()
    {
        cicloTrabalho = 0;
        cicloDescanso = 0;
        cicloDescansoLongo = 0;

        automatico = false;
        Encerrar = false;
    }

    protected override void SistemaDeControleDeExercução(Task objeto)
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
            else if (comando.Key == ConsoleKey.A)
            {
                automatico = !automatico;
            }
        }
        while (!objeto.IsCompleted);
    }

    private Task AlterarTempoManualmente()
    {
        ConsoleKeyInfo comando;

        do
        {

            comando = Console.ReadKey(true);

            switch (comando.Key)
            {
                case ConsoleKey.UpArrow:
                    if (tempo < 3600) tempo += 60;
                    continue;

                case ConsoleKey.DownArrow:
                    if (tempo > 59) tempo -= 60;
                    continue;

                case ConsoleKey.Enter:
                    return Task.CompletedTask;

                case ConsoleKey.A:
                    automatico = !automatico;
                    continue;

                case ConsoleKey.Escape:
                    Encerrar = true;
                    return Task.CompletedTask;

                default:
                    continue;
            }
        }
        while (true);
    }

    private int CalculoPomodoro(int tempoAnterior, bool definidoEntreDescansoETrabalho)
    {
        int tempoIntermediário = tempoAnterior - tempo;

        if (cicloTrabalho % 4 == 0 && cicloTrabalho != 0 && !definidoEntreDescansoETrabalho)
        {
            tempoIntermediário = 900;
        }
        else if (definidoEntreDescansoETrabalho)
        {
            tempoIntermediário = tempoAnterior;
        }
        else if (tempoIntermediário >= 1200 && tempoIntermediário <= 1500)
        {
            tempoIntermediário = 300;
        }
        else if (tempoIntermediário <= 240)
        {
            tempoIntermediário = 60;
        }
        else
        {
            tempoIntermediário = tempoIntermediário < 1200 ? tempoIntermediário * 5 / 20 : tempoIntermediário * 5 / 25;

            tempoIntermediário -= tempoIntermediário % 60;
        }

        return tempoIntermediário;
    }

    static void LimparLinha(int numeroLinha)
    {
        int windowHeight = Console.WindowHeight;
        if (numeroLinha >= 0 && numeroLinha < windowHeight)
        {
            Console.SetCursorPosition(0, numeroLinha);

            string espacos = new string(' ', Console.WindowWidth);
            Console.Write(espacos);
        }
    }

    public override void Funcionalidade()
    {
        Console.Clear();

        bool definidoEntreDescansoETrabalho = true;

        int tempoAnterior = 0;
        int linhaAtualInterfase = Console.CursorTop;

        eventoInterfase += (sender, args) =>
        {
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, linhaAtualInterfase);
                Console.WriteLine($"Trabalho:{cicloTrabalho}    Descanso:{cicloDescanso}    Descanso longo:{cicloDescansoLongo}".PadRight(50));
                Console.WriteLine(MostragemDeTempo().PadRight(10));
                Console.WriteLine(("automatico; " + (automatico ? "ligado" : "desligado") + ": A" + "   Encerrar: Esc").PadRight(50));
            }
        };

        tempo = 1200;

        int linhaAtualTemporizar = Console.CursorTop;

        eventoTemporizador += (sender, args) =>
        {
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, linhaAtualTemporizar);
                Console.WriteLine($"Enter: {(pausa ? "Despausa" : "Pausa").PadRight(9)}Esc: Encerrar execução".PadRight(10));
            }
        };

        while (true)
        {
            if (!automatico)
            {
                Task alterarTempoManualmente = AlterarTempoManualmente();

                if (Encerrar) break;

                if (definidoEntreDescansoETrabalho)
                {
                    tempoAnterior = tempo;
                }
            }

            if (cicloTrabalho % 4 == 0 && cicloTrabalho != 0 && !definidoEntreDescansoETrabalho)
                cicloDescansoLongo++;
            else if (definidoEntreDescansoETrabalho)
                cicloTrabalho++;
            else
                cicloDescanso++;

            Task temporizar = Temporizador();
            SistemaDeControleDeExercução(temporizar);
            LimparLinha(linhaAtualTemporizar);

            definidoEntreDescansoETrabalho = !definidoEntreDescansoETrabalho;

            tempo = CalculoPomodoro(tempoAnterior, definidoEntreDescansoETrabalho);
        }
    }
}