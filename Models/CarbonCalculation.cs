using ReathUIv0._3.Models;
using System;
using static ReathUIv0._3.Models.ReusableAsset;

namespace ReathUIv0._3
{
    internal static class CarbonCalculation
    {
        private static MockDB DB;

        public static void SetDB(MockDB DB_)
        {
            DB = DB_;
        }

        public static CarbonResults CalculateCarbon(ReusableAsset Asset)
        {
            /// MANUFACTURING COSTS
            Material Mat1MFC = DB.GetManufacturingCost(Asset.PrimaryMaterial);
            float primaryMaterialcarbonfactor = ManufacturingCostFromEnum(Mat1MFC, Asset.PrimaryMaterialManufacturing);

            float primaryMaterialcarboncost = primaryMaterialcarbonfactor * 0.001f * Asset.PrimaryWeight * Asset.SampleSize;
            float AuxiliaryMaterialcarboncost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxiliaryMaterial))
            {
                Material Mat2MFC = DB.GetManufacturingCost(Asset.AuxiliaryMaterial);
                float mat2carbonfactor = ManufacturingCostFromEnum(Mat2MFC, Asset.AuxiliaryMaterialManufacturing);
                AuxiliaryMaterialcarboncost = mat2carbonfactor * 0.001f * Asset.AuxiliaryWeight * Asset.SampleSize;
            }

            float ManufacturingCost = primaryMaterialcarboncost + AuxiliaryMaterialcarboncost;

            /// DISPOSAL COSTS
            Disposal Mat1DSC = DB.GetDisposalCost(Asset.PrimaryMaterial);
            float primaryMaterialdisposalfactor = DisposalCostFromEnum(Mat1DSC, Asset.PrimaryDisposalMethod);
            float primaryMaterialdisposalcost = primaryMaterialdisposalfactor * 0.001f * Asset.PrimaryWeight * Asset.SampleSize;
            float AuxiliaryMaterialdisposalcost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxiliaryMaterial))
            {
                Disposal Mat2DSC = DB.GetDisposalCost(Asset.AuxiliaryMaterial);
                float AuxiliaryMaterialdisposalfactor = DisposalCostFromEnum(Mat2DSC, Asset.AuxiliaryDisposalMethod);
                AuxiliaryMaterialdisposalcost = AuxiliaryMaterialdisposalfactor * 0.001f * Asset.AuxiliaryWeight * Asset.SampleSize;
            }
            float DisposalCost = primaryMaterialdisposalcost + AuxiliaryMaterialdisposalcost;

            /// LINEAR COST
            float linearcost = ManufacturingCost + DisposalCost;
            float primaryMaterialLinearCost = primaryMaterialcarboncost + primaryMaterialdisposalcost;
            float AuxiliaryMaterialLinearCost = AuxiliaryMaterialcarboncost + AuxiliaryMaterialdisposalcost;

            /// REUSE MODEL
            float reusemanufacturingcarbon = ManufacturingCost / Asset.MaximumReuses;
            float primaryMaterialReusemanuFacturingCarbon = primaryMaterialcarboncost / Asset.MaximumReuses;
            float AuxiliaryMaterialReusemanuFacturingCarbon = AuxiliaryMaterialcarboncost / Asset.MaximumReuses;

            /// BACKHAUL COST
            Transport exampletransport = DB.GetTransportCost("HGV");
            float totalweight = Asset.SampleSize * (Asset.PrimaryWeight + Asset.AuxiliaryWeight);
            float transportcarbon = Asset.AverageDistanceToReuse * totalweight * 0.001f * (exampletransport.CarbonCost + exampletransport.WttConvFactor);
            float primaryMaterialTransportCarbon = Asset.AverageDistanceToReuse * Asset.PrimaryWeight * 0.001f * (exampletransport.CarbonCost + exampletransport.WttConvFactor);
            float AuxiliaryMaterialTransportCarbon = Asset.AverageDistanceToReuse * Asset.AuxiliaryWeight * 0.001f * (exampletransport.CarbonCost + exampletransport.WttConvFactor);

            /// PREPARATION FOR REUSE COST
            float prepreusecarbon = Asset.PercentageOfManufacturingCarbon * ManufacturingCost;
            float primaryMaterialPrePreuseCarbon = Asset.PercentageOfManufacturingCarbon * primaryMaterialcarboncost;
            float AuxiliaryMaterialPrePreuseCarbon = Asset.PercentageOfManufacturingCarbon * AuxiliaryMaterialcarboncost;

            /// REUSE DISPOSAL
            float reusedisposalcarbon = DisposalCost / Asset.MaximumReuses;
            float primaryMaterialreusedisposalcarbon = primaryMaterialdisposalcost / Asset.MaximumReuses;
            float AuxiliaryMaterialreusedisposalcarbon = AuxiliaryMaterialdisposalcost / Asset.MaximumReuses;

            /// CIRCULAR COST
            float circularcost = reusemanufacturingcarbon + transportcarbon + prepreusecarbon + reusedisposalcarbon;
            float primaryMaterialCircularCostt = primaryMaterialReusemanuFacturingCarbon + primaryMaterialTransportCarbon + primaryMaterialPrePreuseCarbon + primaryMaterialreusedisposalcarbon;
            float AuxiliaryMaterialCircularCostt = AuxiliaryMaterialReusemanuFacturingCarbon + AuxiliaryMaterialTransportCarbon + AuxiliaryMaterialPrePreuseCarbon + AuxiliaryMaterialreusedisposalcarbon;

            return new CarbonResults(Asset.AssetName, linearcost, primaryMaterialLinearCost, AuxiliaryMaterialLinearCost,
                                    circularcost, primaryMaterialCircularCostt, AuxiliaryMaterialCircularCostt,
                                    ManufacturingCost, primaryMaterialcarboncost, AuxiliaryMaterialcarboncost,
                                    DisposalCost, primaryMaterialdisposalcost, AuxiliaryMaterialdisposalcost,
                                    transportcarbon);
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
                default:
                    throw new ArgumentException(cost.MaterialOption + ": Invalid disposal method.");
            }
        }
    }

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
    }
}