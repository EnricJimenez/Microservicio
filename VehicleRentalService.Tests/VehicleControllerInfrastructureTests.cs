using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VehicleRentalService.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class VehicleControllerInfrastructureTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public VehicleControllerInfrastructureTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddVehicle_ShouldReturnBadRequest_WhenModelIsInvalid()
    {
        // Configuración de un modelo de vehículo inválido (por ejemplo, sin Id o sin ManufacturingDate)
        var invalidVehicle = new { Id = "", ManufacturingDate = "" };

        // Realiza una solicitud POST al endpoint /api/vehicle con un modelo inválido
        var response = await _client.PostAsJsonAsync("/api/vehicle", invalidVehicle);

        // Verifico de que la respuesta es BadRequest (código 400)
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddVehicle_ShouldReturnOk_WhenModelIsValid()
    {
        // Configuración de un modelo de vehículo válido
        var validVehicle = new { Id = "123", ManufacturingDate = "2021-08-20" };

        // Solicitud POST al endpoint /api/vehicle con un modelo válido
        var response = await _client.PostAsJsonAsync("/api/vehicle", validVehicle);

        // Verifico de que la respuesta es Ok (código 200)
        response.EnsureSuccessStatusCode();
    }
}
