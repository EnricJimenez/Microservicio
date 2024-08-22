using Microsoft.AspNetCore.Mvc;
using VehicleRentalService.Application.Services;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("rent")]
    public IActionResult RentVehicle([FromBody] RentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Llamamos al servicio para alquilar el vehículo y obtener el ID de la reserva
            var bookingId = _bookingService.RentVehicle(request.VehicleId, request.CustomerId);

            return Ok(new { BookingId = bookingId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message} - {ex.StackTrace}");
        }
    }

    [HttpPost("return/{bookingId}")]
    public IActionResult ReturnVehicle(string bookingId)
    {
        if (string.IsNullOrEmpty(bookingId))
        {
            return BadRequest("BookingId is required.");
        }

        try
        {
            // Llamamos al servicio para devolver el vehículo
            _bookingService.ReturnVehicle(bookingId);

            return Ok(new { Message = "Vehicle returned successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message} - {ex.StackTrace}");
        }
    }
}

public class RentRequest
{
    [Required]
    public string VehicleId { get; set; } = string.Empty;

    [Required]
    public string CustomerId { get; set; } = string.Empty;
}

