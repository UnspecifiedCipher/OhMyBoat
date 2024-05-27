namespace OMB.Aplication.ClasesBase;

public class Offer : IClonable {

    public int Id {get; set;}
    public int transportePosteadoId {get; set;}
    public int transporteOfertadoId {get; set;}
    public Transport transportePosteado {get; set;}
    public Transport transporteOfertado {get; set;}

    public Offer (int transportePosteadoId, int transporteOfertadoId) {
        this.transporteOfertadoId = transporteOfertadoId;
        this.transportePosteadoId = transportePosteadoId;
    }

    public IClonable Clone () {
        return new Offer (this.transportePosteadoId, this.transporteOfertadoId);
    }

}