using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReathUIv0._3.Models
{
    /// <summary>
    /// Class representing the carbon cost factors associated with various methods of freighting goods. 
    /// All factors are measured in kgCO2e per tonne of the material transported per kilometer of distance.
    /// </summary>
    public class Transport
    {
        public string TransportMethod = string.Empty;
        public float TravelFactor = 0.0f;
        public float WTTFactor = 0.0f;

        public Transport(string name, float cost, float convFactor)
        {
            TransportMethod = name;
            TravelFactor = cost;
            WTTFactor = convFactor;
        }
    }
}
