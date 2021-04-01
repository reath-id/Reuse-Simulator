using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReathUIv0._3.Models
{
    /// <summary>
    /// Class representing all the data associated with a single batch of reusable assets. 
    /// </summary>
    public class ReusableAsset
    {
        // Misc Data
        public string DateRange = string.Empty;
        public string AssetCountryOfOrigin = string.Empty;

        // Asset Data
        public string AssetName = string.Empty;
        public int SampleSize = 0;
        public float UnitWeight = 0.0f; // TODO: Remove, adjust GUI
        public float UnitCost = 0.0f;


        // Primary Data
        public string PrimaryMaterial = string.Empty;
        public float PrimaryWeight = 0.0f;
        public string PrimaryManufacturingMethod = string.Empty;
        public string PrimaryDispoMethod = string.Empty;
        public string PrimaryCleaningMethod = string.Empty;
        public ManufactoringMethod PrimaryManufacturingMethod_;
        public DisposalMethod PrimaryDisposalMethod;


        // Auxiliary Data
        public string AuxiliaryMaterial = string.Empty;
        public float AuxiliaryWeight = 0.0f;
        public string AuxiliaryManufacturingMethod = string.Empty;
        public string AuxiliaryDispoMethod = string.Empty;
        public string AuxiliaryCleaningMethod = string.Empty;
        public ManufactoringMethod AuxiliaryManufacturingMethod_;
        public DisposalMethod AuxiliaryDisposalMethod;


        // Recycle Data
        public bool IsRecycled = false;
        public int RecycledPercentage = 0;
        public string RecycledCountryOfOrigin = string.Empty;
        public string ReuseOccurence = string.Empty;
        public int AverageDistanceToReuse = 0;
        public float MaximumReuses = 0.0f;
        public float PercentageOfManufacturingCarbon = 0.0f;


        public ReusableAsset() { }

        public ReusableAsset(int sampleSize, string dateRange, string assetName, float unitCost, float unitWeight, string assetCountryOfOrigin, float primaryWeight, float auxiliaryWeight, string reuseOccurence, int averageDistanceToReuse, float maximumReuses, float percentageOfManufacturingCarbon, string primaryMaterialEmission, string auxiliaryMaterialEmission)
        {
            SampleSize = sampleSize;
            DateRange = dateRange;
            AssetName = assetName;
            UnitCost = unitCost;
            UnitWeight = unitWeight;
            AssetCountryOfOrigin = assetCountryOfOrigin;
            PrimaryMaterial = string.Empty;
            PrimaryManufacturingMethod_ = 0;
            PrimaryWeight = primaryWeight;
            PrimaryManufacturingMethod = primaryMaterialEmission;
            PrimaryDisposalMethod = 0;
            AuxiliaryMaterial = string.Empty;
            AuxiliaryManufacturingMethod_ = 0;
            AuxiliaryWeight = auxiliaryWeight;
            AuxiliaryManufacturingMethod = auxiliaryMaterialEmission;
            IsRecycled = false;
            RecycledPercentage = 0;
            RecycledCountryOfOrigin = string.Empty;
            ReuseOccurence = reuseOccurence;
            AuxiliaryDisposalMethod = 0;
            AverageDistanceToReuse = averageDistanceToReuse;
            MaximumReuses = maximumReuses;
            PercentageOfManufacturingCarbon = percentageOfManufacturingCarbon;
        }

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

        public enum ManufactoringMethod
        {
            Primary, Reused, OpenLoop, ClosedLoop
        }

        public enum DisposalMethod
        {
            Reuse, OpenLoop, ClosedLoop, Combustion, Composting, Landfill, Anaerobic
        }
    }
}
