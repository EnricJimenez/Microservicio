using VehicleRentalService.Core.Entities;


namespace VehicleRentalService.Core.Interfaces
{
    public interface IBookingRepository
    {
        void AddBooking(Booking booking);
        Booking? GetActiveBookingByCustomerId(string customerId);
        void UpdateBooking(Booking booking);
        Booking GetBookingById(string bookingId);
    }
}
