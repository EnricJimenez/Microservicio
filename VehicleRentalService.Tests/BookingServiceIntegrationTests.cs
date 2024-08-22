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
        // Creación de repositorios en memoria
        var vehicleRepo = new InMemoryVehicleRepository();
        var bookingRepo = new InMemoryBookingRepository();
        
        // Creación de un mock para ILogger<BookingService>
        var loggerMock = new Mock<ILogger<BookingService>>();

        // Instanciar el servicio con los mocks y repositorios
        var service = new BookingService(vehicleRepo, bookingRepo, loggerMock.Object);

        // Creación de un vehículo disponible
        var vehicle = new Vehicle
        {
            Id = "1",
            IsRented = false,
            ManufacturingDate = DateTime.UtcNow.AddYears(-3) // El vehículo tiene menos de 5 años
        };
        vehicleRepo.AddVehicle(vehicle);

        // Alquilar el vehículo
        service.RentVehicle("1", "customerId");

        // Obtener el booking creado
        var booking = bookingRepo.GetActiveBookingByCustomerId("customerId");

        // Verificación de que el booking no es nulo antes de acceder a sus propiedades
        Assert.NotNull(booking); // Esto asegurará que booking no sea null

        // Verificación de que el vehículo esté alquilado
        Assert.True(vehicle.IsRented);

        // Devolver el vehículo usando el BookingId correcto
        service.ReturnVehicle(booking!.Id); // Uso del operador `!` porque Assert.NotNull garantiza que booking no es null

        // Verificación de que el vehículo esté disponible nuevamente
        Assert.False(vehicle.IsRented);
    }
}


