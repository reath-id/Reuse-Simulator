using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReathUIv0._3.Models
{

    // Class representing the carbon cost factors associated with various methods of disposing a given material.
    public class Disposal
    {
        public string Material { get; set; }
        public float Reuse { get; set; }
        public float OpenLoop { get; set; }
        public float ClosedLoop { get; set; }
        public float Combustion { get; set; }
        public float Composting { get; set; }
        public float Landfill { get; set; }
        public float AnaerobicDigestion { get; set; }

        public Disposal()
        {
            Material = string.Empty;
            Reuse = 0.0f;
            OpenLoop = 0.0f;
            ClosedLoop = 0.0f;
            Combustion = 0.0f;
            Composting = 0.0f;
            Landfill = 0.0f;
            AnaerobicDigestion = 0.0f;

        }

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
