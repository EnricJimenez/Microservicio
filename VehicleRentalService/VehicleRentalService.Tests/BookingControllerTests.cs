using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VehicleRentalService.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;
// La clase BookingControllerTests es una prueba funcional de integraci�n para verificar que los endpoints del controlador de Booking funcionan correctamente.
public class BookingControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    // HttpClient se utiliza para realizar solicitudes HTTP en el entorno de prueba.
    private readonly HttpClient _client;

    // Constructor que inicializa el cliente HTTP utilizando la f�brica de aplicaciones Web.
    public BookingControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // Prueba que verifica que el endpoint de RentVehicle devuelve un c�digo de estado ok (200 OK).
    [Fact]
    public async Task RentVehicleEndpoint_ShouldReturnOk()
    {
        // Configuraci�n de la solicitud de alquiler de veh�culo.
        var rentRequest = new { VehicleId = "1", CustomerId = "customerId" };

        // Realiza una solicitud POST al endpoint /api/booking/rent con los datos del veh�culo y cliente.
        var response = await _client.PostAsJsonAsync("/api/booking/rent", rentRequest);

        // Verificaci�n de que la respuesta es ok (c�digo de estado 200 OK).
        response.EnsureSuccessStatusCode();
    }
    // Prueba que verifica que el endpoint de RentVehicle devuelve un c�digo de estado BadRequest (400) cuando el modelo es inv�lido.
    [Fact]
    public async Task RentVehicleEndpoint_ShouldReturnBadRequest_WhenModelIsInvalid()
    {
        // Configuraci�n de un modelo inv�lido (por ejemplo, sin VehicleId)
        var rentRequest = new { CustomerId = "customerId" }; // Falta VehicleId

        // Realiza una solicitud POST al endpoint /api/booking/rent con un modelo inv�lido
        var response = await _client.PostAsJsonAsync("/api/booking/rent", rentRequest);

        // Verificaci�n de que la respuesta es BadRequest (c�digo de estado 400)
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}


