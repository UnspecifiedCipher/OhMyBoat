namespace OMB.Repositories;

using OMB.Aplication.ClasesBase;
using OMB.Aplication.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ShipRepository : IShipRepository {
    
    private IShipPostRepository SPRep;
    private IShipImageRepository SIRep;

    public ShipRepository(IShipPostRepository SPRep, IShipImageRepository SIRep) {
        this.SPRep = SPRep;
        this.SIRep = SIRep;
    }

    public void addShip (Ship ship) {
        using(OMBContext context = new OMBContext()) {
            var exists = context.Ships.Where(S => S.plate == ship.plate).SingleOrDefault();
            if(exists == null) {
                context.Add(Clone(ship));
                context.SaveChanges();
            }
            else {
                throw new Exception("Plate number already registered");
            }
        }
    }

    public void deleteShip (int shipId) {
        List<ShipPost> posts = new List<ShipPost>();
        using(OMBContext context = new OMBContext()) {
            var exists = context.Ships.Include(S => S.ShipPosts).Where(S => S.Id == shipId).SingleOrDefault();
            if(exists != null) {
                posts = exists.ShipPosts;
            }
        }
        foreach(ShipPost p in posts) {
            this.SPRep.deleteShipPost(p.Id);
        }

        List<ShipImage> images = new List<ShipImage>();
        using(OMBContext context = new OMBContext()){
            var exists = context.Ships.Include(S => S.ShipImages).Where(S => S.Id == shipId).SingleOrDefault();
            if(exists != null){
                images = exists.ShipImages;
            }
        }
        foreach(ShipImage si in images) {
            this.SIRep.deleteShipImage(si.Id);
        }

        using(OMBContext context = new OMBContext()) {
            var exists = context.Ships.Where(S => S.Id == shipId).SingleOrDefault();
            if(exists != null){
                context.Remove(exists);
                context.SaveChanges();
            }
        }
    }

    public void modifyShip (Ship ship) {
        using(OMBContext context = new OMBContext()) {
            var exists = context.Ships.Where(S => S.Id == ship.Id).SingleOrDefault();
            Ship? aux = null;
            if(exists != null) {
                if(ship.plate != exists.plate) {
                    aux = context.Ships.Where(S => S.plate == ship.plate).SingleOrDefault();
                }
                if(aux == null) {
                    exists.model = ship.model;
                    exists.eslora = ship.eslora;
                    exists.manga = ship.manga;
                    exists.calado = ship.calado;
                    exists.hasEngine = ship.hasEngine;
                    exists.type = ship.type;
                    exists.description = ship.description;
                    exists.plate = ship.plate;
                    context.SaveChanges();
                }
                else {
                    throw new Exception("This plate is already in our database");
                }
            }
        }
    }

    public List<Ship> shipList() {
        using(OMBContext context = new OMBContext()) {
            List<Ship> Ret = new List<Ship>();
            List<Ship> Ori = context.Ships.ToList();
            foreach (Ship ship in Ori)
                Ret.Add(Clone(ship));
            return Ret;
        }
    }

    private Ship Clone(Ship ship) {
        return new Ship(ship.UserId, ship.type, ship.plate, ship.description, ship.model, ship.eslora, ship.manga, ship.calado, ship.hasEngine){Id = ship.Id};
    }
}
