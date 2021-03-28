using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReathUIv0._3.Models
{
    public class Disposal
    {
        public string MaterialOption { get; set; }
        public float Conversion { get; set; }
        public float Reuse { get; set; }
        public float OpenLoop { get; set; }
        public float ClosedLoop { get; set; }
        public float Combustion { get; set; }
        public float Composting { get; set; }
        public float Landfill { get; set; }
        public float AnaerobicDigestion { get; set; }
        public float _none { get; set; }

        public Disposal()
        {
            MaterialOption = string.Empty;
            Conversion = 0.0000000F;
            Reuse = 00.0000F;
            OpenLoop = 00.000F;
            ClosedLoop = 00.000F;
            Combustion = 00.000F;
            Composting = 00.000F;
            Landfill = 0000.000F;
            AnaerobicDigestion = 00.000F;
            // Carlos did this - remove comment
            _none = 00.000F;

        }

        public Disposal(string materialOption,float conversion,float reuse, float openLoop,float closedLoop,float combustion,float composting, float landfill, float anaerobicDigestion, float none)
        {
            MaterialOption = materialOption;
            Conversion = conversion;
            Reuse = reuse;
            OpenLoop = openLoop;
            ClosedLoop = closedLoop;
            Combustion = combustion;
            Composting = composting;
            Landfill = landfill;
            AnaerobicDigestion = anaerobicDigestion;
            _none = none;
        }

    }
}
