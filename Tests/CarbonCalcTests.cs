using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ReathUIv0._3.Models;

namespace ReathUIv0._3.Tests
{
    public class CarbonCalcTests
    {
        static bool EpsilonCMP(float a, float b)
        {
            const float Epsilon = 0.00001f;

            return Math.Abs(a - b) < Epsilon;
        }


        [Fact]
        public void TestBlankAsset()           
        {
            ReusableAsset BlankAsset = new ReusableAsset();
            BlankAsset.PrimaryMaterial = "Construction: Aggregates";
            BlankAsset.MaximumReuses = 1;
            BlankAsset.PrimaryManufacturingMethod = ReusableAsset.ManufactoringMethod.Primary;

            CarbonResults carbonResults = CarbonCalculation.CalculateCarbon(BlankAsset);

            Assert.Equal(0, carbonResults.Total.LinearCarbon);
            Assert.Equal(0, carbonResults.Total.CircularCarbon);
        }

        [Fact]
        public void TestManufacturingConstants()
        {
            Manufacturing examplemat = new Manufacturing("TestManufacturingConstants", 1f, 2f, 3f, 4f);
            CarbonCalculation.Testing.addManufacturing(examplemat);

            Assert.Equal(1f, CarbonCalculation.ManufacturingCostFromEnum(CarbonCalculation.GetManufacturingCost("TestManufacturingConstants"), ReusableAsset.ManufactoringMethod.Primary));
            Assert.Equal(2f, CarbonCalculation.ManufacturingCostFromEnum(CarbonCalculation.GetManufacturingCost("TestManufacturingConstants"), ReusableAsset.ManufactoringMethod.Reused));
            Assert.Equal(3f, CarbonCalculation.ManufacturingCostFromEnum(CarbonCalculation.GetManufacturingCost("TestManufacturingConstants"), ReusableAsset.ManufactoringMethod.OpenLoop));
            Assert.Equal(4f, CarbonCalculation.ManufacturingCostFromEnum(CarbonCalculation.GetManufacturingCost("TestManufacturingConstants"), ReusableAsset.ManufactoringMethod.ClosedLoop));
        }

        [Fact]
        public void TestDisposalConstants()
        {
            Disposal examplemat = new Disposal("TestDisposalConstants", 2f, 3f, 4f, 5f, 6f, 7f, 8f);
            CarbonCalculation.Testing.addDisposal(examplemat);

            Assert.Equal(2f, CarbonCalculation.DisposalCostFromEnum(CarbonCalculation.GetDisposalCost("TestDisposalConstants"), ReusableAsset.DisposalMethod.Reuse));
            Assert.Equal(3f, CarbonCalculation.DisposalCostFromEnum(CarbonCalculation.GetDisposalCost("TestDisposalConstants"), ReusableAsset.DisposalMethod.OpenLoop));
            Assert.Equal(4f, CarbonCalculation.DisposalCostFromEnum(CarbonCalculation.GetDisposalCost("TestDisposalConstants"), ReusableAsset.DisposalMethod.ClosedLoop));
            Assert.Equal(5f, CarbonCalculation.DisposalCostFromEnum(CarbonCalculation.GetDisposalCost("TestDisposalConstants"), ReusableAsset.DisposalMethod.Combustion));
            Assert.Equal(6f, CarbonCalculation.DisposalCostFromEnum(CarbonCalculation.GetDisposalCost("TestDisposalConstants"), ReusableAsset.DisposalMethod.Composting));
            Assert.Equal(7f, CarbonCalculation.DisposalCostFromEnum(CarbonCalculation.GetDisposalCost("TestDisposalConstants"), ReusableAsset.DisposalMethod.Landfill));
            Assert.Equal(8f, CarbonCalculation.DisposalCostFromEnum(CarbonCalculation.GetDisposalCost("TestDisposalConstants"), ReusableAsset.DisposalMethod.Anaerobic));
        }

