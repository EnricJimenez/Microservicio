using VehicleRentalService.Core.Entities;
using System.Collections.Generic;

namespace VehicleRentalService.Core.Interfaces
{
    public interface IVehicleRepository
    {
        void AddVehicle(Vehicle vehicle);
        IEnumerable<Vehicle> GetAvailableVehicles();
        Vehicle GetVehicleById(string id);
    }
}
