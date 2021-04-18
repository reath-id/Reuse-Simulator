using ReathUIv0._3.Connections;
using ReathUIv0._3.Models;
using System;
using System.Collections.Generic;
using static ReathUIv0._3.Models.ReusableAsset;

namespace ReathUIv0._3
{
    internal static class CarbonCalculation
    {
        internal const float NOT_PRESENT = 0.0f;

        public static List<Manufacturing> manufacturingCosts = SqliteDatabaseAccess.LoadManufacturing();
        public static List<Disposal> disposalCosts = SqliteDatabaseAccess.LoadDisposal();
        public static List<Transport> transportCosts = SqliteDatabaseAccess.LoadTransport();

        internal static class Testing
        {
            public static void addManufacturing(Manufacturing m)
            {
                manufacturingCosts.Add(m);
            }

            public static void addDisposal(Disposal m)
            {
                disposalCosts.Add(m);
            }

            public static void addTransport(Transport m)
            {
                transportCosts.Add(m);
            }
        }

        internal static class Detail
        {
            public static float GetManufacturingCost(string material, ManufactoringMethod method, float weight, float noofitems)
            {
                Manufacturing MatMFC = CarbonCalculation.GetManufacturingCost(material);
                float CarbonFactor = ManufacturingCostFromEnum(MatMFC, method);

                return CarbonFactor * 0.001f * weight * noofitems;             
            }

            public static float GetDisposalCost(string material, DisposalMethod method, float weight, float noofitems)
            {
                Disposal MatDSC = CarbonCalculation.GetDisposalCost(material);
                float DisposalFactor = DisposalCostFromEnum(MatDSC, method);
                
                return DisposalFactor * 0.001f * weight * noofitems;
            }

            public static float GetTransportCost(string vehicle, float weight, float noofitems, float avgdist)
            {
                Transport Transport = CarbonCalculation.GetTransportCost(vehicle);
                float TotalWeight = noofitems * weight;

                return avgdist * TotalWeight * 0.001f * (Transport.TravelFactor + Transport.WTTFactor);
            }
        }

        public static CarbonResults CalculateCarbon(ReusableAsset Asset)
        {

            manufacturingCosts = SqliteDatabaseAccess.LoadManufacturing();
            disposalCosts = SqliteDatabaseAccess.LoadDisposal();
            transportCosts = SqliteDatabaseAccess.LoadTransport();

            if (Asset.MaximumReuses < 1)
            {
                throw new ArgumentException("Asset needs to be able to be used at least once.");
            }

            float Mat1ManufacturingCost = Detail.GetManufacturingCost(Asset.PrimaryMaterial, Asset.PrimaryManufacturingMethod, Asset.PrimaryWeight, Asset.SampleSize);
            float Mat2ManufacturingCost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxiliaryMaterial))
            {
                Mat2ManufacturingCost = Detail.GetManufacturingCost(Asset.AuxiliaryMaterial, Asset.AuxiliaryManufacturingMethod, Asset.AuxiliaryWeight, Asset.SampleSize);
            }

            float ManufacturingCost = Mat1ManufacturingCost + Mat2ManufacturingCost;

