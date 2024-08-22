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
            // Prueba unitaria que verifica que se lanza una excepción cuando el vehículo no está disponible

            var mockVehicleRepo = new Mock<IVehicleRepository>(); // Creación de un repositorio simulado de vehículos
            var mockBookingRepo = new Mock<IBookingRepository>(); // Creación de un repositorio simulado de reservas
            var loggerMock = new Mock<ILogger<BookingService>>(); // Creación de un logger simulado

            // Inicialización del servicio de reservas con los repositorios simulados y el logger
            var service = new BookingService(mockVehicleRepo.Object, mockBookingRepo.Object, loggerMock.Object);

            var vehicle = new Vehicle { Id = "1", IsRented = true }; // Creación de un vehículo simulado que ya está alquilado

            // Configuración del comportamiento del repositorio simulado para devolver el vehículo ya alquilado
            mockVehicleRepo.Setup(v => v.GetVehicleById("1")).Returns(vehicle);

            //Realización y verificación de que se lanza la excepción esperada
            Assert.Throws<Exception>(() => service.RentVehicle("1", "customerId"));
        }
        [Fact]
        public void RentVehicle_ShouldThrowException_WhenCustomerAlreadyHasAnActiveBooking()
        {
            // prueba que verifica que se lanza una excepción cuando el cliente ya tiene una reserva activa.

            var mockVehicleRepo = new Mock<IVehicleRepository>(); // Creación de un repositorio simulado de vehículos
            var mockBookingRepo = new Mock<IBookingRepository>(); // Creación de un repositorio simulado de reservas
            var mockLogger = new Mock<ILogger<BookingService>>();

            var service = new BookingService(mockVehicleRepo.Object, mockBookingRepo.Object, mockLogger.Object); // Inicialización del servicio de reservas con los repositorios simulados
            var vehicle = new Vehicle { Id = "1", IsRented = false }; // Creación de un vehículo simulado disponible
            var activeBooking = new Booking { Id = "booking1", VehicleId = "1", CustomerId = "customerId", StartDate = DateTime.UtcNow };

            // Configuración del comportamiento del repositorio simulado
            mockVehicleRepo.Setup(v => v.GetVehicleById("1")).Returns(vehicle);
            mockBookingRepo.Setup(b => b.GetActiveBookingByCustomerId("customerId")).Returns(activeBooking);

            // Act & Assert - Realización y verificación de que se lanza la excepción esperada
            Assert.Throws<Exception>(() => service.RentVehicle("1", "customerId"));
        }
    }
}

