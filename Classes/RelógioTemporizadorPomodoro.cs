public class RelógioTemporizadorPomodoro : RelógioTemporizador
{
    private int _cicloTrabalho;
    private int _cicloDescanso;
    private int _cicloDescansoLongo;
    private bool _automatico;

    private int cicloTrabalho { get { return _cicloTrabalho; } set { _cicloTrabalho = value; OnPropriedadeAtualizada(); } }
    private int cicloDescanso { get { return _cicloDescanso; } set { _cicloDescanso = value; OnPropriedadeAtualizada(); } }
    private int cicloDescansoLongo { get { return _cicloDescansoLongo; } set { _cicloDescansoLongo = value; OnPropriedadeAtualizada(); } }
    private bool automatico { get { return _automatico; } set { _automatico = value; OnPropriedadeAtualizada(); } }
    private bool Encerrar { get; set; }

    public RelógioTemporizadorPomodoro()
    {
        cicloTrabalho = 0;
        cicloDescanso = 0;
        cicloDescansoLongo = 0;

        automatico = true;
        Encerrar = false;
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
                return;
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
        while (true);
    }

    private void AlterarTempoManualmente()
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
                    return;

                case ConsoleKey.A:
                    automatico = !automatico;
                    continue;

                case ConsoleKey.Escape:
                    Encerrar = true;
                    return;

                default:
                    continue;
            }
        }
        while (true);
    }

    public override void Funcionalidade()
    {
        Console.Clear();

        bool definidoEntreDescansoETrabalho = false;

        int tempoAnterior = 0;
        int linhaAtual = Console.CursorTop;

        eventoRelógio += (sender, args) =>
        {
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, linhaAtual);
                Console.WriteLine($"Trabalho:{cicloTrabalho}    Descanso:{cicloDescanso}    Descanso longo:{cicloDescansoLongo}".PadRight(40));
                Console.WriteLine("automatico: " + (automatico ? "ligado" : "desligado").PadRight(9) + "Muder apertado 'A'".PadLeft(4));
                Console.WriteLine($"Enter: {(pausa ? "Despausa" : "Pausa").PadRight(8)}Esq: Encerrar".PadRight(10).PadLeft(4));
                Console.WriteLine(MostragemDeTempo().PadRight(10));
            }
        };

        tempo = 20 * 60;

        AlterarTempoManualmente();

        cicloTrabalho++;
        tempoAnterior = tempo;


        Task temporizar = Temporizador();

        SistemaDeControleDeExercução();

        temporizar.Wait();

        while (!Encerrar)
        {
            if (cicloTrabalho % 4 == 0 && cicloTrabalho != 0)
            {
                tempo = 15 * 60;
                cicloDescansoLongo++;
            }
            else if (!definidoEntreDescansoETrabalho)
            {
                tempo = tempoAnterior;
                cicloDescanso++;

                definidoEntreDescansoETrabalho = !definidoEntreDescansoETrabalho;
            }
            else
            {
                tempo = (int)(tempoAnterior * 0.20);
                cicloTrabalho++;

                definidoEntreDescansoETrabalho = !definidoEntreDescansoETrabalho;
            }

            if (!automatico)
            {
                AlterarTempoManualmente();

                if (definidoEntreDescansoETrabalho)
                    tempoAnterior = tempo;
            }

            temporizar = Temporizador();
            SistemaDeControleDeExercução();

            temporizar.Wait();
        }
    }
}