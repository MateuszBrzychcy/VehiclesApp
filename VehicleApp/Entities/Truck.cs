using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleApp.Entities
{
    public class Truck : Vehicle
    {
        public override string ToString() => $"{Brand} {Name} (Truck)";
    }
}
