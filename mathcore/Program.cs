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
            List<string> exampleresults = File.ReadAllLines("exampleres.csv").ToList();
            int i = 0;

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

                string[] test_values = exampleresults[i++].Split(',');

                Console.WriteLine("Asset: " + res.AssetName);
                Console.WriteLine("Linear Carbon: " + res.LinearCarbonTotal);
                Console.WriteLine("Circular Carbon: " + res.CircularCarbonTotal);
                Console.WriteLine("Savings: " + res.ReuseAsPercent + "%");
                Console.WriteLine("Diff from Expected Linear: " +  (Math.Abs(res.LinearCarbonTotal - float.Parse(test_values[0])) * 100) / res.LinearCarbonTotal + "%");
                Console.WriteLine("Diff from Expected Circular: " + (Math.Abs(res.CircularCarbonTotal - float.Parse(test_values[1])) * 100) / res.CircularCarbonTotal + "%");
                Console.WriteLine("Diff from Expected Manufacturing: " + (Math.Abs(res.RawManufacturingCarbon - float.Parse(test_values[2])) * 100) / res.RawManufacturingCarbon + "%");
                Console.WriteLine("Diff from Expected Disposal: " + (Math.Abs(res.RawDisposalCarbon - float.Parse(test_values[3])) * 100) / res.RawDisposalCarbon + "%");            
                
                if (res.RawTransportCarbon != 0)
                {
                    Console.WriteLine("Diff from Expected Transport: " + (Math.Abs(res.RawTransportCarbon - float.Parse(test_values[4])) * 100) / res.RawTransportCarbon + "%");
                }
                else
                {
                    Console.WriteLine("No transport costs.");
                }
                
                Console.WriteLine("---------------------------------------");
            }
                
            Console.ReadLine();
        }
    }
}
