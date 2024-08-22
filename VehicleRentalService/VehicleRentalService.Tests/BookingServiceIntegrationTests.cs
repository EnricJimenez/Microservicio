using Moq;
using VehicleRentalService.Application.Services;
using VehicleRentalService.Core.Entities;
using VehicleRentalService.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Xunit;

public class BookingServiceIntegrationTests
{
    [Fact]
    public void RentAndReturnVehicle_ShouldWorkAsExpected()
    {
        // Creaci�n de repositorios en memoria
        var vehicleRepo = new InMemoryVehicleRepository();
        var bookingRepo = new InMemoryBookingRepository();

        // Creaci�n de un mock para ILogger<BookingService>
        var loggerMock = new Mock<ILogger<BookingService>>();

        // Instanciar el servicio con los mocks y repositorios
        var service = new BookingService(vehicleRepo, bookingRepo, loggerMock.Object);

        // Creaci�n de un veh�culo disponible
        var vehicle = new Vehicle
        {
            Id = "1",
            IsRented = false,
            ManufacturingDate = DateTime.UtcNow.AddYears(-3) // El veh�culo tiene menos de 5 a�os
        };
        vehicleRepo.AddVehicle(vehicle);

        // Alquilar el veh�culo
        service.RentVehicle("1", "customerId");

        // Obtener el booking creado
        var booking = bookingRepo.GetActiveBookingByCustomerId("customerId");

        // Verificaci�n de que el booking no es nulo antes de acceder a sus propiedades
        Assert.NotNull(booking); // Esto asegurar� que booking no sea null

        // Verificaci�n de que el veh�culo est� alquilado
        Assert.True(vehicle.IsRented);

        // Devolver el veh�culo usando el BookingId correcto
        service.ReturnVehicle(booking!.Id); // Uso del operador `!` porque Assert.NotNull garantiza que booking no es null

        // Verificaci�n de que el veh�culo est� disponible nuevamente
        Assert.False(vehicle.IsRented);
    }
}


