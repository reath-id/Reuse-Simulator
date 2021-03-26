using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReathUIv0._3.Models
{
    public class ReusableAsset
    {
        public int SampleSize { get; set; }
        public string DateRange { get; set; }
        public string AssetName { get; set; }
        public float UnitCost { get; set; }
        public float UnitWeight { get; set; }
        public string AssetCountryOfOrigin { get; set;}
        public string PrimaryMaterial { get; set; }
        public float PrimaryWeight { get; set; }
        public string PrimaryMaterialEmission { get; set; }
        public float PrimaryMaterialCost { get; set; }
        public string PrimaryDispoMethod { get; set; }
        public string PrimaryCleaningMethod { get; set; }
        public string AuxiliaryMaterial { get; set; }
        public float AuxiliaryWeight { get; set; }
        public string AuxiliaryMaterialEmission { get; set; }
        public float AuxiliaryMaterialCost { get; set; }
        public string AuxiliaryDispoMethod { get; set; }
        public string AuxiliaryCleaningMethod { get; set; }
        public int IsRecylced { get; set; }
        public int RecycledPercentage { get; set; }
        public string RecycledCountryOfOrigin { get; set; }
        public string ReuseOccurence { get; set; }
        public int AverageDistanceToReuse { get; set; }
        public float MaximumReuses { get; set; }
        public float PercentageOfManufacturingCarbon { get; set; }
        public ManufactoringMethod PrimaryMaterialManufacturing { get; set; }
        public ManufactoringMethod AuxiliaryMaterialManufacturing { get; set; }
        public EntireDisposalMethod PrimaryDisposalMethod { get; set; }
        public EntireDisposalMethod AuxiliaryDisposalMethod { get; set; }


        public ReusableAsset()
        {
            SampleSize = 0;
            DateRange = string.Empty;
            AssetName = string.Empty;
            UnitCost = 0F;
            UnitWeight = 0F;
            AssetCountryOfOrigin = string.Empty;
            PrimaryMaterial = string.Empty;
            PrimaryWeight = 0F;
            PrimaryMaterialEmission = string.Empty;
            PrimaryMaterialCost = 0F;
            PrimaryDispoMethod = string.Empty;
            PrimaryCleaningMethod = string.Empty;
            AuxiliaryMaterial = string.Empty;
            AuxiliaryWeight = 0F;
            AuxiliaryMaterialEmission = string.Empty;
            AuxiliaryMaterialCost = 0F;
            AuxiliaryDispoMethod = string.Empty;
            AuxiliaryCleaningMethod = string.Empty;
            IsRecylced = 0;
            RecycledPercentage = 0;
            RecycledCountryOfOrigin = string.Empty;
            ReuseOccurence = string.Empty;
            AverageDistanceToReuse = 0;
            MaximumReuses = 0F;
            PercentageOfManufacturingCarbon = 0F;
        }

        public ReusableAsset(int sampleSize,string dateRange,string assetName ,float unitCost, float unitWeight,string assetCountryOfOrigin, float primaryWeight,float auxiliaryWeight,string reuseOccurence,int averageDistanceToReuse,float maximumReuses,float percentageOfManufacturingCarbon,string primaryMaterialEmission,float primaryMaterialCost,string auxiliaryMaterialEmission,float auxiliaryMaterialCost)
        {
            SampleSize = sampleSize;
            DateRange = dateRange;
            AssetName = assetName;
            UnitCost = unitCost;
            UnitWeight = unitWeight;
            AssetCountryOfOrigin = assetCountryOfOrigin;
            PrimaryMaterial = string.Empty;
            PrimaryMaterialManufacturing = 0;
            PrimaryWeight = primaryWeight;
            PrimaryMaterialEmission = primaryMaterialEmission;
            PrimaryMaterialCost = primaryMaterialCost;
            PrimaryDisposalMethod = 0;
            AuxiliaryMaterial = string.Empty;
            AuxiliaryMaterialManufacturing = 0;
            AuxiliaryWeight = auxiliaryWeight;
            AuxiliaryMaterialEmission = auxiliaryMaterialEmission;
            AuxiliaryMaterialCost = auxiliaryMaterialCost;
            IsRecylced = 0; ;
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

        public static EntireDisposalMethod StringToDisposalMethod(string s)
        {
            if (s == "Landfill")
            {
                return EntireDisposalMethod.Landfill;
            }
            else if (s == "Reuse")
            {
                return EntireDisposalMethod.Reuse;
            }
            else if (s == "Open Loop")
            {
                return EntireDisposalMethod.OpenLoop;
            }
            else if (s == "Closed Loop")
            {
                return EntireDisposalMethod.ClosedLoop;
            }
            else if (s == "Combustion")
            {
                return EntireDisposalMethod.Combustion;
            }
            else if (s == "Composting")
            {
                return EntireDisposalMethod.Composting;
            }
            else if (s == "Anaerobic")
            {
                return EntireDisposalMethod.Anaerobic;
            }
            else throw new ArgumentException("Disposal Method " + s + " doesn't exist.");
        }
    

        public enum ManufactoringMethod
        {
            Primary, Reused, OpenLoop, ClosedLoop
        }
        
        public enum EntireDisposalMethod
        {
            Reuse, OpenLoop, ClosedLoop, Combustion, Composting, Landfill, Anaerobic
        }
    }
}
