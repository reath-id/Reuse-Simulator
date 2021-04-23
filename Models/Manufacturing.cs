using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReathUIv0._3.Models
{
    /// <summary>
    /// Class representing the carbon cost factors associated with various methods of manufacturing a given material. 
    /// All factors are measured in kgCO2e per tonne of the material.
    /// </summary>
    public class Manufacturing
    {
        public string Material = string.Empty;
        public float Primary = CarbonCalculation.NOT_PRESENT;
        public float Reused = CarbonCalculation.NOT_PRESENT;
        public float OpenLoop = CarbonCalculation.NOT_PRESENT;
        public float ClosedLoop = CarbonCalculation.NOT_PRESENT;

        public Manufacturing(string manuMaterial, float production, float reuse, float openSource, float closedSource)
        {
            Material = manuMaterial;
            Primary = production;
            Reused = reuse;
            OpenLoop = openSource;
            ClosedLoop = closedSource;
        }
    }

    

}
