using System;

namespace ReathUIv0._1
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
            ManufacturingCost Mat1MFC = DB.GetManufacturingCost(Asset.PrimaryMaterial);
            float primaryMaterialcarbonfactor = ManufacturingCostFromEnum(Mat1MFC, Asset.PrimaryMaterialManufacturing);

            float primaryMaterialcarboncost = primaryMaterialcarbonfactor * 0.001f * Asset.PrimaryWeight * Asset.NoOfItems;
            float AuxiliaryMaterialcarboncost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxiliaryMaterial))
            {
                ManufacturingCost Mat2MFC = DB.GetManufacturingCost(Asset.AuxiliaryMaterial);
                float mat2carbonfactor = ManufacturingCostFromEnum(Mat2MFC, Asset.AuxiliaryMaterialManufacturing);
                AuxiliaryMaterialcarboncost = mat2carbonfactor * 0.001f * Asset.AuxiliaryWeight * Asset.NoOfItems;
            }

            float ManufacturingCost = primaryMaterialcarboncost + AuxiliaryMaterialcarboncost;

            /// DISPOSAL COSTS
            DisposalCost Mat1DSC = DB.GetDisposalCost(Asset.PrimaryMaterial);
            float primaryMaterialdisposalfactor = DisposalCostFromEnum(Mat1DSC, Asset.PrimaryDisposalMethod);
            float primaryMaterialdisposalcost = primaryMaterialdisposalfactor * 0.001f * Asset.PrimaryWeight * Asset.NoOfItems;
            float AuxiliaryMaterialdisposalcost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxiliaryMaterial))
            {
                DisposalCost Mat2DSC = DB.GetDisposalCost(Asset.AuxiliaryMaterial);
                float AuxiliaryMaterialdisposalfactor = DisposalCostFromEnum(Mat2DSC, Asset.AuxiliaryDisposalMethod);
                AuxiliaryMaterialdisposalcost = AuxiliaryMaterialdisposalfactor * 0.001f * Asset.AuxiliaryWeight * Asset.NoOfItems;
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
            TransportCost exampletransport = DB.GetTransportCost("HGV");
            float totalweight = Asset.NoOfItems * (Asset.PrimaryWeight + Asset.AuxiliaryWeight);
            float transportcarbon = Asset.AvgDistanceToRecycle * totalweight * 0.001f * (exampletransport.Cost + exampletransport.WTTFactor);
            float primaryMaterialTransportCarbon = Asset.AvgDistanceToRecycle * Asset.PrimaryWeight * 0.001f * (exampletransport.Cost + exampletransport.WTTFactor);
            float AuxiliaryMaterialTransportCarbon = Asset.AvgDistanceToRecycle * Asset.AuxiliaryWeight * 0.001f * (exampletransport.Cost + exampletransport.WTTFactor);

            /// PREPARATION FOR REUSE COST
            float prepreusecarbon = Asset.PrepForReuseCarbonFactor * ManufacturingCost;
            float primaryMaterialPrePreuseCarbon = Asset.PrepForReuseCarbonFactor * primaryMaterialcarboncost;
            float AuxiliaryMaterialPrePreuseCarbon = Asset.PrepForReuseCarbonFactor * AuxiliaryMaterialcarboncost;

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

        private static float ManufacturingCostFromEnum(ManufacturingCost cost, ManufactoringMethod method)
        {
            switch (method)
            {
                case ManufactoringMethod.Primary:
                    return cost.PrimaryProduction;

                case ManufactoringMethod.Reused:
                    if (cost.Reused != -1f)
                    {
                        return cost.Reused;
                    }
                    else throw new ArgumentException(cost.MaterialName + " cannot be Reused.");
                case ManufactoringMethod.ClosedLoop:
                    if (cost.ClosedLoop != -1f)
                    {
                        return cost.ClosedLoop;
                    }
                    else throw new ArgumentException(cost.MaterialName + " cannot be acquired from a Closed Loop.");
                case ManufactoringMethod.OpenLoop:
                    if (cost.OpenLoop != -1f)
                    {
                        return cost.OpenLoop;
                    }
                    else throw new ArgumentException(cost.MaterialName + " cannot be acquired from an Open Loop.");
                default:
                    throw new ArgumentException(cost.MaterialName + ": Invalid manufacturing method.");
            }
        }

        private static float DisposalCostFromEnum(DisposalCost cost, DisposalMethod method)
        {
            switch (method)
            {
                case DisposalMethod.Landfill:
                    return cost.Landfill;

                case DisposalMethod.Reuse:
                    if (cost.Reuse != -1f)
                    {
                        return cost.Reuse;
                    }
                    else throw new ArgumentException(cost.MaterialName + " cannot be disposed to reuse.");
                case DisposalMethod.ClosedLoop:
                    if (cost.ClosedLoop != -1f)
                    {
                        return cost.ClosedLoop;
                    }
                    else throw new ArgumentException(cost.MaterialName + " cannot be disposed into a Closed Loop.");
                case DisposalMethod.OpenLoop:
                    if (cost.OpenLoop != -1f)
                    {
                        return cost.OpenLoop;
                    }
                    else throw new ArgumentException(cost.MaterialName + " cannot be disposed into an Open Loop.");
                case DisposalMethod.Combustion:
                    if (cost.Combustion != -1f)
                    {
                        return cost.Combustion;
                    }
                    else throw new ArgumentException(cost.MaterialName + " cannot be combusted.");
                case DisposalMethod.Composting:
                    if (cost.Composting != -1f)
                    {
                        return cost.Composting;
                    }
                    else throw new ArgumentException(cost.MaterialName + " cannot be composed.");
                case DisposalMethod.Anaerobic:
                    if (cost.AnaerobicDigestion != -1f)
                    {
                        return cost.AnaerobicDigestion;
                    }
                    else throw new ArgumentException(cost.MaterialName + " cannot be digested anaerobically.");
                default:
                    throw new ArgumentException(cost.MaterialName + ": Invalid disposal method.");
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