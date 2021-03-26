using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReathUIv0._3.Models
{
    public class Transport
    {
        public string VehicleName { get; set; }
        public float CarbonCost { get; set; }
        public float WttConvFactor { get; set; }

        public Transport(string name,float cost,float convFactor)
        {
            VehicleName = name;
            CarbonCost = cost;
            WttConvFactor = convFactor;
        }
    }
}
