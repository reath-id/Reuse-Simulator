using ReathUIv0._3.Connections;
using ReathUIv0._3.Models;
using System;
using System.Collections.Generic;
using static ReathUIv0._3.Models.ReusableAsset;

namespace ReathUIv0._3
{
    internal static class CarbonCalculation
    {
        private static List<Material> manufacturingCosts = SqliteDatabaseAccess.RetreiveMaterial();
        private static List<Disposal> disposalCosts = SqliteDatabaseAccess.LoadDisposal();
        private static List<Transport> transportCosts = SqliteDatabaseAccess.LoadTransport();

        public static CarbonResults CalculateCarbon(ReusableAsset Asset)
        {

            manufacturingCosts = SqliteDatabaseAccess.RetreiveMaterial();
            disposalCosts = SqliteDatabaseAccess.LoadDisposal();
            transportCosts = SqliteDatabaseAccess.LoadTransport();

            Material Mat1MFC = GetManufacturingCost(Asset.PrimaryMaterial);
            float Mat1CarbonFactor = ManufacturingCostFromEnum(Mat1MFC, Asset.PrimaryMaterialManufacturing);

            float Mat1ManufacturingCost = Mat1CarbonFactor * 0.001f * Asset.PrimaryWeight * Asset.SampleSize;
            float Mat2ManufacturingCost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxiliaryMaterial))
            {
                Material Mat2MFC = GetManufacturingCost(Asset.AuxiliaryMaterial);
                float Mat2CarbonFactor = ManufacturingCostFromEnum(Mat2MFC, Asset.AuxiliaryMaterialManufacturing);
                Mat2ManufacturingCost = Mat2CarbonFactor * 0.001f * Asset.AuxiliaryWeight * Asset.SampleSize;
            }

            float ManufacturingCost = Mat1ManufacturingCost + Mat2ManufacturingCost;