            /// DISPOSAL COSTS         
            float Mat1DisposalCost = Detail.GetDisposalCost(Asset.PrimaryMaterial, Asset.PrimaryDisposalMethod, Asset.PrimaryWeight, Asset.SampleSize);
            float Mat2DisposalCost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxiliaryMaterial))
            {
                Mat2DisposalCost = Detail.GetDisposalCost(Asset.AuxiliaryMaterial, Asset.AuxiliaryDisposalMethod, Asset.AuxiliaryWeight, Asset.SampleSize);
            }

            float DisposalCost = Mat1DisposalCost + Mat2DisposalCost;

            /// LINEAR COST
            float LinearCost = ManufacturingCost + DisposalCost;
            float Mat1LinearCost = Mat1ManufacturingCost + Mat1DisposalCost;
            float Mat2LinearCost = Mat2ManufacturingCost + Mat2DisposalCost;

            /// REUSE MODEL
            float ReuseManufacturingCost = ManufacturingCost / Asset.MaximumReuses;
            float Mat1ReuseManufacturingCost = Mat1ManufacturingCost / Asset.MaximumReuses;
            float Mat2ReuseManufacturingCost = Mat2ManufacturingCost / Asset.MaximumReuses;

            /// BACKHAUL COST
            /// 

            float avgDistance = (float)Asset.AverageDistanceToReuse;

            float Mat1TransportationCost = Detail.GetTransportCost("HGV", Asset.PrimaryWeight, Asset.SampleSize, avgDistance);              
            float Mat2TransportationCost = Detail.GetTransportCost("HGV", Asset.AuxiliaryWeight, Asset.SampleSize, avgDistance);
            float TransportationCost = Mat1TransportationCost + Mat2TransportationCost;

            /// PREPARATION FOR REUSE COST
            float PrepReuseCost = Asset.PercentageOfManufacturingCarbon / 100 * ManufacturingCost;
            float Mat1PrepReuseCost = Asset.PercentageOfManufacturingCarbon / 100 * Mat1ManufacturingCost;
            float Mat2PrepReuseCost = Asset.PercentageOfManufacturingCarbon / 100 * Mat2ManufacturingCost;

            /// REUSE DISPOSAL
            float ReuseDisposalCost = DisposalCost / Asset.MaximumReuses;
            float Mat1ReuseDisposalCost = Mat1DisposalCost / Asset.MaximumReuses;
            float Mat2ReuseDisposalCost = Mat2DisposalCost / Asset.MaximumReuses;

            /// CIRCULAR COST
            float CircularCost = ReuseManufacturingCost + TransportationCost + PrepReuseCost + ReuseDisposalCost;
            float Mat1CircularCost = Mat1ReuseManufacturingCost + Mat1TransportationCost + Mat1PrepReuseCost + Mat1ReuseDisposalCost;
            float Mat2CircularCost = Mat2ReuseManufacturingCost + Mat2TransportationCost + Mat2PrepReuseCost + Mat2ReuseDisposalCost;

            CarbonMaterialResults Mat1 = new CarbonMaterialResults(Asset.PrimaryMaterial, Mat1LinearCost, Mat1CircularCost, Mat1ManufacturingCost, Mat1DisposalCost, Mat1TransportationCost);
            CarbonMaterialResults Mat2 = new CarbonMaterialResults(Asset.AuxiliaryMaterial, Mat2LinearCost, Mat2CircularCost, Mat2ManufacturingCost, Mat2DisposalCost, Mat2TransportationCost);
            CarbonMaterialResults Total = new CarbonMaterialResults(Asset.AssetName, LinearCost, CircularCost, ManufacturingCost, DisposalCost, TransportationCost);

            return new CarbonResults(Mat1, Mat2, Total);
        }

        public static float ManufacturingCostFromEnum(Manufacturing cost, ManufactoringMethod method)
        {
            switch (method)
            {
                case ManufactoringMethod.Primary:
                    if (cost.Primary != NOT_PRESENT)
                    {
                        return cost.Primary;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be produced raw.");
                case ManufactoringMethod.Reused:
                    if (cost.Reused != NOT_PRESENT)
                    {
                        return cost.Reused;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be Reused.");
                case ManufactoringMethod.ClosedLoop:
                    if (cost.ClosedLoop != NOT_PRESENT)
                    {
                        return cost.ClosedLoop;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be acquired from a Closed Loop.");
                case ManufactoringMethod.OpenLoop:
                    if (cost.OpenLoop != NOT_PRESENT)
                    {
                        return cost.OpenLoop;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be acquired from an Open Loop.");
                default:
                    throw new ArgumentException(cost.Material + ": Invalid manufacturing method.");
            }
        }

        public static float DisposalCostFromEnum(Disposal cost, DisposalMethod method)
        {
            switch (method)
            {
                case DisposalMethod.Landfill:
                    if (cost.Landfill != NOT_PRESENT)
                    {
                        return cost.Landfill;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be disposed to reuse.");
                case DisposalMethod.Reuse:
                    if (cost.Reuse != NOT_PRESENT)
                    {
                        return cost.Reuse;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be disposed to reuse.");
                case DisposalMethod.ClosedLoop:
                    if (cost.ClosedLoop != NOT_PRESENT)
                    {
                        return cost.ClosedLoop;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be disposed into a Closed Loop.");
                case DisposalMethod.OpenLoop:
                    if (cost.OpenLoop != NOT_PRESENT)
                    {
                        return cost.OpenLoop;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be disposed into an Open Loop.");
                case DisposalMethod.Combustion:
                    if (cost.Combustion != NOT_PRESENT)
                    {
                        return cost.Combustion;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be combusted.");
                case DisposalMethod.Composting:
                    if (cost.Composting != NOT_PRESENT)
                    {
                        return cost.Composting;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be composed.");
                case DisposalMethod.Anaerobic:
                    if (cost.AnaerobicDigestion != NOT_PRESENT)
                    {
                        return cost.AnaerobicDigestion;
                    }
                    else throw new ArgumentException(cost.Material + " cannot be digested anaerobically.");                          
                default:
                    throw new ArgumentException(cost.Material + ": Invalid disposal method.");
            }
        }

        public static Manufacturing GetManufacturingCost(string MaterialName)
        {

            foreach (Manufacturing mat in manufacturingCosts)
            {
                if (mat.Material.Equals(MaterialName) == true)
                {
                    return mat;
                }
            }

            throw new ArgumentException("Provided material: " + MaterialName + " does not have any manufacturing cost associated with it.");
        }

        public static Disposal GetDisposalCost(string DisposalName)
        {

            foreach (Disposal dispo in disposalCosts)
            {
                if (dispo.Material.Equals(DisposalName) == true)
                {
                    return dispo;
                }
            }

            throw new ArgumentException("Provided material: " + DisposalName + " does not have any disposal cost associated with it.");
        }

        public static Transport GetTransportCost(string TransportName)
        {
            foreach (Transport trans in transportCosts)
            {
                if (trans.TransportMethod.Equals(TransportName) == true)
                {
                    return trans;
                }
            }

            throw new ArgumentException("Provided transportation method: " + TransportName + " does not have any transport cost associated with it..");
        }
    }


    public struct CarbonMaterialResults
    {
        public string Name;
        public float LinearCarbon;
        public float CircularCarbon;
        public float ManufacturingCarbon;
        public float DisposalCarbon;
        public float TransportCarbon;
        public float ReuseAsPercent;

        public CarbonMaterialResults(string Name_, float Linear, float Circular, float Manufacturing, float Disposal, float Transport)
        {
            Name = Name_;
            LinearCarbon = Linear;
            CircularCarbon = Circular;
            ManufacturingCarbon = Manufacturing;
            DisposalCarbon = Disposal;
            TransportCarbon = Transport;
            ReuseAsPercent = (1 - (Circular / Linear)) * 100;
        }
    }

    public struct CarbonResults
    {
        public CarbonMaterialResults Primary;
        public CarbonMaterialResults Auxiliary;
        public CarbonMaterialResults Total;

        public CarbonResults(CarbonMaterialResults Mat1, CarbonMaterialResults Mat2, CarbonMaterialResults Total_)
        {
            Primary = Mat1;
            Auxiliary = Mat2;
            Total = Total_;
        }
    }
}