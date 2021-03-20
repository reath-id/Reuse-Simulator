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
            float Mat1CarbonFactor = ManufacturingCostFromEnum(Mat1MFC, Asset.PrimaryMaterialManufacturing);

            float Mat1ManufacturingCost = Mat1CarbonFactor * 0.001f * Asset.PrimaryWeight * Asset.NoOfItems;
            float Mat2ManufacturingCost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxiliaryMaterial))
            {
                ManufacturingCost Mat2MFC = DB.GetManufacturingCost(Asset.AuxiliaryMaterial);
                float Mat2CarbonFactor = ManufacturingCostFromEnum(Mat2MFC, Asset.AuxiliaryMaterialManufacturing);
                Mat2ManufacturingCost = Mat2CarbonFactor * 0.001f * Asset.AuxiliaryWeight * Asset.NoOfItems;
            }

            float ManufacturingCost = Mat1ManufacturingCost + Mat2ManufacturingCost;

            /// DISPOSAL COSTS
            DisposalCost Mat1DSC = DB.GetDisposalCost(Asset.PrimaryMaterial);
            float Mat1DisposalFactor = DisposalCostFromEnum(Mat1DSC, Asset.PrimaryDisposalMethod);
            float Mat1DisposalCost = Mat1DisposalFactor * 0.001f * Asset.PrimaryWeight * Asset.NoOfItems;
            float Mat2DisposalCost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxiliaryMaterial))
            {
                DisposalCost Mat2DSC = DB.GetDisposalCost(Asset.AuxiliaryMaterial);
                float Mat2DisposalFactor = DisposalCostFromEnum(Mat2DSC, Asset.AuxiliaryDisposalMethod);
                Mat2DisposalCost = Mat2DisposalFactor * 0.001f * Asset.AuxiliaryWeight * Asset.NoOfItems;
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
            TransportCost ExampleTransport = DB.GetTransportCost("HGV");
            float TotalWeight = Asset.NoOfItems * (Asset.PrimaryWeight + Asset.AuxiliaryWeight);
            float TransportationCost = Asset.AvgDistanceToRecycle * TotalWeight * 0.001f * (ExampleTransport.Cost + ExampleTransport.WTTFactor);
            float Mat1TransportationCost = Asset.AvgDistanceToRecycle * Asset.PrimaryWeight * Asset.NoOfItems * 0.001f * (ExampleTransport.Cost + ExampleTransport.WTTFactor);
            float Mat2TransportationCost = Asset.AvgDistanceToRecycle * Asset.AuxiliaryWeight * Asset.NoOfItems * 0.001f * (ExampleTransport.Cost + ExampleTransport.WTTFactor);

            /// PREPARATION FOR REUSE COST
            float PrepReuseCost = Asset.PrepForReuseCarbonFactor * ManufacturingCost;
            float Mat1PrepReuseCost = Asset.PrepForReuseCarbonFactor * Mat1ManufacturingCost;
            float Mat2PrepReuseCost = Asset.PrepForReuseCarbonFactor * Mat2ManufacturingCost;

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

           // return new CarbonResults(Asset.AssetName, linearcost, primaryMaterialLinearCost, AuxiliaryMaterialLinearCost,
           //                         circularcost, primaryMaterialCircularCostt, AuxiliaryMaterialCircularCostt,
           //                         ManufacturingCost, primaryMaterialcarboncost, AuxiliaryMaterialcarboncost,
           //                         DisposalCost, primaryMaterialdisposalcost, AuxiliaryMaterialdisposalcost,
            //                        transportcarbon);
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
        public CarbonMaterialResults Auxillary;
        public CarbonMaterialResults Total;

        public CarbonResults(CarbonMaterialResults Mat1, CarbonMaterialResults Mat2, CarbonMaterialResults Total_)
        {
            Primary = Mat1;
            Auxillary = Mat2;
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
    }
    */
}