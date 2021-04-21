using ReathUIv0._3.Connections;
using ReathUIv0._3.Models;
using ReathUIv0._3.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ReathUIv0._3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Manufacturing> materialChoices = new List<Manufacturing>();
        private List<Disposal> disposalChoices = new List<Disposal>();
        private List<string> cleaningMethodChoice = new List<string>();
        private List<string> materialEmissionChoice = new List<string>();
        private List<string> countries = new List<string>();
        private ReusableAsset reusableAsset = new ReusableAsset();
        private string dataSampleSize, nameOfAsset, unitCost, unitWeight, primaryMaterialWeight, auxiliarMaterialWeight, recycledPercent, mePercent, reuseOccurence, avgDistance;        
        //last edit 11:07 10/04/2021 sean mcallister
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new InputViewModel();
        }

        //Data Sample Size [1]
        private void textBox_dataSampleSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataSampleSize = textBox_dataSampleSize.Text.ToString().Trim();
        }

        //Data Range of Sample [2]
        private void dropDown_dateRangeofSample_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_dateRangeofSample.SelectedItem.ToString().Equals("Date Range of Sample Size") == false)
            {
                reusableAsset.DateRange = dropDown_dateRangeofSample.SelectedItem.ToString().Trim();
            }
            else
            {
                reusableAsset.DateRange = "Date Range of Sample Size";
            }
        }

        //Name of Asset [3]
        private void textBox_nameOfAsset_TextChanged(object sender, TextChangedEventArgs e)
        {
            nameOfAsset = textBox_nameOfAsset.Text.ToString().Trim();
        }

        //Unit Cost GBP [4]
        private void textBox_unitCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            unitCost = textBox_unitCost.Text.ToString().Trim();
        }

        //Country of Orgin [6]
        private void dropDown_countryOfOrigin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_countryOfOrigin.SelectedItem.ToString().Equals("Country of Origin") == false)
            {
                reusableAsset.AssetCountryOfOrigin = dropDown_countryOfOrigin.SelectedItem.ToString().Trim();
            }
            else
            {
                reusableAsset.AssetCountryOfOrigin = null;
            }
        }

        //Primary Material [7]
        private void dropDown_primaryMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox_primaryMaterialPercent.IsEnabled = true;
            reusableAsset.PrimaryMaterial = dropDown_primaryMaterial.SelectedItem.ToString().Trim();

            reusableAsset.PrimaryManufacturingMethod = ReusableAsset.ManufactoringMethod.None;
            reusableAsset.PrimaryDisposalMethod = ReusableAsset.DisposalMethod.None;

            dropDown_primaryDisposalMethod.SelectedIndex = -1;
            dropDown_primaryManufacturingEmissions.SelectedIndex = -1;

            materialEmissionChoice.Clear();
            dropDown_primaryDisposalMethod.Items.Clear();

            Disposal disposal = CarbonCalculation.GetDisposalCost(reusableAsset.PrimaryMaterial);

            if (disposal.Landfill != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Landfill");
            }

            if (disposal.Reuse != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Reuse");
            }

            if (disposal.OpenLoop != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Open Loop");
            }

            if (disposal.ClosedLoop != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Closed Loop");
            }

            if (disposal.Composting != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Composting");
            }

            if (disposal.Combustion != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Combustion");
            }

            if (disposal.AnaerobicDigestion != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Anaerobic");
            }

            foreach (string matEmissionMethod in materialEmissionChoice)
            {
                dropDown_primaryDisposalMethod.Items.Add(matEmissionMethod);
            }

            dropDown_primaryDisposalMethod.IsEnabled = true;


            materialEmissionChoice.Clear();

            dropDown_primaryManufacturingEmissions.Items.Clear();

            Manufacturing method = CarbonCalculation.GetManufacturingCost(dropDown_primaryMaterial.SelectedItem.ToString().Trim());

            if (method.Primary != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Primary");
            }

            if (method.Reused != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Reused");
            }

            if (method.OpenLoop != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Open Loop");
            }

            if (method.ClosedLoop != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Closed Loop");
            }

            foreach (string matEmissionMethod in materialEmissionChoice)
            {
                dropDown_primaryManufacturingEmissions.Items.Add(matEmissionMethod);
            }

            dropDown_primaryManufacturingEmissions.IsEnabled = true;
        }

        //P M % [7.1]
        private void textBox_primaryMaterialPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            primaryMaterialWeight = textBox_primaryMaterialPercent.Text.ToString().Trim();
        }

        private void dropDown_primaryManufacturingEmissions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_primaryManufacturingEmissions.SelectedItem != null)
            {
                 reusableAsset.PrimaryManufacturingMethod = ReusableAsset.StringToManufacturingMethod(dropDown_primaryManufacturingEmissions.SelectedItem.ToString().Trim());

                 Console.WriteLine(dropDown_primaryManufacturingEmissions.SelectedItem.ToString().Trim());
            }
            else
            {
                reusableAsset.PrimaryManufacturingMethod = ReusableAsset.ManufactoringMethod.None;
            }
        }

        //Aux Material [8]
        private void dropDown_auxMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox_auxMaterialPercent.IsEnabled = true;
            reusableAsset.AuxiliaryMaterial = dropDown_auxMaterial.SelectedItem.ToString().Trim();

            reusableAsset.AuxiliaryManufacturingMethod = ReusableAsset.ManufactoringMethod.None;
            reusableAsset.AuxiliaryDisposalMethod = ReusableAsset.DisposalMethod.None;

            dropDown_auxDisposalMethod.SelectedIndex = -1;
            dropDown_auxiliaryManufacturingEmissions.SelectedIndex = -1;

            materialEmissionChoice.Clear();
            dropDown_auxDisposalMethod.Items.Clear();

            Disposal disposal = CarbonCalculation.GetDisposalCost(reusableAsset.AuxiliaryMaterial);

            if (disposal.Landfill != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Landfill");
            }

            if (disposal.Reuse != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Reuse");
            }

            if (disposal.OpenLoop != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Open Loop");
            }

            if (disposal.ClosedLoop != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Closed Loop");
            }

            if (disposal.Composting != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Composting");
            }

            if (disposal.Combustion != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Combustion");
            }

            if (disposal.AnaerobicDigestion != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Anaerobic");
            }

            foreach (string matEmissionMethod in materialEmissionChoice)
            {
                dropDown_auxDisposalMethod.Items.Add(matEmissionMethod);
            }

            dropDown_auxDisposalMethod.IsEnabled = true;


            materialEmissionChoice.Clear();

            dropDown_auxiliaryManufacturingEmissions.Items.Clear();

            Manufacturing method = CarbonCalculation.GetManufacturingCost(reusableAsset.AuxiliaryMaterial);

            if (method.Primary != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Primary");
            }

            if (method.Reused != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Reused");
            }

            if (method.OpenLoop != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Open Loop");
            }

            if (method.ClosedLoop != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Closed Loop");
            }

            foreach (string matEmissionMethod in materialEmissionChoice)
            {
                dropDown_auxiliaryManufacturingEmissions.Items.Add(matEmissionMethod);
            }

            dropDown_auxiliaryManufacturingEmissions.IsEnabled = true;

        }

        private void FillAuxiliaryEmissions()
        {

            materialEmissionChoice.Clear();

            dropDown_auxiliaryManufacturingEmissions.Items.Clear();

            Manufacturing method = CarbonCalculation.GetManufacturingCost(dropDown_auxMaterial.SelectedItem.ToString().Trim());

            if (method.Primary != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Primary");
            }

            if (method.Reused != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Reuse");
            }

            if (method.OpenLoop != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Open Loop");
            }

            if (method.ClosedLoop != CarbonCalculation.NOT_PRESENT)
            {
                materialEmissionChoice.Add("Closed Loop");
            }

            foreach (string matEmissionMethod in materialEmissionChoice)
            {
                dropDown_auxiliaryManufacturingEmissions.Items.Add(matEmissionMethod);
            }

            dropDown_auxiliaryManufacturingEmissions.IsEnabled = true;
        }

        //A M % [8.1]
        private void textBox_auxMaterialPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            auxiliarMaterialWeight = textBox_auxMaterialPercent.Text.ToString().Trim();
        }

        private void dropDown_auxiliaryManufacturingEmissions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_auxiliaryManufacturingEmissions.SelectedItem != null)
            {
                reusableAsset.AuxiliaryManufacturingMethod = ReusableAsset.StringToManufacturingMethod(dropDown_auxiliaryManufacturingEmissions.SelectedItem.ToString().Trim());
            }
            else
            {
                reusableAsset.AuxiliaryManufacturingMethod = ReusableAsset.ManufactoringMethod.None;
            }
        }

        // Primary Disposal Method
        private void dropDown_primaryDisposalMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_primaryDisposalMethod.SelectedItem != null)
            {
                reusableAsset.PrimaryDisposalMethod = ReusableAsset.StringToDisposalMethod(dropDown_primaryDisposalMethod.SelectedItem.ToString().Trim());
            }
            else
            {
                reusableAsset.PrimaryDisposalMethod = ReusableAsset.DisposalMethod.None;
            }
        }

        //Auxiliary Disposal Method
        private void dropDown_auxDisposalMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_auxDisposalMethod.SelectedItem != null)
            {
                reusableAsset.AuxiliaryDisposalMethod = ReusableAsset.StringToDisposalMethod(dropDown_auxDisposalMethod.SelectedItem.ToString().Trim());
            }
            else
            {
                reusableAsset.AuxiliaryDisposalMethod = ReusableAsset.DisposalMethod.None;
            }
        }

        //Is Recycled? [9]
        private void dropDown_isRecycled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_isRecycled.SelectedItem.ToString() == "Yes")
            {
                reusableAsset.IsRecycled = true;
                textBox_RecycledPercent.IsEnabled = true;
                dropDown_recycledCountryOfOrigin.IsEnabled = true;
            }
            else if (dropDown_isRecycled.SelectedItem.ToString().Equals("Is the Item Recycled")) // TODO: why is this a branch
            {
                reusableAsset.IsRecycled = false;
                textBox_RecycledPercent.IsEnabled = false;
            }
            else
            {
                reusableAsset.IsRecycled = false;
                textBox_RecycledPercent.IsEnabled = false;
                dropDown_recycledCountryOfOrigin.IsEnabled = false;
                dropDown_recycledCountryOfOrigin.SelectedIndex = 0;
                textBox_RecycledPercent.Text = "Recycled Percent";
                recycledPercent = "0";
                reusableAsset.RecycledCountryOfOrigin = "None";
            }
        }

        //R P % [9.1]
        private void textBox_RecycledPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            recycledPercent = textBox_RecycledPercent.Text.ToString().Trim();
        }

        //Country of Origin [10]
        private void dropDown_recycledCountryOfOrigin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_recycledCountryOfOrigin.SelectedItem.ToString().Equals("Recycled Country of Origin") == false)
            {
                reusableAsset.RecycledCountryOfOrigin = dropDown_recycledCountryOfOrigin.SelectedItem.ToString().Trim();
            }
            else
            {
                reusableAsset.RecycledCountryOfOrigin = "";
            }
        }

        //Reuse Time Cycle [12]
        private void textbox_reuseTimeCycle_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(textbox_reuseTimeCycle.Text.Equals("Time Cycle") == false)
            {
                reuseOccurence = textbox_reuseTimeCycle.Text;
            }
            else
            {
                reuseOccurence = "";
            }
        }

        //Average Distance p/reuse cycle [14]
        private void textbox_averageDistancePerReuse_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(textbox_averageDistancePerReuse.Text.Equals("Avg Distance per Reuse") == false)
            {
                avgDistance = textbox_averageDistancePerReuse.Text;
            }
            else
            {
                avgDistance = "";
            }
        }

        //Maximum Reuses of Asset [15]
        private void dropDown_maxReuseOfAsset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_maxReuseOfAsset.SelectedItem.ToString().Equals("Maximum Reuse of Asset") == false)
            {
                reusableAsset.MaximumReuses = (int)dropDown_maxReuseOfAsset.SelectedItem;
            }
            else
            {
                reusableAsset.MaximumReuses = 0;
            }
        }

        //M E % [16]
        private void textBox_mePercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            mePercent = textBox_mePercent.Text.ToString().Trim();
        }

        //Info Box [17]
        private void textBox_infoBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox_infoBox.Text.Equals("Asset Successfully saved") && btnGraphs.IsEnabled.Equals(false) == true)
            {
                btnGraphs.IsEnabled = true;
                btnData.IsEnabled = true;
            }
        }

        //Tab button Input
        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
        }

        //Tab button Graphs
        private void btnGraphs_Click(object sender, RoutedEventArgs e)
        {
            //I changed this in order to get INotifyProperty working on Graphs
            this.Hide();
            Views.Graphs context = new Views.Graphs();
            Window window = new Views.Graphs();
            window.DataContext = context;
            window.Show();
        }

        //Tab button Data
        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            Views.Data context = new Views.Data();
            Window dataWindow = new Views.Data();
            dataWindow.DataContext = context;
            dataWindow.Show();
        }

        //Tab button Settings
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
        }

        //Button add Assets
        private void btn_addAsset_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new InputViewModel(reusableAsset, dataSampleSize, nameOfAsset, unitCost, unitWeight, primaryMaterialWeight, auxiliarMaterialWeight, recycledPercent, mePercent, reuseOccurence, avgDistance);
        }

        //Button generate Random Data
        private void btn_randomData_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            //datasamplesize
            textBox_dataSampleSize.Text = Convert.ToString(rnd.Next(1, 999999));

            //datarangesample
            dropDown_dateRangeofSample.SelectedIndex = rnd.Next(1, 15);

            //nameofasset
            textBox_nameOfAsset.Text = "Asset" + rnd.Next(1, 50);

            //unitcost
            int gbp = rnd.Next(1, 20);
            textBox_unitCost.Text = Convert.ToString(gbp);


            //countryoforigin
            dropDown_countryOfOrigin.SelectedIndex = rnd.Next(dropDown_countryOfOrigin.Items.Count + 1);


            //primarymaterial
            dropDown_primaryMaterial.SelectedIndex = rnd.Next(1, 40);

            //primaryweight
            double primaryweight = rnd.Next(5, 1000);
            textBox_primaryMaterialPercent.Text = Convert.ToString(primaryweight);

            //auxmaterial
            dropDown_auxMaterial.SelectedIndex = rnd.Next(1, 40);

            //auxweight
            double auxweight = rnd.Next(1, 150);
            textBox_auxMaterialPercent.Text = Convert.ToString(auxweight);

            //isrecycled
            dropDown_isRecycled.SelectedIndex = rnd.Next(0, 2);

            //recycledpercent
            if (dropDown_isRecycled.SelectedIndex == 0)
            {
                textBox_RecycledPercent.Text = Convert.ToString(rnd.Next(1, 100));
            }
            else
            {
                textBox_RecycledPercent.Text = "";
            }

            //recycledcountry
            if (dropDown_isRecycled.SelectedIndex == 1)
            {
                dropDown_recycledCountryOfOrigin.SelectedIndex = rnd.Next(dropDown_recycledCountryOfOrigin.Items.Count);
                while (dropDown_recycledCountryOfOrigin.SelectedIndex == 0)
                {
                    dropDown_recycledCountryOfOrigin.SelectedIndex = rnd.Next(dropDown_recycledCountryOfOrigin.Items.Count);
                }
            }

            //singlecycletime
            double singlecycle = rnd.Next(1, 120);
            int chance = rnd.Next(1, 2);
            if(chance == 1)
            {
                textbox_reuseTimeCycle.Text = Convert.ToString(singlecycle) + ".0";
            }
            else
            {
                textbox_reuseTimeCycle.Text = Convert.ToString(singlecycle) + ".5";
            }

            //disposalmethod
            dropDown_primaryDisposalMethod.SelectedIndex = rnd.Next(dropDown_primaryDisposalMethod.Items.Count);
            dropDown_auxDisposalMethod.SelectedIndex = rnd.Next(dropDown_auxDisposalMethod.Items.Count);

            //reusedistance
            double dist = rnd.Next(1, 500);
            textbox_averageDistancePerReuse.Text = Convert.ToString(dist) + ".0";

            //maximumreuse
            dropDown_maxReuseOfAsset.SelectedIndex = rnd.Next(1, 25);

            //MEpercent
            double mepercent = rnd.NextDouble() * (100.00 - 1.00) + 1.01;
            mepercent = Math.Round(mepercent, 2);
            textBox_mePercent.Text = Convert.ToString(mepercent + 0.00);

            //Primary Manufacturing Emissions
            dropDown_primaryManufacturingEmissions.SelectedIndex = rnd.Next(dropDown_primaryManufacturingEmissions.Items.Count);

            //Auxiliary Manufacturing Emissions
            dropDown_auxiliaryManufacturingEmissions.SelectedIndex = rnd.Next(dropDown_auxiliaryManufacturingEmissions.Items.Count);


        }

        #region Loading data/When loaded

        private void btnGraphs_Loaded(object sender, RoutedEventArgs e)
        {
            if (SqliteDatabaseAccess.CheckData() == true)
            {
                btnGraphs.IsEnabled = true;
            }
            else
            {
                btnGraphs.IsEnabled = false;
            }
        }

        private void btnData_Loaded(object sender, RoutedEventArgs e)
        {
            if (SqliteDatabaseAccess.CheckData() == true)
            {
                btnData.IsEnabled = true;
            }
            else
            {
                btnData.IsEnabled = false;
            }
        }

        private void dropDown_primaryMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            materialChoices = SqliteDatabaseAccess.LoadMaterials();

            dropDown_primaryMaterial.SelectedIndex = -1;

            dropDown_auxMaterial.SelectedIndex = -1;

            foreach (Manufacturing material in materialChoices)
            {
                dropDown_primaryMaterial.Items.Add(material.Material);
                dropDown_auxMaterial.Items.Add(material.Material);
            }
        }

        private void dropDown_primaryDisposalMethod_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_primaryDisposalMethod.SelectedIndex = -1;
        }


        private void dropDown_auxDisposalMethod_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_auxDisposalMethod.SelectedIndex = -1;
        }

        private void dropDown_primaryManufacturingEmissions_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_primaryManufacturingEmissions.SelectedIndex = -1;
            dropDown_primaryManufacturingEmissions.IsEnabled = false;
        }



        private void dropDown_auxiliaryManufacturingEmissions_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_auxiliaryManufacturingEmissions.SelectedIndex = -1;
            dropDown_auxiliaryManufacturingEmissions.IsEnabled = false;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        

        public void textBox_infoBox_ErrorMessageDisplay(string errorMessage)
        {
            textBox_infoBox.Text = errorMessage;
        }

        private void dropDown_isRecycled_Loaded(object sender, RoutedEventArgs e)
        {
            //dropDown_isRecycled.Items.Add("Is the Item Recycled");
            dropDown_isRecycled.Items.Add("Yes");
            dropDown_isRecycled.Items.Add("No");

            dropDown_isRecycled.SelectedIndex = 1;
            reusableAsset.IsRecycled = false;
        }

        private void dropDown_countryOfOrigin_Loaded(object sender, RoutedEventArgs e)
        {
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo getCulture in getCultureInfo)
            {
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);
                if (!(countries.Contains(getRegionInfo.EnglishName)))
                {
                    countries.Add(getRegionInfo.EnglishName);
                }
            }

            countries.Sort();

            dropDown_countryOfOrigin.Items.Add("Country of Origin");
            dropDown_countryOfOrigin.SelectedIndex = 0;
            dropDown_recycledCountryOfOrigin.Items.Add("Recycled Country of Origin");
            dropDown_recycledCountryOfOrigin.SelectedIndex = 0;

            foreach (string country in countries)
            {
                dropDown_countryOfOrigin.Items.Add(country);
                dropDown_recycledCountryOfOrigin.Items.Add(country);
            }
        }

        private void dropDown_maxReuseOfAsset_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_maxReuseOfAsset.Items.Add("Maximum Reuse of Asset");
            dropDown_maxReuseOfAsset.SelectedIndex = 0;

            for (int i = 5; i < 31; i++)
            {
                dropDown_maxReuseOfAsset.Items.Add(i);
            }
        }

        private void dropDown_dateRangeofSample_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_dateRangeofSample.Items.Add("Date Range of Sample Size");
            dropDown_dateRangeofSample.SelectedIndex = 0;
            dropDown_dateRangeofSample.Items.Add("1 Week");
            dropDown_dateRangeofSample.Items.Add("2 Weeks");
            dropDown_dateRangeofSample.Items.Add("3 Weeks");
            dropDown_dateRangeofSample.Items.Add("1 Month");
            dropDown_dateRangeofSample.Items.Add("2 Months");
            dropDown_dateRangeofSample.Items.Add("3 Months");
            dropDown_dateRangeofSample.Items.Add("4 Months");
            dropDown_dateRangeofSample.Items.Add("5 Months");
            dropDown_dateRangeofSample.Items.Add("6 Months");
            dropDown_dateRangeofSample.Items.Add("8 Months");
            dropDown_dateRangeofSample.Items.Add("9 Months");
            dropDown_dateRangeofSample.Items.Add("10 Months");
            dropDown_dateRangeofSample.Items.Add("11 Months");
            dropDown_dateRangeofSample.Items.Add("1 year");
        }

        #endregion Loading data/When loaded
    }
}