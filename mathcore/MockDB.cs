using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace mathcore
{
    class MockDB
    {
        Dictionary<string, ManufacturingCost> manufacturingCosts = new Dictionary<string, ManufacturingCost>();
        Dictionary<string, DisposalCost> disposalCosts = new Dictionary<string, DisposalCost>();
        Dictionary<string, TransportCost> transportCosts = new Dictionary<string, TransportCost>();

        public MockDB(string Path)
        {
            List<string> manufacturing = File.ReadAllLines(Path + "/manufacturing.csv").Skip(1).ToList();
            foreach (string entry in manufacturing)
            {
                string[] inputs = entry.Split(',');
                manufacturingCosts[inputs[0]] = new ManufacturingCost(inputs[0], EmptyToInv(inputs[1]), EmptyToInv(inputs[2]), EmptyToInv(inputs[3]), EmptyToInv(inputs[4]));
            }

            List<string> disposal = File.ReadAllLines(Path + "/disposal.csv").Skip(1).ToList();
            foreach (string entry in disposal)
            {
                string[] inputs = entry.Split(',');
                disposalCosts[inputs[0]] = new DisposalCost(inputs[0], EmptyToInv(inputs[2]), EmptyToInv(inputs[3]), EmptyToInv(inputs[4]), EmptyToInv(inputs[5]), EmptyToInv(inputs[6]), EmptyToInv(inputs[7]), EmptyToInv(inputs[8]));
            }

            List<string> transport = File.ReadAllLines(Path + "/freighting.csv").Skip(1).ToList();
            foreach (string entry in transport)
            {
                string[] inputs = entry.Split(',');
                transportCosts[inputs[0]] = new TransportCost(inputs[0], EmptyToInv(inputs[1]));
            }
        }


        public ManufacturingCost GetManufacturingCost(string MaterialName)
        {
            if (manufacturingCosts.ContainsKey(MaterialName))
            {
                return manufacturingCosts[MaterialName];
            }
            else throw new ArgumentException("Material Doesn't Exist");
        }

        public DisposalCost GetDisposalCost(string MaterialName)
        {
            if (disposalCosts.ContainsKey(MaterialName))
            {
                return disposalCosts[MaterialName];
            }
            else throw new ArgumentException("Material Doesn't Exist");
        }

        public TransportCost GetTransportCost(string MaterialName)
        {
            if (transportCosts.ContainsKey(MaterialName))
            {
                return transportCosts[MaterialName];
            }
            else throw new ArgumentException("Material Doesn't Exist");
        }

        private float EmptyToInv(string field)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return -1;
            }
            else return float.Parse(field);
        }

    }

    struct ManufacturingCost
    {
        public string MaterialName;
        public float PrimaryProduction;
        public float Reused;
        public float OpenLoop;
        public float ClosedLoop;

        public ManufacturingCost(string Name, float Primary, float Reuse, float Open, float Closed)
        {
            MaterialName = Name;
            PrimaryProduction = Primary;
            Reused = Reuse;
            OpenLoop = Open;
            ClosedLoop = Closed;
        }
    }

    struct DisposalCost
    {
        public string MaterialName;
        public float Reuse;
        public float OpenLoop;
        public float ClosedLoop;
        public float Combustion;
        public float Composting;
        public float Landfill;
        public float AnaerobicDigestion;

        public DisposalCost(string Name, float Reusing, float Open, float Closed, float Combust, float Compost, float Land, float Anaerobic)
        {
            MaterialName = Name;
            Reuse = Reusing;
            OpenLoop = Open;
            ClosedLoop = Closed;
            Combustion = Combust;
            Composting = Compost;
            Landfill = Land;
            AnaerobicDigestion = Anaerobic;
        }
    }

    struct TransportCost
    {
        public string TransportName;
        public float Cost;

        public TransportCost(string Name, float CostKM)
        {
            TransportName = Name;
            Cost = CostKM;
        }
    }
}
