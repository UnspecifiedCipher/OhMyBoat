using System.Drawing;
using OMB.Aplication.ClasesBase;

namespace OMB.Repositories;

public class VehicleImageRepository{
    public void addVehicleImage(int Id, Image img){
        using(OMBContext context = new OMBContext()){
            Vehicle? v = context.Vehicles.Where(ve => ve.Id == Id).SingleOrDefault();
            if(v != null){
                int i = context.VehicleImages.Where(im => im.VehicleId == Id).Count();
                if (i < 3){
                    using(var ms = new MemoryStream()){
                        (new Bitmap(img)).Save(ms, img.RawFormat);
                        context.Add(new VehicleImage(Id, ms.ToArray()));
                        context.SaveChanges();
                    }
                }
            }
        }
    }

    public List<VehicleImage> listVehicleImages(){
        using(OMBContext context = new OMBContext()){
            List<VehicleImage> ret = new List<VehicleImage>();
            List<VehicleImage> ori = context.VehicleImages.ToList();
            foreach(VehicleImage v in ori){
                ret.Add(Clone(v));
            }
            return context.VehicleImages.ToList();
        }
    }

    private VehicleImage Clone(VehicleImage ve){
        byte[] aux = new byte[ve.Image.Count()];
        for(int i = 0; i < ve.Image.Count(); i++){
            aux[i] = ve.Image[i];
        }
        return new VehicleImage(ve.VehicleId, aux){Id = ve.Id};
    }
}