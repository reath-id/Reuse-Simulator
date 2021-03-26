using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReathUIv0._3.Models
{
    public class Material
    {
        public string ManufacturingMaterial { get; set; }
        public float MaterialProduction { get; set; }
        public float Reused { get; set; }
        public float OpenLoopSource { get; set; }
        public float ClosedLoopSource { get; set; }

        public Material(string manuMaterial,float production,float reuse,float openSource,float closedSource)
        {
            ManufacturingMaterial = manuMaterial;
            MaterialProduction = production;
            Reused = reuse;
            OpenLoopSource = openSource;
            ClosedLoopSource = closedSource;
        }
    }
}
