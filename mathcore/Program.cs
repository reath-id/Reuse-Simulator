using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace mathcore
{
    class Program
    {
        static void Main(string[] args)
        {
            CarbonCalculation.SetDB(new MockDB("."));

            List<string> exampleassets = File.ReadAllLines("exampleassets.csv").Skip(1).ToList();

            foreach (string asset in exampleassets)
            {
                ReusableAsset reusableAsset = new ReusableAsset();

                string[] asset_values = asset.Split(',');

                reusableAsset.AssetName = asset_values[0];
                reusableAsset.NoOfItems = int.Parse(asset_values[1]);
                reusableAsset.PrimaryMaterial = asset_values[2];
                reusableAsset.PrimaryWeight = float.Parse(asset_values[3]);
                reusableAsset.PrimaryMaterialManufacturing = ReusableAsset.StringToManufacturingMethod(asset_values[4]);
                reusableAsset.PrimaryDisposalMethod = ReusableAsset.StringToDisposalMethod(asset_values[8]);

                if (asset_values[5] != "None")
                {
                    reusableAsset.AuxillaryMaterial = asset_values[5];
                    reusableAsset.AuxillaryWeight = float.Parse(asset_values[6]);
                    reusableAsset.AuxillaryMaterialManufacturing = ReusableAsset.StringToManufacturingMethod(asset_values[7]);
                    reusableAsset.AuxillaryDisposalMethod = ReusableAsset.StringToDisposalMethod(asset_values[9]);
                }

                reusableAsset.MaximumReuses = int.Parse(asset_values[10]);
                reusableAsset.AvgDistanceToRecycle = float.Parse(asset_values[11]);
                reusableAsset.PrepForReuseCarbonFactor = float.Parse(asset_values[12]);

                CarbonResults res = CarbonCalculation.CalculateCarbon(reusableAsset);

                Console.WriteLine("Asset: " + res.AssetName);
                Console.WriteLine("Linear Carbon: " + res.LinearCarbon);
                Console.WriteLine("Circular Carbon: " + res.CircularCarbon);
                Console.WriteLine("Savings: " + res.ReuseAsPercent);
                Console.WriteLine("---------------------------------------");
            }

            Console.ReadLine();
        }
    }
}