        [Fact]
        public void TestManufacturingResult()
        {
            Manufacturing examplemat = new Manufacturing("TestManufacturingResult", 1.2f, 3f, 9f, -1f);
            CarbonCalculation.Testing.addManufacturing(examplemat);

            float carbonres1 = CarbonCalculation.Detail.GetManufacturingCost("TestManufacturingResult", ReusableAsset.ManufactoringMethod.Primary, 20, 20);

            Assert.True(EpsilonCMP(1.2f*20f*20f*0.001f, carbonres1));


            float carbonres2 = CarbonCalculation.Detail.GetManufacturingCost("TestManufacturingResult", ReusableAsset.ManufactoringMethod.Reused, 1, 1000);

            Assert.True(EpsilonCMP(3f*1f*1000f*0.001f, carbonres2));


            float carbonres3 = CarbonCalculation.Detail.GetManufacturingCost("TestManufacturingResult", ReusableAsset.ManufactoringMethod.OpenLoop, 100, 2);

            Assert.True(EpsilonCMP(2f*100f*9f*0.001f, carbonres3));


            bool invalidexception = false;

            try
            {
                float carbonres4 = CarbonCalculation.Detail.GetManufacturingCost("TestManufacturingResult", ReusableAsset.ManufactoringMethod.ClosedLoop, 1, 1);
            }
            catch (ArgumentException)
            {
                invalidexception = true;
            }

            Assert.True(invalidexception);
        }

        [Fact]
        public void TestDisposalResult()
        {
            Disposal exampledisp = new Disposal("TestDisposalResult", 2f, 3f, 4f, 5f, 6f, 7f, CarbonCalculation.NOT_PRESENT);
            CarbonCalculation.Testing.addDisposal(exampledisp);

            float carbonres1 = CarbonCalculation.Detail.GetDisposalCost("TestDisposalResult", ReusableAsset.DisposalMethod.Reuse, 20, 20);

            Assert.True(EpsilonCMP(2f * 20f * 20f * 0.001f, carbonres1));


            float carbonres2 = CarbonCalculation.Detail.GetDisposalCost("TestDisposalResult", ReusableAsset.DisposalMethod.OpenLoop, 1, 1000);

            Assert.True(EpsilonCMP(3f * 1f * 1000f * 0.001f, carbonres2));


            float carbonres3 = CarbonCalculation.Detail.GetDisposalCost("TestDisposalResult", ReusableAsset.DisposalMethod.ClosedLoop, 100, 2);

            Assert.True(EpsilonCMP(4f * 100f * 2f * 0.001f, carbonres3));


            float carbonres4 = CarbonCalculation.Detail.GetDisposalCost("TestDisposalResult", ReusableAsset.DisposalMethod.Combustion, 5, 5);

            Assert.True(EpsilonCMP(5f * 5 * 5f * 0.001f, carbonres4));


            float carbonres5 = CarbonCalculation.Detail.GetDisposalCost("TestDisposalResult", ReusableAsset.DisposalMethod.Composting, 0.1f, 1600);

            Assert.True(EpsilonCMP(6f * 0.1f * 1600f * 0.001f, carbonres5));


            float carbonres6 = CarbonCalculation.Detail.GetDisposalCost("TestDisposalResult", ReusableAsset.DisposalMethod.Landfill, 0.001f, 1000000);

            Assert.True(EpsilonCMP(7f * 0.001f * 1000000f * 0.001f, carbonres6));


            bool invalidexception = false;

            try
            {
                float carbonres7 = CarbonCalculation.Detail.GetDisposalCost("TestDisposalResult", ReusableAsset.DisposalMethod.Anaerobic, 1, 1);
            }
            catch (ArgumentException)
            {
                invalidexception = true;
            }

            Assert.True(invalidexception);
        }

        [Fact]
        public void TestTransportResult()
        {
            Transport exampletrans = new Transport("TestTransportResult", 12f, 14f);
            CarbonCalculation.Testing.addTransport(exampletrans);

            float carbonres1 = CarbonCalculation.Detail.GetTransportCost("TestTransportResult", 3f, 1000f, 250f);

            Assert.True(EpsilonCMP(26f * 3f * 1000f * 250f * 0.001f, carbonres1));

            float carbonres2 = CarbonCalculation.Detail.GetTransportCost("TestTransportResult", 0.2f, 100000f, 27.5f);

            Assert.True(EpsilonCMP(26f * 0.2f * 100000f * 27.5f * 0.001f, carbonres2));

            float carbonres3 = CarbonCalculation.Detail.GetTransportCost("TestTransportResult", 1f, 245f, 0.5f);

            Assert.True(EpsilonCMP(26f * 1f * 245 * 0.5f * 0.001f, carbonres3));

            bool invalidexception = false;

            try
            {
                float carbonres4 = CarbonCalculation.Detail.GetTransportCost("Nonexistent", 0.2f, 100000f, 27.5f);
            }
            catch (ArgumentException)
            {
                invalidexception = true;
            }

            Assert.True(invalidexception);

        }

