using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleApp.Entities
{
    public class Car : Vehicle
    {
        public int NumberOfSeats { get; set; }
        public string? BodyType { get; set; }
        public override string ToString() => $"{Brand} {Name} (Car)";

    }
}
