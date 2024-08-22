FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["VehicleRentalService.API/VehicleRentalService.API.csproj", "VehicleRentalService.API/"]
COPY ["VehicleRentalService.Application/VehicleRentalService.Application.csproj", "VehicleRentalService.Application/"]
COPY ["VehicleRentalService.Core/VehicleRentalService.Core.csproj", "VehicleRentalService.Core/"]
COPY ["VehicleRentalService.Infrastructure/VehicleRentalService.Infrastructure.csproj", "VehicleRentalService.Infrastructure/"]

RUN dotnet restore "VehicleRentalService.API/VehicleRentalService.API.csproj"


COPY . .


WORKDIR "/src/VehicleRentalService.API"
RUN dotnet build -c Release -o /app/build


FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VehicleRentalService.API.dll"]
