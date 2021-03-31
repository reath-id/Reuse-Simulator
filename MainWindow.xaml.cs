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
        private List<Material> materialChoices = new List<Material>();
        private List<Disposal> disposalChoices = new List<Disposal>();
        private List<string> cleaningMethodChoice = new List<string>();
        private List<string> materialEmissionChoice = new List<string>();
        private List<string> countries = new List<string>();
        private ReusableAsset reusableAsset = new ReusableAsset();
        private string dataSampleSize, nameOfAsset, unitCost, unitWeight, primaryMaterialWeight, auxiliarMaterialWeight, recycledPercent, mePercent;

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

        //Unit Weight [5]
        private void textBox_unitWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            unitWeight = textBox_unitWeight.Text.ToString().Trim();
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
            if (dropDown_primaryMaterial.SelectedItem.ToString().Equals("Primary Material") == false)
            {
                textBox_primaryMaterialPercent.IsEnabled = true;
                reusableAsset.PrimaryMaterial = dropDown_primaryMaterial.SelectedItem.ToString().Trim();
                FillPrimaryDisposalMethod(dropDown_primaryMaterial.SelectedItem.ToString().Trim());
                FillPrimaryEmissions();
            }
            else
            {
                textBox_primaryMaterialPercent.Text = "Primary Weight";
                textBox_primaryMaterialPercent.IsEnabled = false;
                reusableAsset.PrimaryMaterial = null;
                reusableAsset.PrimaryDispoMethod = "";
                reusableAsset.PrimaryCleaningMethod = "";
                dropDown_primaryDisposalMethod.SelectedIndex = 0;

                if (dropDown_primaryDisposalMethod.Items.Count.Equals(2) == true)
                {
                    dropDown_primaryDisposalMethod.Items.RemoveAt(1);
                }

                dropDown_primaryManufacturingEmissions.Items.Clear();
                dropDown_primaryManufacturingEmissions.Items.Add("Primary Manufacturing Emission");
                dropDown_primaryManufacturingEmissions.SelectedIndex = 0;
                dropDown_primaryManufacturingEmissions.IsEnabled = false;
                reusableAsset.PrimaryMaterialEmission = "";
            }
        }

        private void FillPrimaryEmissions()
        {
            //reusableAsset.DisposalMethod = dropDown_methodOfDisposal.SelectedItem.ToString().Trim();

            materialEmissionChoice.Clear();

            dropDown_primaryManufacturingEmissions.Items.Clear();

            foreach (Material method in materialChoices)
            {
                if (dropDown_primaryMaterial.SelectedItem.ToString().Trim().Equals(method.ManufacturingMaterial))
                {
                    if (method.MaterialProduction != 0)
                    {
                        materialEmissionChoice.Add("Primary");
                    }

                    if (method.Reused != 0)
                    {
                        materialEmissionChoice.Add("Reuse");
                    }

                    if (method.OpenLoopSource != 0)
                    {
                        materialEmissionChoice.Add("Open Loop");
                    }

                    if (method.ClosedLoopSource != 0)
                    {
                        materialEmissionChoice.Add("Closed Loop");
                    }
                }
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
                if (dropDown_primaryManufacturingEmissions.SelectedItem.ToString().Equals("Primary Manufacturing Emission") == false)
                {
                    reusableAsset.PrimaryMaterialEmission = dropDown_primaryManufacturingEmissions.SelectedItem.ToString().Trim();
                    Console.WriteLine(dropDown_primaryManufacturingEmissions.SelectedItem.ToString().Trim());
                }
            }
            else
            {
                reusableAsset.PrimaryMaterialEmission = "";
            }
        }

        //Aux Material [8]
        private void dropDown_auxMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_auxMaterial.SelectedItem.ToString().Equals("Auxiliar Material") == false)
            {
                textBox_auxMaterialPercent.IsEnabled = true;
                reusableAsset.AuxiliaryMaterial = dropDown_auxMaterial.SelectedItem.ToString().Trim();
                dropDown_auxDisposalMethod.IsEnabled = true;
                FillAuxiliaryDisposalMethod(dropDown_auxMaterial.SelectedItem.ToString().Trim());
                FillAuxiliaryEmissions();
            }
            else
            {
                reusableAsset.AuxiliaryMaterial = null;
                textBox_auxMaterialPercent.Text = "Aux Weight";
                textBox_auxMaterialPercent.IsEnabled = false;
                dropDown_auxDisposalMethod.IsEnabled = false;
                reusableAsset.AuxiliaryDispoMethod = "";
                reusableAsset.AuxiliaryCleaningMethod = "";
                dropDown_auxDisposalMethod.SelectedIndex = 0;
                dropDown_auxCleaningMethod.SelectedIndex = 0;

                dropDown_auxiliaryManufacturingEmissions.Items.Clear();
                dropDown_auxiliaryManufacturingEmissions.Items.Add("Auxiliary Manufacturing Emission");
                dropDown_auxiliaryManufacturingEmissions.SelectedIndex = 0;
                dropDown_auxiliaryManufacturingEmissions.IsEnabled = false;
                reusableAsset.AuxiliaryMaterialEmission = "";
            }
        }

        private void FillAuxiliaryEmissions()
        {
            //reusableAsset.DisposalMethod = dropDown_methodOfDisposal.SelectedItem.ToString().Trim();

            materialEmissionChoice.Clear();

            dropDown_auxiliaryManufacturingEmissions.Items.Clear();

            foreach (Material method in materialChoices)
            {
                if (dropDown_auxMaterial.SelectedItem.ToString().Trim().Equals(method.ManufacturingMaterial))
                {
                    if (method.MaterialProduction != 0)
                    {
                        materialEmissionChoice.Add("Primary");
                    }

                    if (method.Reused != 0)
                    {
                        materialEmissionChoice.Add("Reuse");
                    }

                    if (method.OpenLoopSource != 0)
                    {
                        materialEmissionChoice.Add("Open Loop");
                    }

                    if (method.ClosedLoopSource != 0)
                    {
                        materialEmissionChoice.Add("Closed Loop");
                    }
                }
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
                if (dropDown_auxiliaryManufacturingEmissions.SelectedItem.ToString().Equals("Auxiliary Manufacturing Emission") == false)
                {
                    reusableAsset.AuxiliaryMaterialEmission = dropDown_auxiliaryManufacturingEmissions.SelectedItem.ToString().Trim();
                }
            }
            else
            {
                reusableAsset.PrimaryMaterialEmission = "";
            }
        }

        // Primary Disposal Method
        private void dropDown_primaryDisposalMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_primaryDisposalMethod.SelectedItem.ToString().Equals("Method of Disposal") == false)
            {
                dropDown_primaryDisposalMethod.IsEnabled = true;

                reusableAsset.PrimaryDispoMethod = dropDown_primaryDisposalMethod.SelectedItem.ToString().Trim();

                cleaningMethodChoice.Clear();

                dropDown_primaryCleaningMethod.Items.Clear();

                foreach (Disposal method in disposalChoices)
                {
                    if (dropDown_primaryDisposalMethod.SelectedItem.ToString().Trim().Equals(method.MaterialOption))
                    {
                        if (method.Reuse != 0)
                        {
                            cleaningMethodChoice.Add("Reuse");
                        }

                        if (method.OpenLoop != 0)
                        {
                            cleaningMethodChoice.Add("Open Loop");
                        }

                        if (method.ClosedLoop != 0)
                        {
                            cleaningMethodChoice.Add("Closed Loop");
                        }

                        if (method.Combustion != 0)
                        {
                            cleaningMethodChoice.Add("Combustion");
                        }

                        if (method.Composting != 0)
                        {
                            cleaningMethodChoice.Add("Composting");
                        }

                        if (method.Landfill != 0)
                        {
                            cleaningMethodChoice.Add("Landfill");
                        }

                        if (method.AnaerobicDigestion != 0)
                        {
                            cleaningMethodChoice.Add("Anaerobic digestion");
                        }
                    }
                }

                foreach (string cleaningMethod in cleaningMethodChoice)
                {
                    dropDown_primaryCleaningMethod.Items.Add(cleaningMethod);
                }

                dropDown_primaryCleaningMethod.IsEnabled = true;
            }
            else
            {
                dropDown_primaryCleaningMethod.Items.Clear();
                dropDown_primaryCleaningMethod.Items.Add("Cleaning Method");
                dropDown_primaryCleaningMethod.SelectedIndex = 0;
                dropDown_primaryCleaningMethod.IsEnabled = false;
                reusableAsset.PrimaryDispoMethod = "";
            }
        }

        //Primary Cleaning Method
        private void dropDown_primaryCleaningMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_primaryCleaningMethod.SelectedItem != null)
            {
                if (dropDown_primaryCleaningMethod.SelectedItem.ToString().Equals("Cleaning Method") == false)
                {
                    reusableAsset.PrimaryCleaningMethod = dropDown_primaryCleaningMethod.SelectedItem.ToString().Trim();
                }
            }
            else
            {
                reusableAsset.PrimaryCleaningMethod = "";
            }
        }

        //Auxiliary Disposal Method
        private void dropDown_auxDisposalMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_auxDisposalMethod.SelectedItem.ToString().Equals("Method of Disposal") == false)
            {
                dropDown_auxDisposalMethod.IsEnabled = true;

                reusableAsset.AuxiliaryDispoMethod = dropDown_auxDisposalMethod.SelectedItem.ToString().Trim();

                cleaningMethodChoice.Clear();

                dropDown_auxCleaningMethod.Items.Clear();

                foreach (Disposal method in disposalChoices)
                {
                    if (dropDown_auxDisposalMethod.SelectedItem.ToString().Trim().Equals(method.MaterialOption))
                    {
                        if (method.Reuse != 0)
                        {
                            cleaningMethodChoice.Add("Reuse");
                        }

                        if (method.OpenLoop != 0)
                        {
                            cleaningMethodChoice.Add("Open Loop");
                        }

                        if (method.ClosedLoop != 0)
                        {
                            cleaningMethodChoice.Add("Closed Loop");
                        }

                        if (method.Combustion != 0)
                        {
                            cleaningMethodChoice.Add("Combustion");
                        }

                        if (method.Composting != 0)
                        {
                            cleaningMethodChoice.Add("Composting");
                        }

                        if (method.Landfill != 0)
                        {
                            cleaningMethodChoice.Add("Landfill");
                        }

                        if (method.AnaerobicDigestion != 0)
                        {
                            cleaningMethodChoice.Add("Anaerobic digestion");
                        }
                    }
                }

                foreach (string cleaningMethod in cleaningMethodChoice)
                {
                    dropDown_auxCleaningMethod.Items.Add(cleaningMethod);
                }

                dropDown_auxCleaningMethod.IsEnabled = true;
            }
            else
            {
                dropDown_auxCleaningMethod.Items.Clear();
                dropDown_auxCleaningMethod.Items.Add("Cleaning Method");
                dropDown_auxCleaningMethod.SelectedIndex = 0;
                dropDown_auxCleaningMethod.IsEnabled = false;
                reusableAsset.AuxiliaryDispoMethod = "";
            }
        }

        //Auxiliary Cleaning Method
        private void dropDown_auxCleaningMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_auxCleaningMethod.SelectedItem != null)
            {
                if (dropDown_auxCleaningMethod.SelectedItem.ToString().Equals("Cleaning Method") == false)
                {
                    reusableAsset.AuxiliaryCleaningMethod = dropDown_auxCleaningMethod.SelectedItem.ToString().Trim();
                }
            }
            else
            {
                reusableAsset.AuxiliaryCleaningMethod = "";
            }
        }

        //Is Recycled? [9]
        private void dropDown_isRecycled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_isRecycled.SelectedItem.ToString() == "Yes")
            {
                reusableAsset.IsRecylced = 1;
                textBox_RecycledPercent.IsEnabled = true;
                dropDown_recycledCountryOfOrigin.IsEnabled = true;
            }
            else if (dropDown_isRecycled.SelectedItem.ToString().Equals("Is the Item Recycled"))
            {
                reusableAsset.IsRecylced = 2;
                textBox_RecycledPercent.IsEnabled = false;
            }
            else
            {
                reusableAsset.IsRecylced = 0;
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
        private void dropDown_reuseTimeCycle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_reuseTimeCycle.SelectedItem.ToString().Equals("Reuse Time Cycle") == false)
            {
                reusableAsset.ReuseOccurence = dropDown_reuseTimeCycle.SelectedItem.ToString().Trim();
            }
            else
            {
                reusableAsset.ReuseOccurence = "";
            }
        }

        //Average Distance p/reuse cycle [14]
        private void dropDown_averageDistancePerReuse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dropDown_averageDistancePerReuse.SelectedItem.ToString().Equals("Average Distance Per Reuse") == false)
            {
                reusableAsset.AverageDistanceToReuse = (int)dropDown_averageDistancePerReuse.SelectedItem;
            }
            else
            {
                reusableAsset.AverageDistanceToReuse = 0;
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
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
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
        }

        //Tab button Settings
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
        }

        //Button add Assets
        private void btn_addAsset_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new InputViewModel(reusableAsset, dataSampleSize, nameOfAsset, unitCost, unitWeight, primaryMaterialWeight, auxiliarMaterialWeight, recycledPercent, mePercent);
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
            double gbp = rnd.NextDouble() * (9999.00 - 1.01) + 1.01 + 00.01;
            gbp = Math.Round(gbp, 2);
            textBox_unitCost.Text = Convert.ToString(gbp);


            //unitweight
            int unitweight = rnd.Next(400, 9999);
            textBox_unitWeight.Text = Convert.ToString(unitweight);

            //countryoforigin
            dropDown_countryOfOrigin.SelectedIndex = rnd.Next(dropDown_countryOfOrigin.Items.Count);

            //primarymaterial
            dropDown_primaryMaterial.SelectedIndex = rnd.Next(1, 40);

            //primaryweight
            int y = rnd.Next(1, 47);
            int x = unitweight / 2 + y;
            int primaryweight = x;
            textBox_primaryMaterialPercent.Text = Convert.ToString(primaryweight);

            //auxmaterial
            dropDown_auxMaterial.SelectedIndex = rnd.Next(1, 40);

            //auxweight
            double auxweight = unitweight - primaryweight;
            textBox_auxMaterialPercent.Text = Convert.ToString(auxweight);


            //isrecycled
            dropDown_isRecycled.SelectedIndex = rnd.Next(1, 3);

            //recycledpercent
            if (dropDown_isRecycled.SelectedIndex == 1)
            {
                textBox_RecycledPercent.Text = Convert.ToString(rnd.Next(1, 100));
            }

            //recycledcountry
            if (dropDown_isRecycled.SelectedIndex == 1)
            {
                dropDown_recycledCountryOfOrigin.SelectedIndex = rnd.Next(dropDown_recycledCountryOfOrigin.Items.Count);
            }

            //reusetimecycle
            dropDown_reuseTimeCycle.SelectedIndex = rnd.Next(1, 6);

            /*
            //disposalmethod
            dropDown_methodOfDisposal.SelectedIndex = rnd.Next(dropDown_methodOfDisposal.Items.Count);

            //cleaningmethod
            dropDown_cleaningMethod.SelectedIndex = rnd.Next(dropDown_cleaningMethod.Items.Count);
            */

            //reusedistance
            dropDown_averageDistancePerReuse.SelectedIndex = rnd.Next(1, 50);

            //maximumreuse
            dropDown_maxReuseOfAsset.SelectedIndex = rnd.Next(1, 10);

            //MEpercent
            double mepercent = rnd.NextDouble() * (100.00 - 1.00) + 1.01;
            mepercent = Math.Round(mepercent, 2);
            textBox_mePercent.Text = Convert.ToString(mepercent + 0.00);

            //Primary Manufacturing Emissions
            dropDown_primaryManufacturingEmissions.SelectedIndex = rnd.Next(dropDown_primaryManufacturingEmissions.Items.Count);

            //Auxiliary Manufacturing Emissions
            dropDown_auxiliaryManufacturingEmissions.SelectedIndex = rnd.Next(dropDown_auxiliaryManufacturingEmissions.Items.Count);

            //Primary Disposal Method
            dropDown_primaryCleaningMethod.SelectedIndex = rnd.Next(dropDown_primaryCleaningMethod.Items.Count);

            //Auxiliary Disposal Method
            dropDown_auxCleaningMethod.SelectedIndex = rnd.Next(dropDown_auxCleaningMethod.Items.Count);
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

            dropDown_primaryMaterial.Items.Add("Primary Material");
            dropDown_primaryMaterial.SelectedIndex = 0;

            dropDown_auxMaterial.Items.Add("Auxiliar Material");
            dropDown_auxMaterial.SelectedIndex = 0;

            foreach (Material material in materialChoices)
            {
                dropDown_primaryMaterial.Items.Add(material.ManufacturingMaterial);
                dropDown_auxMaterial.Items.Add(material.ManufacturingMaterial);
            }
        }

        private void dropDown_primaryDisposalMethod_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_primaryDisposalMethod.Items.Add("Method of Disposal");
            dropDown_primaryDisposalMethod.SelectedIndex = 0;
        }

        private void FillPrimaryDisposalMethod(string selectionMade)
        {
            disposalChoices = SqliteDatabaseAccess.LoadDisposal(selectionMade);

            if (disposalChoices.Count() <= 0)
            {
                reusableAsset.PrimaryDispoMethod = "None";
                reusableAsset.PrimaryCleaningMethod = "None";

                if (dropDown_primaryDisposalMethod.Items.Count.Equals(1))
                {
                    dropDown_primaryDisposalMethod.Items.Add("None");
                    dropDown_primaryDisposalMethod.SelectedIndex = 1;
                    dropDown_primaryCleaningMethod.IsEnabled = false;
                    dropDown_primaryDisposalMethod.IsEnabled = false;

                    dropDown_primaryCleaningMethod.Items.Add("None");
                    dropDown_primaryCleaningMethod.SelectedIndex = 0;
                }
                else
                {
                    dropDown_primaryDisposalMethod.SelectedIndex = 0;
                    dropDown_primaryDisposalMethod.Items.RemoveAt(1);
                    dropDown_primaryDisposalMethod.Items.Add("None");
                    dropDown_primaryDisposalMethod.SelectedIndex = 1;
                    dropDown_primaryCleaningMethod.IsEnabled = false;
                    dropDown_primaryDisposalMethod.IsEnabled = false;
                    dropDown_primaryCleaningMethod.Items.Add("None");
                    dropDown_primaryCleaningMethod.SelectedIndex = 0;
                }
            }
            else
            {
                if (dropDown_primaryDisposalMethod.Items.Count.Equals(1))
                {
                    dropDown_primaryDisposalMethod.Items.Add(selectionMade);
                    dropDown_primaryDisposalMethod.SelectedIndex = 1;
                }
                else
                {
                    dropDown_primaryDisposalMethod.SelectedIndex = 0;
                    dropDown_primaryDisposalMethod.Items.RemoveAt(1);
                    dropDown_primaryDisposalMethod.Items.Add(selectionMade);
                    dropDown_primaryDisposalMethod.SelectedIndex = 1;
                }
            }
        }

        private void FillAuxiliaryDisposalMethod(string selectionMade)
        {
            disposalChoices = SqliteDatabaseAccess.LoadDisposal(selectionMade);

            if (disposalChoices.Count() <= 0)
            {
                reusableAsset.AuxiliaryDispoMethod = "None";
                reusableAsset.AuxiliaryCleaningMethod = "None";

                if (dropDown_auxDisposalMethod.Items.Count.Equals(1))
                {
                    dropDown_auxDisposalMethod.Items.Add("None");
                    dropDown_auxDisposalMethod.SelectedIndex = 1;
                    dropDown_auxCleaningMethod.IsEnabled = false;
                    dropDown_auxDisposalMethod.IsEnabled = false;

                    dropDown_auxCleaningMethod.Items.Add("None");
                    dropDown_auxCleaningMethod.SelectedIndex = 0;
                }
                else
                {
                    dropDown_auxDisposalMethod.SelectedIndex = 0;
                    dropDown_auxDisposalMethod.Items.RemoveAt(1);
                    dropDown_auxDisposalMethod.Items.Add("None");
                    dropDown_auxDisposalMethod.SelectedIndex = 1;
                    dropDown_auxCleaningMethod.IsEnabled = false;
                    dropDown_auxDisposalMethod.IsEnabled = false;
                    dropDown_auxCleaningMethod.Items.Add("None");
                    dropDown_auxCleaningMethod.SelectedIndex = 0;
                }
            }
            else
            {
                if (dropDown_auxDisposalMethod.Items.Count.Equals(1))
                {
                    dropDown_auxDisposalMethod.Items.Add(selectionMade);
                    dropDown_auxDisposalMethod.SelectedIndex = 1;
                }
                else
                {
                    dropDown_auxDisposalMethod.SelectedIndex = 0;
                    dropDown_auxDisposalMethod.Items.RemoveAt(1);
                    dropDown_auxDisposalMethod.Items.Add(selectionMade);
                    dropDown_auxDisposalMethod.SelectedIndex = 1;
                }
            }
        }

        private void dropDown_auxDisposalMethod_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_auxDisposalMethod.Items.Add("Method of Disposal");
            dropDown_auxDisposalMethod.SelectedIndex = 0;
        }

        private void dropDown_primaryManufacturingEmissions_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_primaryManufacturingEmissions.Items.Add("Primary Manufacturing Emission");
            dropDown_primaryManufacturingEmissions.SelectedIndex = 0;
            dropDown_primaryManufacturingEmissions.IsEnabled = false;
        }

        private void dropDown_auxiliaryManufacturingEmissions_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_auxiliaryManufacturingEmissions.Items.Add("Auxiliary Manufacturing Emission");
            dropDown_auxiliaryManufacturingEmissions.SelectedIndex = 0;
            dropDown_auxiliaryManufacturingEmissions.IsEnabled = false;
        }

        public void textBox_infoBox_ErrorMessageDisplay(string errorMessage)
        {
            textBox_infoBox.Text = errorMessage;
        }

        private void dropDown_isRecycled_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_isRecycled.Items.Add("Is the Item Recycled");
            dropDown_isRecycled.Items.Add("Yes");
            dropDown_isRecycled.Items.Add("No");

            dropDown_isRecycled.SelectedIndex = 0;
            reusableAsset.IsRecylced = 2;
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

        private void dropDown_reuseTimeCycle_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_reuseTimeCycle.Items.Add("Reuse Time Cycle");
            dropDown_reuseTimeCycle.SelectedIndex = 0;
            dropDown_reuseTimeCycle.Items.Add("Daily");
            dropDown_reuseTimeCycle.Items.Add("Weekly");
            dropDown_reuseTimeCycle.Items.Add("Fortnightly");
            dropDown_reuseTimeCycle.Items.Add("Triweekly");
            dropDown_reuseTimeCycle.Items.Add("Monthly");
            dropDown_reuseTimeCycle.Items.Add("Bimonthly");
        }

        private void dropDown_averageDistancePerReuse_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_averageDistancePerReuse.Items.Add("Average Distance Per Reuse");
            dropDown_averageDistancePerReuse.SelectedIndex = 0;

            for (int i = 1; i < 51; i++)
            {
                dropDown_averageDistancePerReuse.Items.Add(i);
            }
        }

        private void dropDown_maxReuseOfAsset_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_maxReuseOfAsset.Items.Add("Maximum Reuse of Asset");
            dropDown_maxReuseOfAsset.SelectedIndex = 0;

            for (int i = 1; i < 11; i++)
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