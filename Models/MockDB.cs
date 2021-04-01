using ReathUIv0._3.Connections;
using ReathUIv0._3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReathUIv0._3
{
    internal class MockDB
    {
        //private Dictionary<string, ManufacturingCost> manufacturingCosts = new Dictionary<string, ManufacturingCost>();
        //private Dictionary<string, DisposalCost> disposalCosts = new Dictionary<string, DisposalCost>();
        //private Dictionary<string, TransportCost> transportCosts = new Dictionary<string, TransportCost>();

        private List<Material> manufacturingCosts = SqliteDatabaseAccess.RetreiveMaterial();
        private List<Disposal> disposalCosts = SqliteDatabaseAccess.LoadDisposal();
        private List<Transport> transportCosts = SqliteDatabaseAccess.LoadTransport();


        public MockDB()
        {
            /*
            List<string> manufacturing = File.ReadAllLines("manufacturing.csv").Skip(1).ToList();
            foreach (string entry in manufacturing)
            {
                string[] inputs = entry.Split(',');
                manufacturingCosts[inputs[0]] = new ManufacturingCost(inputs[0], EmptyToInv(inputs[1]), EmptyToInv(inputs[2]), EmptyToInv(inputs[3]), EmptyToInv(inputs[4]));
            }

            List<string> disposal = File.ReadAllLines("disposal.csv").Skip(1).ToList();
            foreach (string entry in disposal)
            {
                string[] inputs = entry.Split(',');
                disposalCosts[inputs[0]] = new DisposalCost(inputs[0], EmptyToInv(inputs[2]), EmptyToInv(inputs[3]), EmptyToInv(inputs[4]), EmptyToInv(inputs[5]), EmptyToInv(inputs[6]), EmptyToInv(inputs[7]), EmptyToInv(inputs[8]));
            }
            

            List<string> transport = File.ReadAllLines("freighting.csv").Skip(1).ToList();
            foreach (string entry in transport)
            {
                string[] inputs = entry.Split(',');
                transportCosts[inputs[0]] = new TransportCost(inputs[0], EmptyToInv(inputs[1]), EmptyToInv(inputs[2]));
            }

            */

            manufacturingCosts = SqliteDatabaseAccess.RetreiveMaterial();
            disposalCosts = SqliteDatabaseAccess.LoadDisposal();
            transportCosts = SqliteDatabaseAccess.LoadTransport();
        }

        public Material GetManufacturingCost(string MaterialName)
        {
            /*
            if (manufacturingCosts.ContainsKey(MaterialName))
            {
                return manufacturingCosts[MaterialName];
            }
            else throw new ArgumentException("Material Doesn't Exist");
            */

            foreach(Material mat in manufacturingCosts)
            {
                if (mat.ManufacturingMaterial.Equals(MaterialName) == true)
                {
                    return mat;
                }
            }

            return null;
        }

        public Disposal GetDisposalCost(string MaterialName)
        {
            /*
            if (disposalCosts.ContainsKey(MaterialName))
            {
                return disposalCosts[MaterialName];
            }
            else throw new ArgumentException("Material Doesn't Exist");
            */

            foreach (Disposal dispo in disposalCosts)
            {
                if (dispo.Material.Equals(MaterialName) == true)
                {
                    return dispo;
                }
            }

            return null;
        }

        public Transport GetTransportCost(string MaterialName)
        {
            /*
            if (transportCosts.ContainsKey(MaterialName))
            {
                return transportCosts[MaterialName];
            }
            else throw new ArgumentException("Material Doesn't Exist");
            */
            foreach (Transport trans in transportCosts)
            {
                if (trans.VehicleName.Equals(MaterialName) == true)
                {
                    return trans;
                }
            }

            return null;
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
    /*
    internal struct ManufacturingCost
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

    internal struct DisposalCost
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

    internal struct TransportCost
    {
        public string TransportName;
        public float Cost;
        public float WTTFactor;

        public TransportCost(string Name, float CostKM, float WTT)
        {
            TransportName = Name;
            Cost = CostKM;
            WTTFactor = WTT;
        }
    }
    */
}