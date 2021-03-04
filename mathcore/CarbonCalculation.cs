using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathcore
{
    static class CarbonCalculation
    {
        static MockDB DB;

        public static void SetDB(MockDB DB_)
        {
            DB = DB_;
        }
        public static CarbonResults CalculateCarbon(ReusableAsset Asset)
        {

            /// MANUFACTURING COSTS

            ManufacturingCost Mat1MFC = DB.GetManufacturingCost(Asset.PrimaryMaterial);

            float mat1carbonfactor = ManufacturingCostFromEnum(Mat1MFC, Asset.PrimaryMaterialManufacturing);

            float mat1carboncost = mat1carbonfactor * 0.001f * Asset.PrimaryWeight * Asset.NoOfItems;

            float mat2carboncost = 0;
            
            if (!String.IsNullOrWhiteSpace(Asset.AuxillaryMaterial))
            {
                ManufacturingCost Mat2MFC = DB.GetManufacturingCost(Asset.AuxillaryMaterial);

                float mat2carbonfactor = ManufacturingCostFromEnum(Mat2MFC, Asset.AuxillaryMaterialManufacturing);

                mat2carboncost = mat2carbonfactor * 0.001f * Asset.AuxillaryWeight * Asset.NoOfItems;
            }

            float ManufacturingCost = mat1carboncost + mat2carboncost;


            /// DISPOSAL COSTS

            DisposalCost Mat1DSC = DB.GetDisposalCost(Asset.PrimaryMaterial);

            float mat1disposalfactor = DisposalCostFromEnum(Mat1DSC, Asset.PrimaryDisposalMethod);

            float mat1disposalcost = mat1disposalfactor * 0.001f * Asset.PrimaryWeight * Asset.NoOfItems;

            float mat2disposalcost = 0;

            if (!String.IsNullOrWhiteSpace(Asset.AuxillaryMaterial))
            {
                DisposalCost Mat2DSC = DB.GetDisposalCost(Asset.AuxillaryMaterial);

                float mat2disposalfactor = DisposalCostFromEnum(Mat2DSC, Asset.AuxillaryDisposalMethod);

                mat2disposalcost = mat2disposalfactor * 0.001f * Asset.AuxillaryWeight * Asset.NoOfItems;
            }

            float DisposalCost = mat1disposalcost + mat2disposalcost;


            /// LINEAR COST

            float linearcost = ManufacturingCost + DisposalCost;


            /// REUSE MODEL

            float reusemanufacturingcarbon = ManufacturingCost / Asset.MaximumReuses;


            /// BACKHAUL COST

            TransportCost exampletransport = DB.GetTransportCost("HGV");

            float totalweight = Asset.NoOfItems * (Asset.PrimaryWeight + Asset.AuxillaryWeight);
        
            float transportcarbon = Asset.AvgDistanceToRecycle * totalweight * 0.001f * (exampletransport.Cost + exampletransport.WTTFactor);


            /// PREPARATION FOR REUSE COST

            float prepreusecarbon = Asset.PrepForReuseCarbonFactor * ManufacturingCost;


            /// REUSE DISPOSAL

            float reusedisposalcarbon = DisposalCost / Asset.MaximumReuses;


            /// CIRCULAR COST

            float circularcost = reusemanufacturingcarbon + transportcarbon + prepreusecarbon + reusedisposalcarbon;


            return new CarbonResults(Asset.AssetName, linearcost, circularcost, ManufacturingCost, DisposalCost, transportcarbon, mat1carboncost, mat2carboncost, mat1disposalcost, mat2disposalcost);


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
        public float CircularCarbonTotal;
        public float RawManufacturingCarbon;
        public float Mat1ManufacturingCarbon;
        public float Mat2ManufacturingCarbon;
        public float RawDisposalCarbon;
        public float Mat1DisposalCarbon;
        public float Mat2DisposalCarbon;
        public float RawTransportCarbon;
        public float ReuseAsPercent;

        public CarbonResults(string Asset, float Linear, float Circular, float Manuf, float Disp, float Trans, float Mat1M, float Mat2M, float Mat1D, float Mat2D)
        {
            AssetName = Asset;
            LinearCarbonTotal = Linear;
            CircularCarbonTotal = Circular;
            ReuseAsPercent = (1 - (Circular / Linear)) * 100;
            RawManufacturingCarbon = Manuf;
            RawDisposalCarbon = Disp;
            RawTransportCarbon = Trans;
            Mat1ManufacturingCarbon = Mat1M;
            Mat2ManufacturingCarbon = Mat2M;
            Mat1DisposalCarbon = Mat1D;
            Mat2DisposalCarbon = Mat2D;
        }
    }
}
