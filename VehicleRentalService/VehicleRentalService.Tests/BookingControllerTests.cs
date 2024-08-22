using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VehicleRentalService.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;
// La clase BookingControllerTests es una prueba funcional de integración para verificar que los endpoints del controlador de Booking funcionan correctamente.
public class BookingControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    // HttpClient se utiliza para realizar solicitudes HTTP en el entorno de prueba.
    private readonly HttpClient _client;

    // Constructor que inicializa el cliente HTTP utilizando la fábrica de aplicaciones Web.
    public BookingControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // Prueba que verifica que el endpoint de RentVehicle devuelve un código de estado ok (200 OK).
    [Fact]
    public async Task RentVehicleEndpoint_ShouldReturnOk()
    {
        // Configuración de la solicitud de alquiler de vehículo.
        var rentRequest = new { VehicleId = "1", CustomerId = "customerId" };

        // Realiza una solicitud POST al endpoint /api/booking/rent con los datos del vehículo y cliente.
        var response = await _client.PostAsJsonAsync("/api/booking/rent", rentRequest);

        // Verificación de que la respuesta es ok (código de estado 200 OK).
        response.EnsureSuccessStatusCode();
    }
    // Prueba que verifica que el endpoint de RentVehicle devuelve un código de estado BadRequest (400) cuando el modelo es inválido.
    [Fact]
    public async Task RentVehicleEndpoint_ShouldReturnBadRequest_WhenModelIsInvalid()
    {
        // Configuración de un modelo inválido (por ejemplo, sin VehicleId)
        var rentRequest = new { CustomerId = "customerId" }; // Falta VehicleId

        // Realiza una solicitud POST al endpoint /api/booking/rent con un modelo inválido
        var response = await _client.PostAsJsonAsync("/api/booking/rent", rentRequest);

        // Verificación de que la respuesta es BadRequest (código de estado 400)
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}