        [Fact]
        public void TestValidManufacturing()
        {
            Manufacturing examplemat = new Manufacturing("TestValidManufacturing", 60f, CarbonCalculation.NOT_PRESENT, CarbonCalculation.NOT_PRESENT, CarbonCalculation.NOT_PRESENT);
            CarbonCalculation.Testing.addManufacturing(examplemat);

            string validmaterial = "TestValidManufacturing";
            ReusableAsset.ManufactoringMethod validmethod = ReusableAsset.ManufactoringMethod.Primary;

            Manufacturing material = CarbonCalculation.GetManufacturingCost(validmaterial);
            float manufacturingcost = CarbonCalculation.ManufacturingCostFromEnum(material, validmethod);

            Assert.NotNull(material);
            Assert.Equal(60f, manufacturingcost);
        }

        [Fact]
        public void TestValidDisposal()
        {
            Disposal exampledisp = new Disposal("TestValidDisposal", CarbonCalculation.NOT_PRESENT, CarbonCalculation.NOT_PRESENT, CarbonCalculation.NOT_PRESENT, CarbonCalculation.NOT_PRESENT, CarbonCalculation.NOT_PRESENT, 60f, CarbonCalculation.NOT_PRESENT);
            CarbonCalculation.Testing.addDisposal(exampledisp);

            string validmaterial = "TestValidDisposal";
            ReusableAsset.DisposalMethod validmethod = ReusableAsset.DisposalMethod.Landfill;

            Disposal disposal = CarbonCalculation.GetDisposalCost(validmaterial);
            float disposalcost = CarbonCalculation.DisposalCostFromEnum(disposal, validmethod);

            Assert.NotNull(disposal);
            Assert.Equal(60f, disposalcost);
        }

        [Fact]
        public void TestValidTransport()
        {
            Transport exampledisp = new Transport("TestValidTransport", 1f, 2f);
            CarbonCalculation.Testing.addTransport(exampledisp);

            string validtransport = "TestValidTransport";

            Transport transport = CarbonCalculation.GetTransportCost(validtransport);

            Assert.NotNull(transport);
            Assert.Equal(1f, transport.TravelFactor);
            Assert.Equal(2f, transport.WTTFactor);
        }

        [Fact]
        public void TestInvalidManufacturing()
        {
            string validmaterial = "Construction: Aggregates";
            string invalidmaterial = "Invalid Material";
            ReusableAsset.ManufactoringMethod invalidmethod = (ReusableAsset.ManufactoringMethod)9;

            Manufacturing validmat = CarbonCalculation.GetManufacturingCost(validmaterial);
            Manufacturing invalidmat = null;

            bool invmatexception = false;
            bool invmetexception = false;

            try
            {
                invalidmat = CarbonCalculation.GetManufacturingCost(invalidmaterial);
            }
            catch (ArgumentException)
            {
                
                invmatexception = true;
            }

            float invmanufacturingcost = -0.1f;

            try
            {
                invmanufacturingcost = CarbonCalculation.ManufacturingCostFromEnum(validmat, invalidmethod);
            }
            catch (ArgumentException)
            {
                invmetexception = true;
            }

            Assert.Null(invalidmat);
            Assert.True(invmatexception);
            Assert.True(invmetexception);
        }

        [Fact]
        public void TestInvalidDisposal()
        {
            string validmaterial = "Construction: Aggregates";
            string invalidmaterial = "Invalid Material";
            ReusableAsset.DisposalMethod invalidmethod = (ReusableAsset.DisposalMethod)16;

            Disposal validmat = CarbonCalculation.GetDisposalCost(validmaterial);
            Disposal invalidmat = null;

            bool invmatexception = false;
            bool invmetexception = false;

            try
            {
                invalidmat = CarbonCalculation.GetDisposalCost(invalidmaterial);
            }
            catch (ArgumentException)
            {

                invmatexception = true;
            }

            float invmanufacturingcost = -0.1f;

            try
            {
                invmanufacturingcost = CarbonCalculation.DisposalCostFromEnum(validmat, invalidmethod);
            }
            catch (ArgumentException)
            {
                invmetexception = true;
            }

            Assert.Null(invalidmat);
            Assert.True(invmatexception);
            Assert.True(invmetexception);
        }

        [Fact]
        public void TestInvalidTransport()
        {
            string invalidtransport = "Invalid Transport";

            bool invtrexception = false;

            try
            {
                Transport invalidtr = CarbonCalculation.GetTransportCost(invalidtransport);
            }
            catch (ArgumentException)
            {

                invtrexception = true;
            }

            Assert.True(invtrexception);
        }
    }
}
