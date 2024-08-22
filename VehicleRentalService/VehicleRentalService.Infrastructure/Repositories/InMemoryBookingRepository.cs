using System.Collections.Generic;
using System.Linq;
using VehicleRentalService.Core.Entities;
using VehicleRentalService.Core.Interfaces;

namespace VehicleRentalService.Infrastructure.Repositories
{
    public class InMemoryBookingRepository : IBookingRepository
    {
        private readonly List<Booking> _bookings = new();

        public void AddBooking(Booking booking)
        {
            _bookings.Add(booking);
        }

        public Booking? GetActiveBookingByCustomerId(string customerId)
        {
            return _bookings.FirstOrDefault(b => b.CustomerId == customerId && b.EndDate == null);
        }

        public void UpdateBooking(Booking booking)
        {
            var existingBooking = _bookings.FirstOrDefault(b => b.Id == booking.Id);
            if (existingBooking != null)
            {
                existingBooking.EndDate = booking.EndDate;
            }
        }
        public Booking GetBookingById(string bookingId)
        {
            return _bookings.FirstOrDefault(b => b.Id == bookingId)!;
        }
    }
}