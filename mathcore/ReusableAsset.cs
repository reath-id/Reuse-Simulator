using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathcore
{
    public class ReusableAsset
    {
        // INFO
        public string AssetName;
        public string AssetCountryOfOrigin;

        // CARBON COST FACTORS

        public int NoOfItems;

        public string PrimaryMaterial;
        public ManufactoringMethod PrimaryMaterialManufacturing;
        public float PrimaryWeight;
        public DisposalMethod PrimaryDisposalMethod;

        public string AuxillaryMaterial;
        public ManufactoringMethod AuxillaryMaterialManufacturing;
        public float AuxillaryWeight;
        public DisposalMethod AuxillaryDisposalMethod;
       
        public float AvgDistanceToRecycle;
        public float PrepForReuseCarbonFactor;
        public int MaximumReuses;


        // UNKNOWN

        public bool IsRecycled;
        public string ReuseOccurence;
        public float RecycledPercentage;
        public string RecycledCountryOfOrigin;   
        public string CleaningMethod;
        public int SampleSize;
        public int DataRange;
        public float UnitCost;

        public static ManufactoringMethod StringToManufacturingMethod(string s)
        {
            if (s == "Primary")
            {
                return ManufactoringMethod.Primary;
            }
            else if (s == "Reused")
            {
                return ManufactoringMethod.Reused;
            }
            else if (s == "Open Loop")
            {
                return ManufactoringMethod.OpenLoop;
            }
            else if (s == "Closed Loop")
            {
                return ManufactoringMethod.ClosedLoop;
            }
            else throw new ArgumentException("Manufacturing Method " + s + " doesn't exist.");
        }

        public static DisposalMethod StringToDisposalMethod(string s)
        {
            if (s == "Landfill")
            {
                return DisposalMethod.Landfill;
            }
            else if (s == "Reuse")
            {
                return DisposalMethod.Reuse;
            }
            else if (s == "Open Loop")
            {
                return DisposalMethod.OpenLoop;
            }
            else if (s == "Closed Loop")
            {
                return DisposalMethod.ClosedLoop;
            }
            else if (s == "Combustion")
            {
                return DisposalMethod.Combustion;
            }
            else if (s == "Composting")
            {
                return DisposalMethod.Composting;
            }
            else if (s == "Anaerobic")
            {
                return DisposalMethod.Anaerobic;
            }
            else throw new ArgumentException("Disposal Method " + s + " doesn't exist.");
        }
    }
    public enum ManufactoringMethod
    {
        Primary, Reused, OpenLoop, ClosedLoop
    }

    public enum DisposalMethod
    {
        Reuse, OpenLoop, ClosedLoop, Combustion, Composting, Landfill, Anaerobic
    }

    
}
