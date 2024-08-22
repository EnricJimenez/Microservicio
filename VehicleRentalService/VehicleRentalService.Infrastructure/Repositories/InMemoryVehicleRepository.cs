using System.Collections.Generic;
using System.Linq;
using VehicleRentalService.Core.Entities;
using VehicleRentalService.Core.Interfaces;

namespace VehicleRentalService.Infrastructure.Repositories
{
    public class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles = new();

        public void AddVehicle(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
        }

        public IEnumerable<Vehicle> GetAvailableVehicles()
        {
            return _vehicles.Where(v => v.IsAvailable()).ToList();
        }

        public Vehicle GetVehicleById(string id)
        {
            return _vehicles.FirstOrDefault(v => v.Id == id) ?? throw new Exception("Vehicle not found");
        }
    }
}