            /// DISPOSAL COSTS
            Disposal Mat1DSC = GetDisposalCost(Asset.PrimaryMaterial);
            float Mat1DisposalFactor = DisposalCostFromEnum(Mat1DSC, Asset.PrimaryDisposalMethod);
            float Mat1DisposalCost = Mat1DisposalFactor * 0.001f * Asset.PrimaryWeight * Asset.SampleSize;
            float Mat2DisposalCost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxiliaryMaterial))
            {
                Disposal Mat2DSC = GetDisposalCost(Asset.AuxiliaryMaterial);
                float Mat2DisposalFactor = DisposalCostFromEnum(Mat2DSC, Asset.AuxiliaryDisposalMethod);
                Mat2DisposalCost = Mat2DisposalFactor * 0.001f * Asset.AuxiliaryWeight * Asset.SampleSize;
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
            Transport ExampleTransport = GetTransportCost("HGV");
            float TotalWeight = Asset.SampleSize * (Asset.PrimaryWeight + Asset.AuxiliaryWeight);
            float TransportationCost = Asset.AverageDistanceToReuse * TotalWeight * 0.001f * (ExampleTransport.CarbonCost + ExampleTransport.WttConvFactor);
            float Mat1TransportationCost = Asset.AverageDistanceToReuse * Asset.PrimaryWeight * Asset.SampleSize * 0.001f * (ExampleTransport.CarbonCost + ExampleTransport.WttConvFactor);
            float Mat2TransportationCost = Asset.AverageDistanceToReuse * Asset.AuxiliaryWeight * Asset.SampleSize * 0.001f * (ExampleTransport.CarbonCost + ExampleTransport.WttConvFactor);

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

        private static float ManufacturingCostFromEnum(Material cost, ManufactoringMethod method)
        {
            switch (method)
            {
                case ManufactoringMethod.Primary:
                    return cost.MaterialProduction;

                case ManufactoringMethod.Reused:
                    if (cost.Reused != -1f)
                    {
                        return cost.Reused;
                    }
                    else throw new ArgumentException(cost.ManufacturingMaterial + " cannot be Reused.");
                case ManufactoringMethod.ClosedLoop:
                    if (cost.ClosedLoopSource != -1f)
                    {
                        return cost.ClosedLoopSource;
                    }
                    else throw new ArgumentException(cost.ManufacturingMaterial + " cannot be acquired from a Closed Loop.");
                case ManufactoringMethod.OpenLoop:
                    if (cost.OpenLoopSource != -1f)
                    {
                        return cost.OpenLoopSource;
                    }
                    else throw new ArgumentException(cost.ManufacturingMaterial + " cannot be acquired from an Open Loop.");
                default:
                    throw new ArgumentException(cost.ManufacturingMaterial + ": Invalid manufacturing method.");
            }
        }

        private static float DisposalCostFromEnum(Disposal cost, EntireDisposalMethod method)
        {
            switch (method)
            {
                case EntireDisposalMethod.Landfill:
                    return cost.Landfill;

                case EntireDisposalMethod.Reuse:
                    if (cost.Reuse != -1f)
                    {
                        return cost.Reuse;
                    }
                    else throw new ArgumentException(cost.MaterialOption + " cannot be disposed to reuse.");
                case EntireDisposalMethod.ClosedLoop:
                    if (cost.ClosedLoop != -1f)
                    {
                        return cost.ClosedLoop;
                    }
                    else throw new ArgumentException(cost.MaterialOption + " cannot be disposed into a Closed Loop.");
                case EntireDisposalMethod.OpenLoop:
                    if (cost.OpenLoop != -1f)
                    {
                        return cost.OpenLoop;
                    }
                    else throw new ArgumentException(cost.MaterialOption + " cannot be disposed into an Open Loop.");
                case EntireDisposalMethod.Combustion:
                    if (cost.Combustion != -1f)
                    {
                        return cost.Combustion;
                    }
                    else throw new ArgumentException(cost.MaterialOption + " cannot be combusted.");
                case EntireDisposalMethod.Composting:
                    if (cost.Composting != -1f)
                    {
                        return cost.Composting;
                    }
                    else throw new ArgumentException(cost.MaterialOption + " cannot be composed.");
                case EntireDisposalMethod.Anaerobic:
                    if (cost.AnaerobicDigestion != -1f)
                    {
                        return cost.AnaerobicDigestion;
                    }
                    else throw new ArgumentException(cost.MaterialOption + " cannot be digested anaerobically.");        
                // Carlos did this - please remove? or change? 
                case EntireDisposalMethod.None:
                        return 0;
                    
                default:
                    throw new ArgumentException(cost.MaterialOption + ": Invalid disposal method.");
            }
        }

        public static Material GetManufacturingCost(string MaterialName)
        {

            foreach (Material mat in manufacturingCosts)
            {
                if (mat.ManufacturingMaterial.Equals(MaterialName) == true)
                {
                    return mat;
                }
            }

            return null;
        }

        public static Disposal GetDisposalCost(string DisposalName)
        {

            foreach (Disposal dispo in disposalCosts)
            {
                if (dispo.MaterialOption.Equals(DisposalName) == true)
                {
                    return dispo;
                }
            }

            return null;
        }

        public static Transport GetTransportCost(string TransportName)
        {
            foreach (Transport trans in transportCosts)
            {
                if (trans.VehicleName.Equals(TransportName) == true)
                {
                    return trans;
                }
            }

            return null;
        }

        private static float EmptyToInv(string field)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return -1;
            }
            else return float.Parse(field);
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
    /*
    public struct CarbonResults
    {
        public string AssetName;
        public float LinearCarbonTotal;
        public float PrimaryMaterialLinearCarbon;
        public float AuxiliaryMaterialLinearCarbon;
        public float CircularCarbonTotal;
        public float PrimaryMaterialCircularCarbon;
        public float AuxiliaryMaterialCircularCarbon;
        public float RawManufacturingCarbon;
        public float PrimaryMaterialManufacturingCarbon;
        public float AuxiliaryMaterialManufacturingCarbon;
        public float RawDisposalCarbon;
        public float PrimaryMaterialDisposalCarbon;
        public float AuxiliaryMaterialDisposalCarbon;
        public float RawTransportCarbon;
        public float ReuseAsPercent;

        public CarbonResults(string Asset, float Linear, float Mat1Linear, float Mat2Linear, float Circular, float Mat1Circular, float Mat2Circular,
            float Manuf, float Mat1M, float Mat2M, float Disp, float Mat1D, float Mat2D, float Trans)
        {
            AssetName = Asset;
            LinearCarbonTotal = Linear;
            PrimaryMaterialLinearCarbon = Mat1Linear;
            AuxiliaryMaterialLinearCarbon = Mat2Linear;
            CircularCarbonTotal = Circular;
            PrimaryMaterialCircularCarbon = Mat1Circular;
            AuxiliaryMaterialCircularCarbon = Mat2Circular;
            ReuseAsPercent = (1 - (Circular / Linear)) * 100;
            RawManufacturingCarbon = Manuf;
            PrimaryMaterialManufacturingCarbon = Mat1M;
            AuxiliaryMaterialManufacturingCarbon = Mat2M;
            RawDisposalCarbon = Disp;
            PrimaryMaterialDisposalCarbon = Mat1D;
            AuxiliaryMaterialDisposalCarbon = Mat2D;
            RawTransportCarbon = Trans;
        }
    }*/

    
}