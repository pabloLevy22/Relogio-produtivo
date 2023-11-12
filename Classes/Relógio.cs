public abstract class Relógio
{
    protected int  _tempo;
    protected bool _desliga;
    protected bool _pausa;

    public    virtual int  tempo   { get { return _tempo;     } set { _tempo = value;   } }
    protected         int  segundo { get { return tempo % 60; } set {;                  } }
    protected         int  minuto  { get { return tempo / 60; } set {; } }
    protected virtual bool desliga { get { return _desliga;   } set { _desliga = value; } }
    protected virtual bool pausa   { get { return _pausa;     } set { _pausa = value;   } }

    protected string MostragemDeTempo()
    {
        string segundos = segundo > 9 ? segundo.ToString() : "0" + segundo.ToString();
        string minutos  = minuto  > 9 ? minuto.ToString()  : "0" + minuto.ToString();

        return minutos + ":" + segundos;
    }

    protected abstract void SistemaDeControleDeExercução(Task objeto);

    public abstract void Funcionalidade();
}