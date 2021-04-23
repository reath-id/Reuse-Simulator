using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReathUIv0._3.Models
{
    /// <summary>
    /// Class representing the carbon cost factors associated with various methods of disposing a given material. 
    /// All factors are measured in kgCO2e per tonne of the material.
    /// </summary>
    public class Disposal
    {
        public string Material = string.Empty;
        public float Reuse = CarbonCalculation.NOT_PRESENT;
        public float OpenLoop = CarbonCalculation.NOT_PRESENT;
        public float ClosedLoop = CarbonCalculation.NOT_PRESENT;
        public float Combustion = CarbonCalculation.NOT_PRESENT;
        public float Composting = CarbonCalculation.NOT_PRESENT;
        public float Landfill = CarbonCalculation.NOT_PRESENT;
        public float AnaerobicDigestion = CarbonCalculation.NOT_PRESENT;

        public Disposal(string materialOption, float reuse, float openLoop, float closedLoop, float combustion, float composting, float landfill, float anaerobicDigestion)
        {
            Material = materialOption;
            Reuse = reuse;
            OpenLoop = openLoop;
            ClosedLoop = closedLoop;
            Combustion = combustion;
            Composting = composting;
            Landfill = landfill;
            AnaerobicDigestion = anaerobicDigestion;
        }

    }
}
