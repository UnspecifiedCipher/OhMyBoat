namespace OMB.Repositories;

using OMB.Aplication.ClasesBase;
using OMB.Aplication.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ShipRepository : IShipRepository{
    
        private IShipPostRepository SPRep;

        public ShipRepository(IShipPostRepository SPRep){
            this.SPRep = SPRep;
        }
        public void addShip (Ship ship){
        using(OMBContext context = new OMBContext()){
            var exists = context.Ships.Where(S => S.plate == ship.plate).SingleOrDefault();
            if(exists == null){
                context.Add(Clone(ship));
            }
            else{
                throw new Exception("Plate number already registered");
            }
        }
    }
    public void deleteShip (int shipId){
        List<ShipPost> posts = new List<ShipPost>();
        using(OMBContext context = new OMBContext()){
            var exists = context.Ships.Include(S => S.ShipPosts).Where(S => S.Id == shipId).SingleOrDefault();
            if(exists != null){
                posts = exists.ShipPosts;
            }
        }
        foreach(ShipPost p in posts){
            this.SPRep.deleteShipPost(p.Id);
        }
        using(OMBContext context = new OMBContext()){
            var exists = context.Ships.Where(S => S.Id == shipId).SingleOrDefault();
            if(exists != null){
                context.Remove(exists);
            }
        }
    }
    public void modifyShip (Ship ship){
        using(OMBContext context = new OMBContext()){
            var exists = context.Ships.Where(S => S.Id == ship.Id).SingleOrDefault();
            var aux = context.Ships.Where(S => S.plate == ship.plate).SingleOrDefault();
            if(exists != null){
                if(aux != null){
                    exists.eslora = ship.eslora;
                    exists.manga = ship.manga;
                    exists.calado = ship.calado;
                    exists.hasEngine = ship.hasEngine;
                    exists.type = ship.type;
                    exists.description = ship.description;
                    exists.plate = ship.plate;
                    context.SaveChanges();
                }
                else{
                    throw new Exception("This plate is already in our database");
                }
            }
        }
    }

    public List<Ship> shipList(){
        using(OMBContext context = new OMBContext()){
            List<Ship> Ret = new List<Ship>();
            List<Ship> Ori = context.Ships.ToList();
            foreach (Ship ship in Ori)
                Ret.Add(Clone(ship));
            return Ret;
        }
    }

    private Ship Clone(Ship ship){
        return new Ship(ship.UserId, ship.type, ship.plate, ship.description, ship.eslora, ship.manga, ship.calado, ship.hasEngine){Id = ship.Id};
    }
}
