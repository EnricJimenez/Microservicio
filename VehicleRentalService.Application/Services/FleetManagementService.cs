using System.Collections.Generic;
using VehicleRentalService.Core.Entities;
using VehicleRentalService.Core.Interfaces;

namespace VehicleRentalService.Application.Services
{
    public class FleetManagementService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public FleetManagementService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public void AddNewVehicle(Vehicle vehicle)
        {
            if (vehicle.ManufacturingDate.AddYears(5) <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Vehicle is too old to be added to the fleet.");
            }
            _vehicleRepository.AddVehicle(vehicle);
        }

        public IEnumerable<Vehicle> GetAvailableVehicles()
        {
            return _vehicleRepository.GetAvailableVehicles();
        }
    }
}
