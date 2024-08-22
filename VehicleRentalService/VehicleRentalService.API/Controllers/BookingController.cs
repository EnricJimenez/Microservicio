using Microsoft.AspNetCore.Mvc;
using VehicleRentalService.Application.Services;
using System.ComponentModel.DataAnnotations;
using System;

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
            var bookingId = Guid.NewGuid().ToString();
            _bookingService.RentVehicle(request.VehicleId, request.CustomerId);

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
            var success = _bookingService.ReturnVehicle(bookingId);

            if (success)
            {
                return Ok(new { Message = "Vehicle returned successfully." });
            }
            else
            {
                return NotFound($"No booking found with BookingId: {bookingId}");
            }
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
