using Microsoft.Extensions.Logging;
using VehicleRentalService.Core.Entities;
using VehicleRentalService.Core.Interfaces;

namespace VehicleRentalService.Application.Services
{
    public class BookingService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IVehicleRepository vehicleRepository, IBookingRepository bookingRepository, ILogger<BookingService> logger)
        {
            _vehicleRepository = vehicleRepository;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        public string RentVehicle(string vehicleId, string customerId)
        {
            _logger.LogInformation("Attempting to rent vehicle with ID {VehicleId} to customer {CustomerId}", vehicleId, customerId);

            var vehicle = _vehicleRepository.GetVehicleById(vehicleId);
            if (vehicle == null || !vehicle.IsAvailable())
            {
                _logger.LogWarning("Vehicle with ID {VehicleId} is not available.", vehicleId);
                throw new Exception("Vehicle is not available.");
            }

            if (_bookingRepository.GetActiveBookingByCustomerId(customerId) != null)
            {
                _logger.LogWarning("Customer {CustomerId} already has an active booking.", customerId);
                throw new Exception("Customer already has an active booking.");
            }

            var booking = new Booking
            {
                Id = Guid.NewGuid().ToString(),
                VehicleId = vehicleId,
                CustomerId = customerId,
                StartDate = DateTime.UtcNow
            };

            vehicle.IsRented = true;
            _vehicleRepository.AddVehicle(vehicle);
            _bookingRepository.AddBooking(booking);
            _logger.LogInformation("Vehicle with ID {VehicleId} rented successfully to customer {CustomerId}", vehicleId, customerId);

            return booking.Id; // Regresar el ID de la reserva
        }

        public void ReturnVehicle(string bookingId)
        {
            _logger.LogInformation("Attempting to return vehicle for booking ID {BookingId}", bookingId);

            var booking = _bookingRepository.GetBookingById(bookingId);
            if (booking == null || booking.EndDate.HasValue)
            {
                _logger.LogWarning("Invalid booking ID {BookingId}.", bookingId);
                throw new Exception("Invalid booking.");
            }

            booking.EndDate = DateTime.UtcNow;
            var vehicle = _vehicleRepository.GetVehicleById(booking.VehicleId);
            vehicle.IsRented = false;

            _bookingRepository.UpdateBooking(booking);
            _vehicleRepository.AddVehicle(vehicle);
            _logger.LogInformation("Vehicle for booking ID {BookingId} returned successfully.", bookingId);
        }
    }
}
