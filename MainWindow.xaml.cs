using ReathUIv0._1.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using ReathUIv0._1.Models;
using ReathUIv0._1.ViewModel;

namespace ReathUIv0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> materialChoices = new List<string>();
        private List<Disposal> disposalChoices = new List<Disposal>();
        private List<string> cleaningMethodChoice = new List<string>();
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
            if(dropDown_dateRangeofSample.SelectedItem.ToString().Equals("Date Range of Sample Size") == false)
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
            if(dropDown_countryOfOrigin.SelectedItem.ToString().Equals("Country of Origin") == false)
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
            if(dropDown_primaryMaterial.SelectedItem.ToString().Equals("Primary Material") == false)
            {
                textBox_primaryMaterialPercent.IsEnabled = true;
                reusableAsset.PrimaryMaterial = dropDown_primaryMaterial.SelectedItem.ToString().Trim();
            }
            else
            {
                textBox_primaryMaterialPercent.Text = "Primary Weight";
                textBox_primaryMaterialPercent.IsEnabled = false;
                reusableAsset.PrimaryMaterial = null;
            }
            
        }

        //P M % [7.1]
        private void textBox_primaryMaterialPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            primaryMaterialWeight = textBox_primaryMaterialPercent.Text.ToString().Trim();
        }

        //Aux Material [8]
        private void dropDown_auxMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dropDown_auxMaterial.SelectedItem.ToString().Equals("Auxiliar Material") == false)
            {
                textBox_auxMaterialPercent.IsEnabled = true;
                reusableAsset.AuxiliarMaterial = dropDown_auxMaterial.SelectedItem.ToString().Trim();
            }
            else
            {
                reusableAsset.AuxiliarMaterial = null;
                textBox_auxMaterialPercent.Text = "Aux Weight";
                textBox_auxMaterialPercent.IsEnabled = false;
            }
            
        }

        //A M % [8.1]
        private void textBox_auxMaterialPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            auxiliarMaterialWeight = textBox_auxMaterialPercent.Text.ToString().Trim();
        }
        
        //Is Recycled? [9]
        private void dropDown_isRecycled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dropDown_isRecycled.SelectedItem.ToString() == "Yes")
            {
                reusableAsset.IsRecylced = 1;
                textBox_RecycledPercent.IsEnabled = true;
                dropDown_recycledCountryOfOrigin.IsEnabled = true;
            }
            else if(dropDown_isRecycled.SelectedItem.ToString().Equals("Is the Item Recycled"))
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
                reusableAsset.CleaningMethod = "None";
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

        //Cleaning Method [11]
        private void dropDown_cleaningMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dropDown_cleaningMethod.SelectedItem != null)
            {
                if (dropDown_cleaningMethod.SelectedItem.ToString().Equals("Cleaning Method") == false)
                {
                    reusableAsset.CleaningMethod = dropDown_cleaningMethod.SelectedItem.ToString().Trim();
                }
            }
            else
            {
                reusableAsset.CleaningMethod = "";
            }

            
            
        }

        //Reuse Time Cycle [12]
        private void dropDown_reuseTimeCycle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dropDown_reuseTimeCycle.SelectedItem.ToString().Equals("Reuse Time Cycle") == false)
            {
                reusableAsset.ReuseOccurence = dropDown_reuseTimeCycle.SelectedItem.ToString().Trim();
            }
            else
            {
                reusableAsset.ReuseOccurence = "";
            }
            
        }

        //Method of Disposal [13]
        private void dropDown_methodOfDisposal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dropDown_methodOfDisposal.SelectedItem.ToString().Equals("Method of Disposal") == false)
            {
                reusableAsset.DisposalMethod = dropDown_methodOfDisposal.SelectedItem.ToString().Trim();

                cleaningMethodChoice.Clear();

                dropDown_cleaningMethod.Items.Clear();

                
                foreach(Disposal method in disposalChoices)
                {
                    if (dropDown_methodOfDisposal.SelectedItem.ToString().Trim().Equals(method.MaterialOption))
                    {

                        if(method.Reuse != 0)
                        {
                            cleaningMethodChoice.Add("Re-use");
                        }

                        if(method.OpenLoop != 0)
                        {
                            cleaningMethodChoice.Add("Open Loop");
                        }

                        if(method.ClosedLoop != 0)
                        {
                            cleaningMethodChoice.Add("Closed Loop");
                        }

                        if(method.Combustion != 0)
                        {
                            cleaningMethodChoice.Add("Combustion");
                        }

                        if(method.Composting != 0)
                        {
                            cleaningMethodChoice.Add("Composting");
                        }

                        if(method.Landfill != 0)
                        {
                            cleaningMethodChoice.Add("Landfill");
                        }

                        if(method.AnaerobicDigestion != 0)
                        {
                            cleaningMethodChoice.Add("Anaerobic digestion");
                        }
                    }
                }

                foreach(string cleaningMethod in cleaningMethodChoice)
                {
                    dropDown_cleaningMethod.Items.Add(cleaningMethod);
                }

                dropDown_cleaningMethod.IsEnabled = true;
            }
            else
            {
                dropDown_cleaningMethod.Items.Clear();
                dropDown_cleaningMethod.Items.Add("Cleaning Method");
                dropDown_cleaningMethod.SelectedIndex = 0;
                dropDown_cleaningMethod.IsEnabled = false;
                reusableAsset.DisposalMethod = "";
            }
            
        }

        //Average Distance p/reuse cycle [14]
        private void dropDown_averageDistancePerReuse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dropDown_averageDistancePerReuse.SelectedItem.ToString().Equals("Average Distance Per Reuse") == false)
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
            if(dropDown_maxReuseOfAsset.SelectedItem.ToString().Equals("Maximum Reuse of Asset") == false)
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
            if(textBox_infoBox.Text.Equals("Asset Successfully saved") && btnGraphs.IsEnabled.Equals(false) == true)
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
            double minimum = 1.001;
            double maximum = 9999.001;
            double decimalinsurance = 0.001;

            double gbp = rnd.NextDouble() * (9999.00 - 1.01) + 1.01 + 00.01;
            gbp = Math.Round(gbp, 2);
            //textBox_unitCost.Text = Convert.ToString(gbp);

            string temp = Convert.ToString(gbp);

            int decimalPoint = temp.IndexOf('.') + 1;

            int sum = temp.Length - decimalPoint;

            if (decimalPoint == 0)
            {
                temp = temp + ".00";
                textBox_unitCost.Text = temp;
            }
            if (sum == 1)
            {
                temp = temp + "0";
                textBox_unitCost.Text = temp;
            }
            else
            {
                if(sum == 2)
                {
                    textBox_unitCost.Text = temp;
                }
                else
                {
                    while(sum != 2)
                    {
                        temp.Remove(0, temp.Length - 1);
                        sum = temp.Length - decimalPoint;
                    }

                    textBox_unitCost.Text = temp;
                }
                
            }

            //unitweight
            double unitweight = rnd.NextDouble() * (9999.000 - 1.001) + 1.001 + decimalinsurance;
            unitweight = Math.Round(unitweight, 3);
            //textBox_unitWeight.Text = Convert.ToString(unitweight + 0.000);

            temp = Convert.ToString(unitweight);

            decimalPoint = temp.IndexOf('.') + 1;

            sum = temp.Length - decimalPoint;

            if (decimalPoint == 0)
            {
                temp = temp + ".00";
                textBox_unitWeight.Text = temp;
            }
            else if (sum == 1)
            {
                temp = temp + "00";
                textBox_unitWeight.Text = temp;
            }
            else if (sum == 2)
            {
                temp = temp + "0";
                textBox_unitWeight.Text = temp;
            }
            else
            {
                if (sum == 3)
                {
                    textBox_unitWeight.Text = temp;
                }
                else
                {
                    while (sum != 3)
                    {
                        temp.Remove(0, temp.Length - 1);
                        sum = temp.Length - decimalPoint;
                    }

                    textBox_unitWeight.Text = temp;
                }
                
            }

            //countryoforigin
            dropDown_countryOfOrigin.SelectedIndex = rnd.Next(dropDown_countryOfOrigin.Items.Count);

            //primarymaterial
            dropDown_primaryMaterial.SelectedIndex = rnd.Next(1, 40);

            //primaryweight
            double primaryweight = rnd.NextDouble() * (maximum - minimum) + minimum + decimalinsurance;
            primaryweight = Math.Round(primaryweight, 3);
            textBox_primaryMaterialPercent.Text = Convert.ToString(primaryweight);

            double weightcheck = (unitweight / 2 + 1);

            while (primaryweight >= unitweight || primaryweight < weightcheck)
            {
                primaryweight = rnd.NextDouble() * (maximum - minimum) + minimum + decimalinsurance;
                primaryweight = Math.Round(primaryweight, 3);
                //textBox_primaryMaterialPercent.Text = Convert.ToString(primaryweight);

                temp = Convert.ToString(primaryweight);

                decimalPoint = temp.IndexOf('.') + 1;

                sum = temp.Length - decimalPoint;

                if (decimalPoint == 0)
                {
                    temp = temp + ".000";
                    textBox_primaryMaterialPercent.Text = temp;
                }
                else if (sum == 1)
                {
                    temp = temp + "00";
                    textBox_primaryMaterialPercent.Text = temp;
                }
                else if (sum == 2)
                {
                    temp = temp + "0";
                    textBox_primaryMaterialPercent.Text = temp;
                }
                else
                {
                    if (sum == 3)
                    {
                        textBox_primaryMaterialPercent.Text = temp;
                    }
                    else
                    {
                        while (sum != 3)
                        {
                            temp.Remove(0, temp.Length - 1);
                            sum = temp.Length - decimalPoint;
                        }

                        textBox_primaryMaterialPercent.Text = temp;
                    }
                }
            }

            //auxmaterial
            dropDown_auxMaterial.SelectedIndex = rnd.Next(1, 40);

            //auxweight
            double auxweight = unitweight - primaryweight;
            //textBox_auxMaterialPercent.Text = Convert.ToString(auxweight);

            temp = Convert.ToString(auxweight);

            decimalPoint = temp.IndexOf('.') + 1;

            sum = temp.Length - decimalPoint;

            if(decimalPoint == 0)
            {
                temp = temp + ".000";
                textBox_auxMaterialPercent.Text = temp;
            }
            else if (sum == 1)
            {
                temp = temp + "00";
                textBox_auxMaterialPercent.Text = temp;
            }
            else if (sum == 2)
            {
                temp = temp + "0";
                textBox_auxMaterialPercent.Text = temp;
            }
            else
            {
                if (sum == 3)
                {
                    textBox_auxMaterialPercent.Text = temp;
                }
                else
                {
                    while (sum != 3)
                    {
                        temp.Remove(0, temp.Length - 1);
                        sum = temp.Length - decimalPoint;
                    }

                    textBox_auxMaterialPercent.Text = temp;
                }

            }

            //isrecycled
            dropDown_isRecycled.SelectedIndex = rnd.Next(1, 3);

            //recycledpercent
            if(dropDown_isRecycled.SelectedIndex == 1)
            {
                textBox_RecycledPercent.Text = Convert.ToString(rnd.Next(1, 100));
            }

            //recycledcountry
            if(dropDown_isRecycled.SelectedIndex == 1)
            {
                dropDown_recycledCountryOfOrigin.SelectedIndex = rnd.Next(dropDown_recycledCountryOfOrigin.Items.Count);
            }
            

            //reusetimecycle 
            dropDown_reuseTimeCycle.SelectedIndex = rnd.Next(1, 6);

            //disposalmethod
            dropDown_methodOfDisposal.SelectedIndex = rnd.Next(dropDown_methodOfDisposal.Items.Count);

            //cleaningmethod
            dropDown_cleaningMethod.SelectedIndex = rnd.Next(dropDown_cleaningMethod.Items.Count);

            //reusedistance
            dropDown_averageDistancePerReuse.SelectedIndex = rnd.Next(1, 50);

            //maximumreuse
            dropDown_maxReuseOfAsset.SelectedIndex = rnd.Next(1, 10);

            //MEpercent
            double mepercent = rnd.NextDouble() * (100.00 - 1.00) + 1.01;
            mepercent = Math.Round(mepercent, 2);
            textBox_mePercent.Text = Convert.ToString(mepercent + 0.00);

            temp = Convert.ToString(mepercent);

            decimalPoint = temp.IndexOf('.') + 1;

            sum = temp.Length - decimalPoint;

            if (decimalPoint == 0)
            {
                temp = temp + ".00";
                textBox_auxMaterialPercent.Text = temp;
            }
            else if (sum == 1)
            {
                temp = temp + "0";
                textBox_mePercent.Text = temp;
            }
            else
            {
                if (sum == 2)
                {
                    textBox_mePercent.Text = temp;
                }
                else
                {
                    while (sum != 2)
                    {
                        temp.Remove(0, temp.Length - 1);
                        sum = temp.Length - decimalPoint;
                    }

                    textBox_mePercent.Text = temp;
                }
                textBox_mePercent.Text = temp;
            }
        }

        #region Loading data/When loaded

        private void btnGraphs_Loaded(object sender, RoutedEventArgs e)
        {
            if(SqliteDatabaseAccess.CheckData() == true)
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
            
            foreach(string material in materialChoices)
            {
                dropDown_primaryMaterial.Items.Add(material);
                dropDown_auxMaterial.Items.Add(material);
            }
            
            
        }

        private void dropDown_methodOfDisposal_Loaded(object sender, RoutedEventArgs e)
        {
            disposalChoices = SqliteDatabaseAccess.LoadDisposal();

            dropDown_methodOfDisposal.Items.Add("Method of Disposal");
            dropDown_methodOfDisposal.SelectedIndex = 0;

            foreach(Disposal disposalMethod in disposalChoices)
            {
                dropDown_methodOfDisposal.Items.Add(disposalMethod.MaterialOption);
            }

            dropDown_cleaningMethod.IsEnabled = false;
        }

        private void dropDown_cleaningMethod_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_cleaningMethod.Items.Add("Cleaning Method");
            dropDown_cleaningMethod.SelectedIndex = 0;
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

            foreach(CultureInfo getCulture in getCultureInfo)
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

            foreach(string country in countries)
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

            for(int i = 1;i < 51; i++)
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
        #endregion
    }
}
