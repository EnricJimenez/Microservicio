namespace VehicleRentalService.Core.Entities
{
    public class Vehicle
    {
        public string Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public bool IsRented { get; set; }

        public Vehicle()
        {
            Id = string.Empty;
            Make = string.Empty;
            Model = string.Empty;
        }

        public bool IsAvailable() 
        {
            return !IsRented && ManufacturingDate.AddYears(5) > DateTime.UtcNow;
        }
    }
}

