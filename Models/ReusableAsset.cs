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
        //public string PrimaryManufString = string.Empty;
        //public string PrimaryDispoString = string.Empty;
        //public string PrimaryCleaningMethod = string.Empty;
        public ManufactoringMethod PrimaryManufacturingMethod;
        public DisposalMethod PrimaryDisposalMethod;


        // Auxiliary Data
        public string AuxiliaryMaterial = string.Empty;
        public float AuxiliaryWeight = 0.0f;
        //public string AuxiliaryManufString = string.Empty;
        //public string AuxiliaryDispoString = string.Empty;
        //public string AuxiliaryCleaningMethod = string.Empty;
        public ManufactoringMethod AuxiliaryManufacturingMethod;
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
            PrimaryManufacturingMethod = 0;
            PrimaryWeight = primaryWeight;
           // PrimaryManufString = primaryMaterialEmission;
            PrimaryDisposalMethod = 0;
            AuxiliaryMaterial = string.Empty;
            AuxiliaryManufacturingMethod = 0;
            AuxiliaryWeight = auxiliaryWeight;
            //  AuxiliaryManufString = auxiliaryMaterialEmission;
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

        public static string ManufacturingMethodToString(ManufactoringMethod m)
        {
            switch (m)
            {
                case ManufactoringMethod.Primary:
                    return "Primary";
                case ManufactoringMethod.Reused:
                    return "Reused";
                case ManufactoringMethod.ClosedLoop:
                    return "Closed Loop";
                case ManufactoringMethod.OpenLoop:
                    return "Open Loop";
                default:
                    throw new ArgumentException(m.ToString() + ": Invalid manufacturing method.");
            }
        }

        public static string DisposalMethodToString(DisposalMethod m)
        {
            switch (m)
            {
                case DisposalMethod.Landfill:
                    return "Landfill";
                case DisposalMethod.Reuse:
                    return "Reuse";
                case DisposalMethod.ClosedLoop:
                    return "Closed Loop";
                case DisposalMethod.OpenLoop:
                    return "Open Loop";
                case DisposalMethod.Combustion:
                    return "Combustion";
                case DisposalMethod.Composting:
                    return "Composting";
                case DisposalMethod.Anaerobic:
                    return "Anaerobic";
                default:
                    throw new ArgumentException(m.ToString() + ": Invalid disposal method.");
            }
        }

        public enum ManufactoringMethod
        {
            Primary, Reused, OpenLoop, ClosedLoop, None
        }

        public enum DisposalMethod
        {
            Reuse, OpenLoop, ClosedLoop, Combustion, Composting, Landfill, Anaerobic, None
        }
    }
}
