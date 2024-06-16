namespace OMB.Aplication.ClasesBase;

public class ResolvedExchange : Exchange{
    public bool happen {get; set;}

    public int transportePosteadoId {get; set;}
    public int transporteOfertadoId {get; set;}
    public TransportHistory? transportePosteado {get; set;}
    public TransportHistory? transporteOfertado {get; set;}

    public ResolvedExchange (int transportePosteadoId, int transporteOfertadoId, bool happen) {
        this.transporteOfertadoId = transporteOfertadoId;
        this.transportePosteadoId = transportePosteadoId;
        this.happen = happen;
    }

    public override IClonable Clone(){
        return new ResolvedExchange(this.transportePosteadoId, this.transporteOfertadoId, this.happen){Id=this.Id, fechaYHora=this.fechaYHora, sede=this.sede, transporteOfertado=(this.transporteOfertado != null)? (TransportHistory)this.transporteOfertado.Clone() : null, transportePosteado=(this.transportePosteado != null)? (TransportHistory)this.transportePosteado.Clone() : null};
    }

}