namespace VehicleApp.Entities
{
    public abstract class Vehicle : EntityBase
    {
        public string? Brand { get; set; }
        public string? YearOfManufacture { get; set; }
        public string? Name { get; set; }
        public FuelType FuelType { get; set; }       
    }
    public enum FuelType
    {
        Petrol = 1,
        Diesel = 2
    };
}
