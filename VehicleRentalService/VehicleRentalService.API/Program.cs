using Microsoft.Extensions.DependencyInjection;
using VehicleRentalService.Application.Services;
using VehicleRentalService.Core.Interfaces;
using VehicleRentalService.Core.Entities;
using VehicleRentalService.Infrastructure.Repositories;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("VehicleRentalService.Tests")]

namespace VehicleRentalService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<FleetManagementService>();
            builder.Services.AddScoped<BookingService>();

            builder.Services.AddSingleton<IVehicleRepository, InMemoryVehicleRepository>();
            builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();

            var app = builder.Build();

            // Añadir vehículo al repositorio en memoria
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var vehicleRepo = services.GetRequiredService<IVehicleRepository>();

                // Añadir un vehículo con ID = "1"
                var vehicle = new Vehicle
                {
                    Id = "1",
                    IsRented = false,
                    ManufacturingDate = DateTime.UtcNow.AddYears(-3) // El vehículo tiene menos de 5 años
                };

                vehicleRepo.AddVehicle(vehicle);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


