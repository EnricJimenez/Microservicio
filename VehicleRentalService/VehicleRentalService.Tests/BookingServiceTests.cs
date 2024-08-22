using VehicleRentalService.Core.Entities;
using VehicleRentalService.Core.Interfaces;
using VehicleRentalService.Application.Services;
using System;
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;

namespace VehicleRentalService.Tests
{
    // Clase de pruebas para el servicio de reservas (BookingService)
    public class BookingServiceTests
    {
        [Fact]
        public void RentVehicle_ShouldThrowException_WhenVehicleIsNotAvailable()
        {
            // Prueba unitaria que verifica que se lanza una excepci�n cuando el veh�culo no est� disponible

            var mockVehicleRepo = new Mock<IVehicleRepository>(); // Creaci�n de un repositorio simulado de veh�culos
            var mockBookingRepo = new Mock<IBookingRepository>(); // Creaci�n de un repositorio simulado de reservas
            var loggerMock = new Mock<ILogger<BookingService>>(); // Creaci�n de un logger simulado

            // Inicializaci�n del servicio de reservas con los repositorios simulados y el logger
            var service = new BookingService(mockVehicleRepo.Object, mockBookingRepo.Object, loggerMock.Object);

            var vehicle = new Vehicle { Id = "1", IsRented = true }; // Creaci�n de un veh�culo simulado que ya est� alquilado

            // Configuraci�n del comportamiento del repositorio simulado para devolver el veh�culo ya alquilado
            mockVehicleRepo.Setup(v => v.GetVehicleById("1")).Returns(vehicle);

            //Realizaci�n y verificaci�n de que se lanza la excepci�n esperada
            Assert.Throws<Exception>(() => service.RentVehicle("1", "customerId"));
        }
        [Fact]
        public void RentVehicle_ShouldThrowException_WhenCustomerAlreadyHasAnActiveBooking()
        {
            // prueba que verifica que se lanza una excepci�n cuando el cliente ya tiene una reserva activa.

            var mockVehicleRepo = new Mock<IVehicleRepository>(); // Creaci�n de un repositorio simulado de veh�culos
            var mockBookingRepo = new Mock<IBookingRepository>(); // Creaci�n de un repositorio simulado de reservas
            var mockLogger = new Mock<ILogger<BookingService>>();

            var service = new BookingService(mockVehicleRepo.Object, mockBookingRepo.Object, mockLogger.Object); // Inicializaci�n del servicio de reservas con los repositorios simulados
            var vehicle = new Vehicle { Id = "1", IsRented = false }; // Creaci�n de un veh�culo simulado disponible
            var activeBooking = new Booking { Id = "booking1", VehicleId = "1", CustomerId = "customerId", StartDate = DateTime.UtcNow };

            // Configuraci�n del comportamiento del repositorio simulado
            mockVehicleRepo.Setup(v => v.GetVehicleById("1")).Returns(vehicle);
            mockBookingRepo.Setup(b => b.GetActiveBookingByCustomerId("customerId")).Returns(activeBooking);

            // Act & Assert - Realizaci�n y verificaci�n de que se lanza la excepci�n esperada
            Assert.Throws<Exception>(() => service.RentVehicle("1", "customerId"));
        }
    }
}

