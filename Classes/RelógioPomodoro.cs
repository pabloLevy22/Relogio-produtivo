public class RelógioPomodoro : Relógio
{
    private bool _automatico;

    private int ciclo { get; set; }
    private bool automatico
    {
        get
        {
            return _automatico;
        }
        set
        {
            _automatico = value;
            OnPropriedadeAtualizada();
        }
    }

    public override void Funcionalidade()
    {
        Console.Clear();
        Console.WriteLine("Defina o numero de ciclos e tempo");

        RelógioPomodoro pomodoro = new RelógioPomodoro();

        pomodoro.SistemaDeSelecionaCiclo();
        pomodoro.CicloPomodoro();
    }
    protected override void SistemaDeControleDeExercução()
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
            while (true);
    }

    private void SistemaDeSelecionaCiclo()
    {
        int linhaAtual = Console.CursorTop;
        ciclo = 4;
        tempo = 20;

        do
        {
            Console.SetCursorPosition(0, linhaAtual);
            Console.WriteLine($"Ciclo:{ciclo}\nTempo:{tempo} de trabalho" +
                            $"|{tempo / 5} descanso|{30} descanso longo");
            Console.WriteLine("Seta \u2194 para diminuir e aumenta os ciclo" +
                            "\nSeta \u2195 para diminuir e aumenta tempo");

            ConsoleKeyInfo comando = Console.ReadKey();

            switch (comando.Key)
            {
                case ConsoleKey.UpArrow:
                    if (tempo < 60) tempo++;
                    continue;

                case ConsoleKey.DownArrow:
                    if (tempo > 1) tempo--;
                    continue;

                case ConsoleKey.RightArrow:
                    if (ciclo < 9) ciclo++;
                    continue;

                case ConsoleKey.LeftArrow:
                    if (ciclo > 1) ciclo--;
                    continue;

                case ConsoleKey.Enter:
                    break;

                default:
                    continue;
            }

            break;
        }
        while (true);

        tempo *= 60;
    }

    private void CicloPomodoro()
    {
        Console.Clear();

        int tempoOriginal = tempo;
        int trabalhoCiclo = 0;
        int descansoCiclo = 0;
        int descansoLongoCiclo = 0;
        int LinhaDoAutomatico = Console.CursorTop;

        var cancellationTokenSource = new CancellationTokenSource();

        Task tarefa = Task.Run(SistemaDeControleDeExercução);

        PropriedadeAtualizada += (sender, args) =>
        {
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, LinhaDoAutomatico);
                Console.WriteLine($"Trabalho:{trabalhoCiclo}  Descanso:{descansoCiclo} Descanso longo:{descansoLongoCiclo}".PadRight(40));
                Console.WriteLine("automatico: " + (automatico ? "ligado" : "desligado").PadRight(10) + "Muder apertado 'A'");
                Console.WriteLine($"Enter: {(pausa ? "Despausa" : "Pausa").PadRight(9)}Esq: Encerrar".PadRight(10));
                Console.WriteLine(MostragemDeTempo().PadRight(10));
            }
        };

        do
        {
            if (trabalhoCiclo == descansoCiclo + descansoLongoCiclo)
            {
                tempo = tempoOriginal;

                Temporizador();

                trabalhoCiclo++;
            }
            else if (trabalhoCiclo == ciclo)
            {
                tempo = (int)(tempoOriginal * 1.5);

                Temporizador();

                descansoLongoCiclo++;
            }
            else
            {
                tempo = tempoOriginal / 4;

                Temporizador();

                descansoCiclo++;
            }

            if (ciclo == descansoCiclo + descansoLongoCiclo)
            {
                return;
            }
            else if (automatico)
            {
                desliga = false;

                continue;
            }
            
            desliga = false;

            do
            {
                Console.Clear();
                Console.WriteLine("Continua(S) ou não(N)?");

                ConsoleKeyInfo comando = Console.ReadKey(true);

                if (comando.Key == ConsoleKey.S)
                    break;
                else if (comando.Key == ConsoleKey.N)
                    return;
            }
            while (true);
        }
        while (true);
    }
}