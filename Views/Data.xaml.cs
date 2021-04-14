using Microsoft.Win32;
using ReathUIv0._3.Connections;
using ReathUIv0._3.Models;
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
using System.IO;

namespace ReathUIv0._3.Views
{
    /// <summary>
    /// Interaction logic for Data.xaml
    /// </summary>
    public partial class Data : Window
    {
        private List<string> LoadAsset = new List<string>();
        private ReusableAsset assetSelect = new ReusableAsset();
        private string filePath = AppDomain.CurrentDomain.BaseDirectory + "AssetInfo.csv";

        public Data()
        {
            InitializeComponent();
            textBlock_exportPath.Text = "Select Export Path";
        }

        private void comboBox_AssetSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           assetSelect = SqliteDatabaseAccess.RetrieveAssets(comboBox_AssetSelection.SelectedItem.ToString().Trim());
        }

        private void button_export_Click(object sender, RoutedEventArgs e)
        {
            if(textBlock_exportPath.Text.Equals("Select Export Path") || comboBox_AssetSelection.SelectedItem == null)
            {
                MessageBox.Show("Error Please ensure a Asset has been selected and FilePath selected");
            }
            else if (CheckCsv(comboBox_AssetSelection.SelectedItem.ToString().Trim()) == true)
            {
                MessageBox.Show("Asset is Already in CSV");
            }
            else
            {
                CarbonResults carbonResults = CarbonCalculation.CalculateCarbon(assetSelect);

                var totalEconomicImpactLinear = assetSelect.UnitCost * assetSelect.SampleSize; // Linear economic impact
                var totalEconomicImpactCircular = totalEconomicImpactLinear / assetSelect.MaximumReuses; // Circular economic impact

                double primaryLinearCarbon = carbonResults.Primary.LinearCarbon;
                double primaryCircularCarbon = carbonResults.Primary.CircularCarbon;
                double auxiliaryLinearCarbon = carbonResults.Auxiliary.LinearCarbon;
                double auxiliaryCircularCarbon = carbonResults.Auxiliary.CircularCarbon;
                double totalLinearCarbon = primaryLinearCarbon + auxiliaryLinearCarbon;
                double totalCircularCarbon = primaryCircularCarbon + auxiliaryCircularCarbon;
                double totalEconomicLinear = totalEconomicImpactLinear;
                double totalEconomicCircular = totalEconomicImpactCircular;
                

                try
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(textBlock_exportPath.Text,true))
                    {
                        file.WriteLine(comboBox_AssetSelection.Text + "," + primaryLinearCarbon + "," + primaryCircularCarbon + "," + auxiliaryLinearCarbon + "," + auxiliaryCircularCarbon + "," + totalLinearCarbon + "," + totalCircularCarbon + "," + totalEconomicLinear + "," + totalEconomicCircular);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            }
        }

        private void button_cancelExport_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_exportPath_Click(object sender, RoutedEventArgs e)
        {
            //textBlock_exportPath.Text = "SET TO NEW PATH";

            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Pick CSV File";
            open.Filter = "csv files (*.csv)|*.csv";
            

            if(open.ShowDialog() == true)
            {
                textBlock_exportPath.Text = open.FileName;
            }
        }

        private void comboBox_AssetSelection_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAsset = SqliteDatabaseAccess.RetreiveAssetAndId();

            foreach(string assetId in LoadAsset)
            {
                comboBox_AssetSelection.Items.Add(assetId);
            }
        }

        private void textBlock_exportPath_Loaded(object sender, RoutedEventArgs e)
        {
            

            if (!System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.FileStream fs = System.IO.File.Create(filePath);

                    fs.Close();

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                    {
                        file.WriteLine("Asset Name" + "," + "Primary Linear Carbon Impact" + "," + "Primary Circular Carbon Impact" + "," + "Auxiliary Linear Carbon Impact" + "," + "Auxiliary Circular Carbon Impact" + "," + "Total Linear Carbon Impact" + "," + "Total Circular Carbon Impact" + "," + "Total Economic Impact Linear" + "," + "Total Economic Impact Circular");
                    }

                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }

            textBlock_exportPath.Text = filePath;
            
        }

        private bool CheckCsv(string assetName)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(@filePath);

                for(int i = 0;i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(',');
                    if (fields[0].Equals(assetName))
                    {
                        return true;
                    }

                }

                return false;

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
