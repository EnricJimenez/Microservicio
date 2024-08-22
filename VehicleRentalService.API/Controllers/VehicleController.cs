using Microsoft.AspNetCore.Mvc;
using VehicleRentalService.Application.Services;
using VehicleRentalService.Core.Entities;

namespace VehicleRentalService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly FleetManagementService _fleetManagementService;

        public VehicleController(FleetManagementService fleetManagementService)
        {
            _fleetManagementService = fleetManagementService;
        }

        [HttpPost]
        public IActionResult AddVehicle([FromBody] Vehicle vehicle)
        {
            _fleetManagementService.AddNewVehicle(vehicle);
            return Ok();
        }

        [HttpGet("available")]
        public IActionResult GetAvailableVehicles()
        {
            var vehicles = _fleetManagementService.GetAvailableVehicles();
            return Ok(vehicles);
        }
    }
}
