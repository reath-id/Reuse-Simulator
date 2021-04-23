using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ReathUIv0._3.Connections;
using ReathUIv0._3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ReathUIv0._3.Views
{
    /// <summary>
    /// Interaction logic for Graphs.xaml
    /// </summary>
    public partial class Graphs : Window, INotifyPropertyChanged
    {
        #region Graph Settings properties

        private string xLabelText;
        public string XLabelText
        {
            get { return xLabelText; }
            set { OnPropertyChanged(ref xLabelText, value); }
        }

        private string yLabelText;
        public string YLabelText
        {
            get { return yLabelText; }
            set { OnPropertyChanged(ref yLabelText, value); }
        }

        private string titleLeftGraph;
        public string TitleLeftGraph
        {
            get { return titleLeftGraph; }
            set { OnPropertyChanged(ref titleLeftGraph, value); }
        }

        private string titleRightGraph;
        public string TitleRightGraph
        {
            get { return titleRightGraph; }
            set { OnPropertyChanged(ref titleRightGraph, value); }
        }

        private int xRotationText;
        public int XRotationText
        {
            get { return xRotationText; }
            set { OnPropertyChanged(ref xRotationText, value); }
        }

        private int yRotationText;
        public int YRotationText
        {
            get { return yRotationText; }
            set { OnPropertyChanged(ref yRotationText, value); }
        }

        private string legendPosition;
        public string LegendPosition
        {
            get { return legendPosition; }
            set { OnPropertyChanged(ref legendPosition, value); }
        }

        private string speedValue;
        public string SpeedValue
        {
            get { return speedValue; }
            set { OnPropertyChanged(ref speedValue, value); }
        }

        private string[] labelLeftGraph;
        public string[] LabelLeftGraph
        {
            get { return labelLeftGraph; }
            set { OnPropertyChanged(ref labelLeftGraph, value); }
        }
        private string[] labelRigthGraph;
        public string[] LabelRigthGraph
        {
            get { return labelRigthGraph; }
            set { OnPropertyChanged(ref labelRigthGraph, value); }
        }

        private Func<double, string> formatterLeftGraph;
        public Func<double, string> FormatterLeftGraph
        {
            get { return formatterLeftGraph; }
            set { OnPropertyChanged(ref formatterLeftGraph, value); }
        }

        private Func<double, string> formatterRightGraph;
        public Func<double, string> FormatterRightGraph
        {
            get { return formatterRightGraph; }
            set { OnPropertyChanged(ref formatterRightGraph, value); }
        }

        private Brush environmentalImpactColour0;
        public Brush EnvironmentalImpactColour0
        {
            get { return environmentalImpactColour0; }
            set { OnPropertyChanged(ref environmentalImpactColour0, value); }
        }

        private Brush environmentalImpactColour1;
        public Brush EnvironmentalImpactColour1
        {
            get { return environmentalImpactColour1; }
            set { OnPropertyChanged(ref environmentalImpactColour1, value); }
        }

        private Brush environmentalImpactColour2;
        public Brush EnvironmentalImpactColour2
        {
            get { return environmentalImpactColour2; }
            set { OnPropertyChanged(ref environmentalImpactColour2, value); }
        }

        private Brush economicImpactColour0;
        public Brush EconomicImpactColour0
        {
            get { return economicImpactColour0; }
            set { OnPropertyChanged(ref economicImpactColour0, value); }
        }

        private Brush economicImpactColour1;
        public Brush EconomicImpactColour1
        {
            get { return economicImpactColour1; }
            set { OnPropertyChanged(ref economicImpactColour1, value); }
        }

        private Brush economicImpactColour2;
        public Brush EconomicImpactColour2
        {
            get { return economicImpactColour2; }
            set { OnPropertyChanged(ref economicImpactColour2, value); }
        }

        private int environmentalSelectedTheme;
        public int EnvironmentalSelectedTheme
        {
            get { return environmentalSelectedTheme; }
            set { OnPropertyChanged(ref environmentalSelectedTheme, value); LoadCompareGraphs(); }
        }

        private int economicsSelectedTheme;
        public int EconomicsSelectedTheme
        {
            get { return economicsSelectedTheme; }
            set { OnPropertyChanged(ref economicsSelectedTheme, value); LoadCompareGraphs(); }
        }

        private ObservableCollection<Brush> environmentalBrushes;
        public ObservableCollection<Brush> EnvironmentalBrushes
        {
            get { return environmentalBrushes; }
            set { OnPropertyChanged(ref environmentalBrushes, value); }
        }

        private ObservableCollection<Brush> economicBrushes;
        public ObservableCollection<Brush> EconomicBrushes
        {
            get { return economicBrushes; }
            set { OnPropertyChanged(ref economicBrushes, value); ; }
        }
        private string economicImpactGraphText;
        public string EconomicImpactGraphText
        {
            get { return economicImpactGraphText; }
            set { OnPropertyChanged(ref economicImpactGraphText, value); }
        }

        private string material1;
        public string Material1
        {
            get { return material1; }
            set { OnPropertyChanged(ref material1, value); }
        }

        private string material2;
        public string Material2
        {
            get { return material2; }
            set { OnPropertyChanged(ref material2, value); }
        }

        #endregion Graph Settings properties

        #region Other properties

        private int assetSelected;
        public int AssetSelected
        {
            get { return assetSelected; }
            set
            {
                OnPropertyChanged(ref assetSelected, value);
                if (assetSelected >= 0)
                {
                    currentAsset = SqliteDatabaseAccess.RetrieveAssets(loadedAssets[AssetSelected]);
                    LoadCompareComboBox();
                    LoadCompareComboBox2();
                    UpdateCarbonResults();
                }
            }
        }

        private int compareSelected;
        public int CompareSelected
        {
            get { return compareSelected; }
            set
            {
                OnPropertyChanged(ref compareSelected, value);
                if (CompareSelected > 0) { UpdateCompareAsset(); }
                if (CompareSelected == 0)
                {
                    UpdateCarbonResults();
                    LoadCompareComboBox2();
                }
            }
        }

        private int compareSelected2;
        public int CompareSelected2
        {
            get { return compareSelected2; }
            set
            {
                OnPropertyChanged(ref compareSelected2, value);
                if (CompareSelected2 > 0) { UpdateCompareAsset(); }
                if (CompareSelected2 == 0)
                {
                    LoadCompareGraphs();
                }
            }
        }

        private SeriesCollection environmnetalSeriesCollection;
        public SeriesCollection EnvironmentalSeriesCollection
        {
            get { return environmnetalSeriesCollection; }
            set { OnPropertyChanged(ref environmnetalSeriesCollection, value); }
        }

        private SeriesCollection economicSeriesCollection;
        public SeriesCollection EconomicSeriesCollection
        {
            get { return economicSeriesCollection; }
            set { OnPropertyChanged(ref economicSeriesCollection, value); }
        }

        private ObservableCollection<string> compareAssetFirstList;
        public ObservableCollection<string> CompareAssetFirstList
        {
            get { return compareAssetFirstList; }
            set { OnPropertyChanged(ref compareAssetFirstList, value); }
        }

        private ObservableCollection<string> compareAssetSecondList;
        public ObservableCollection<string> CompareAssetSecondList
        {
            get { return compareAssetSecondList; }
            set { OnPropertyChanged(ref compareAssetSecondList, value); }
        }

        private ObservableCollection<string> assetsLoad;
        public ObservableCollection<string> AssetsLoad
        {
            get { return assetsLoad; }
            set { OnPropertyChanged(ref assetsLoad, value); }
        }


        #endregion Other properties

        private CarbonResults carbonResults;
        private ReusableAsset currentAsset;
        private ReusableAsset compareAsset;
        private readonly List<string> loadedAssets;

        #region Methods
        public Graphs()
        {
            InitializeComponent();

            loadedAssets = SqliteDatabaseAccess.RetreiveAssetAndId();  // Retrieving all assets from the user's database

            UpdateCarbonResults();  //Loading the graphs labels and settings
            LoadGraphSettings();
            LoadAssetsComboBox();
            CompareSelected = 0;
            CompareSelected2 = 0;
        }
        private void LoadGraphLabels() // Loads the labels for the graphs
        {
            //Labels and title for the Environmental Impact graph
            TitleLeftGraph = "Environmental Impact per " + currentAsset.SampleSize + " packagings";  // Sets title for Environmental graph
            LabelLeftGraph = new[] { "Linear", "Circular" }; // Sets the names of each of the columns group
            FormatterLeftGraph = (x) => string.Format("{0:N2}", x) + " kg CO2e"; // Formatter sets the y axis label, displaying 2 decimals

            //Labels and title for the Economic Impact graph
            TitleRightGraph = "Economic Impact per " + currentAsset.SampleSize + " packagings";  // Sets title for Economic graph
            LabelRigthGraph = new[] { "Linear", "Circular" }; // Sets the names of each of the columns group
            FormatterRightGraph = (x) => string.Format("{0:N2}", x) + " £"; // Formatter sets the y axis label, displaying 2 decimals
            EconomicImpactGraphText = "Cost in £"; // Sets legend title for the economic graph

            XLabelText = ""; // Set the label for the graphs x axis empty
            YLabelText = ""; // Set the label for the graphs y axis empty
        } 
        private void LoadGraphSettings() // Loads the settings for the graphs: legend position, animation speed, etc
        {
            // Settings for the graphs
            LegendPosition = "Top"; // Position of the legend
            XRotationText = 0; // X axis text rotation
            YRotationText = 15; // Y axis text rotation
            SpeedValue = "00:00:0.5"; // Animation speed for the graphs

        } 
        private void LoadEnvironmentalGraph() // Loads environmental graph and creates the columns
        {
            LoadEnvironmentalTheme(); // Loads theme

            // Loading Material2 from the currentAsset. Material1 is used to
            // display the current material on the graph's legend
            Material1 = currentAsset.PrimaryMaterial;

            // Creating the environmental graph
            EnvironmentalSeriesCollection = new SeriesCollection {
                new StackedColumnSeries
                {
                    // Adding values for the linear and circular column
                    Values = new ChartValues<ObservableValue> {
                        new ObservableValue(carbonResults.Primary.LinearCarbon), // Primary material Linear environmnetal impact
                        new ObservableValue(carbonResults.Primary.CircularCarbon) // Primary material Circular environmental impact
                    },
                    PointGeometry=null,
                    Fill=EnvironmentalImpactColour0,
                    Stroke=EnvironmentalImpactColour0,
                    StrokeThickness=8,
                    DataLabels=false,
                    StackMode=StackMode.Values,
                    FontSize=13,
                    MaxColumnWidth=100,
                    LabelsPosition= BarLabelPosition.Perpendicular,
                    Title=Material1
                },
            };

            // If auxiliary material exists in the asset, add it to the environmnetal graph
            if (!String.IsNullOrWhiteSpace(currentAsset.AuxiliaryMaterial))
            {
                // Loading Material2 from the currentAsset. Material2 is used to
                // display the current material on the graph's legend
                Material2 = currentAsset.AuxiliaryMaterial;

                // Adding auxiliar material into the environmental graph
                EnvironmentalSeriesCollection.Add(
                    new StackedColumnSeries
                    {
                        // Adding values for the linear and circular column
                        Values = new ChartValues<ObservableValue> {
                            new ObservableValue(carbonResults.Auxiliary.LinearCarbon), // Auxiliary material Linear environmnetal impact
                            new ObservableValue(carbonResults.Auxiliary.CircularCarbon) // Auxiliary material Circular environmental impact
                        },
                        PointGeometry = null,
                        Fill = EnvironmentalImpactColour1,
                        Stroke = EnvironmentalImpactColour1,
                        StrokeThickness = 8,
                        DataLabels = false,
                        StackMode = StackMode.Values,
                        FontSize = 13,
                        MaxColumnWidth = 100,
                        LabelsPosition = BarLabelPosition.Perpendicular,
                        Title = Material2
                    }
                );
            }
        }
        private void LoadEconomicsGraph() // Loads economics graph and creates the columns
        {
            LoadEconomicsTheme(); // Loads theme

            // Calculating the total of the economic impact: linear and circular
            var totalEconomicImpactLinear = currentAsset.UnitCost * currentAsset.SampleSize; // Linear economic impact
            var totalEconomicImpactCircular = totalEconomicImpactLinear / currentAsset.MaximumReuses; // Circular economic impact

            // Creating the economic graph
            EconomicSeriesCollection = new SeriesCollection {
                new ColumnSeries
                {                                        
                    // Adding values for the linear and circular column
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(totalEconomicImpactLinear),
                        new ObservableValue(totalEconomicImpactCircular)
                    },
                    PointGeometry = null,
                    Fill = EconomicImpactColour0,
                    Stroke = EconomicImpactColour0,
                    StrokeThickness = 8,
                    DataLabels = false,
                    FontSize = 13,
                    MaxColumnWidth = 100,
                    LabelsPosition = BarLabelPosition.Perpendicular,
                    Title = EconomicImpactGraphText
                }
            };
        }
        private void LoadEnvironmentalTheme() // Loads and sets the colour theme for the environmental graph
        {
            // Switches colours for the environmental graph's theme
            switch (EnvironmentalSelectedTheme)
            {
                case 0:
                    EnvironmentalImpactColour0 = (Brush)new BrushConverter().ConvertFromString("#4C3F54");
                    EnvironmentalImpactColour1 = (Brush)new BrushConverter().ConvertFromString("#E5D8ED");
                    EnvironmentalImpactColour2 = (Brush)new BrushConverter().ConvertFromString("#A986C2");
                    break;

                case 1:
                    EnvironmentalImpactColour0 = (Brush)new BrushConverter().ConvertFromString("#A986C2");
                    EnvironmentalImpactColour1 = (Brush)new BrushConverter().ConvertFromString("#9BEBDD");
                    EnvironmentalImpactColour2 = (Brush)new BrushConverter().ConvertFromString("#4C3F54");
                    break;

                case 2:
                    EnvironmentalImpactColour0 = (Brush)new BrushConverter().ConvertFromString("#FFA275");
                    EnvironmentalImpactColour1 = (Brush)new BrushConverter().ConvertFromString("#FE4E61");
                    EnvironmentalImpactColour2 = (Brush)new BrushConverter().ConvertFromString("#E5D8ED");
                    break;

                default:
                    EnvironmentalImpactColour0 = (Brush)new BrushConverter().ConvertFromString("#4C3F54");
                    EnvironmentalImpactColour1 = (Brush)new BrushConverter().ConvertFromString("#E5D8ED");
                    EnvironmentalImpactColour2 = (Brush)new BrushConverter().ConvertFromString("#A986C2");
                    break;
            }

            //Adds colours to a collection in order to use them while comparing assets
            EnvironmentalBrushes = new ObservableCollection<Brush>();
            EnvironmentalBrushes.Add(EnvironmentalImpactColour0);
            EnvironmentalBrushes.Add(EnvironmentalImpactColour1);
            EnvironmentalBrushes.Add(EnvironmentalImpactColour2);
        }
        private void LoadEconomicsTheme() // Loads and sets the colour theme for the economics graph
        {
            // Switches colours for the economics graph's theme
            switch (EconomicsSelectedTheme)
            {
                case 0:
                    EconomicImpactColour0 = (Brush)new BrushConverter().ConvertFromString("#D5440B");
                    EconomicImpactColour1 = (Brush)new BrushConverter().ConvertFromString("#42454B");
                    EconomicImpactColour2 = (Brush)new BrushConverter().ConvertFromString("#FE4E61");
                    break;

                case 1:
                    EconomicImpactColour0 = (Brush)new BrushConverter().ConvertFromString("#42454B");
                    EconomicImpactColour1 = (Brush)new BrushConverter().ConvertFromString("#D5440B");
                    EconomicImpactColour2 = (Brush)new BrushConverter().ConvertFromString("#A986C2");
                    break;

                default:
                    EconomicImpactColour0 = (Brush)new BrushConverter().ConvertFromString("#D5440B");
                    EconomicImpactColour1 = (Brush)new BrushConverter().ConvertFromString("#42454B");
                    EconomicImpactColour2 = (Brush)new BrushConverter().ConvertFromString("#FE4E61");
                    break;
            }
            //Adds colours to a collection in order to use them while comparing assets
            EconomicBrushes = new ObservableCollection<Brush>();
            EconomicBrushes.Add(EconomicImpactColour0);
            EconomicBrushes.Add(EconomicImpactColour1);
            EconomicBrushes.Add(EconomicImpactColour2);
        }
        private void UpdateCarbonResults() // Updates the the carbon results for current asset
        {
            if (currentAsset != null) // Check if currentAsset is not null
            {
                // Using hte currentAsset information to perform the calculations necessary for the graphs
                carbonResults = CarbonCalculation.CalculateCarbon(currentAsset);

                // Loading settings, graphs and setting the graph's labels
                LoadGraphLabels();
                LoadEnvironmentalGraph();
                LoadEconomicsGraph();

            }
        }
        private void UpdateCompareAsset() // Updates the the carbon results when comparing assets
        {
            if (CompareSelected2 > 0) // if only comparing 1 asset
            {
                var comp1 = SqliteDatabaseAccess.RetrieveAssets(CompareAssetFirstList[CompareSelected]);
                var comp2 = SqliteDatabaseAccess.RetrieveAssets(CompareAssetSecondList[CompareSelected2]);

                if (comp1.AssetName == comp2.AssetName)
                {
                    LoadCompareComboBox2();
                    return;
                }
                // Uses the currentAsset information to perform the calculations necessary for the graphs
                carbonResults = CarbonCalculation.CalculateCarbon(compareAsset);
            }

            else // if comparing 2 assets
            {
                LoadCompareComboBox2();
            }

            // Loading settings, graphs and setting the graph's labels
            LoadCompareGraphs();
        }
        private void LoadCompareGraphs() // Loads graphs for comparison
        {
            LoadEnvironmentalTheme();
            LoadEconomicsTheme();

            if (CompareSelected > 0)
            {
                EnvironmentalSeriesCollection = new SeriesCollection();
                EconomicSeriesCollection = new SeriesCollection();
                Dictionary<ObservableCollection<string>, int> assetsToCompare = new Dictionary<ObservableCollection<string>, int>();
                assetsToCompare.Add(AssetsLoad, AssetSelected);
                assetsToCompare.Add(CompareAssetFirstList, CompareSelected);

                if (compareSelected2 > 0)
                {
                    assetsToCompare.Add(CompareAssetSecondList, CompareSelected2);
                }

                var colourIndex = 0;
                foreach (var asset in assetsToCompare)
                {
                    LoadCompareEnvironmentalGraphs(asset, EnvironmentalBrushes[colourIndex]);
                    LoadCompareEconomicsGraphs(asset, EconomicBrushes[colourIndex]);
                    colourIndex++;
                }
            }

            if (CompareSelected == 0)
            {
                carbonResults = CarbonCalculation.CalculateCarbon(currentAsset);
                LoadGraphLabels();
                LoadEnvironmentalGraph();
                LoadEconomicsGraph();
            }
        }
        private void LoadCompareEnvironmentalGraphs(KeyValuePair<ObservableCollection<string>, int> asset, Brush colour) // Creates environmental columns for comparison
        {
            compareAsset = SqliteDatabaseAccess.RetrieveAssets(asset.Key[asset.Value]); // Retrieving asset from the database
            compareAsset.SampleSize = 1000; // Setting assets sample size to 1000

            carbonResults = CarbonCalculation.CalculateCarbon(compareAsset);

            var assetName = compareAsset.AssetName;
            // Creating environmental graph
            var columns = new ColumnSeries
            {
                // Adding values for the linear colunm and circular column
                Values = new ChartValues<ObservableValue> {
                    new ObservableValue(carbonResults.Primary.LinearCarbon + carbonResults.Auxiliary.LinearCarbon ), // Total Linear environmnetal impact
                    new ObservableValue(carbonResults.Primary.CircularCarbon + carbonResults.Auxiliary.CircularCarbon) // Total Circular environmental impact
                },
                PointGeometry = null,
                Fill = colour,
                Stroke = colour,
                StrokeThickness = 8,
                DataLabels = false,
                FontSize = 13,
                MaxColumnWidth = 100 / 1.5,
                LabelsPosition = BarLabelPosition.Perpendicular,
                Title = assetName
            };

            EnvironmentalSeriesCollection.Add(columns); // Adds the created column into the environmental graph
            TitleLeftGraph = "Environmental Impact comparison on " + compareAsset.SampleSize + " packagings"; // Sets environmental graphs title
            LabelLeftGraph = new[] { "Total Linear", "Total Circular" }; // Sets title for the columns group
        }
        private void LoadCompareEconomicsGraphs(KeyValuePair<ObservableCollection<string>, int> asset, Brush colour) // Creates economic columns for comparison
        {
            compareAsset = SqliteDatabaseAccess.RetrieveAssets((asset.Key[asset.Value]));// Retrieving asset from the database
            compareAsset.SampleSize = 1000;  // Setting assets sample to 1000

            // Calculating the total of the economic impact: linear and circular
            var currentAssetTotalEconomicImpactLinear = compareAsset.UnitCost * compareAsset.SampleSize; // Linear economic impact
            var currentAssetTotalEconomicImpactCircular = currentAssetTotalEconomicImpactLinear / compareAsset.MaximumReuses; // Circular economic impact  
            var assetName = compareAsset.AssetName;

            // Creating environmental graph
            var columns = new ColumnSeries
            {
                // Adding values for the linear colunm and circular column
                Values = new ChartValues<ObservableValue> {
                        new ObservableValue(currentAssetTotalEconomicImpactLinear), // Total Linear environmnetal impact column
                        new ObservableValue(currentAssetTotalEconomicImpactCircular) // Total Circular environmental impact column
                    },
                PointGeometry = null,
                Fill = colour,
                Stroke = colour,
                StrokeThickness = 8,
                DataLabels = false,
                FontSize = 13,
                MaxColumnWidth = 100 / 1.5,
                LabelsPosition = BarLabelPosition.Perpendicular,
                Title = assetName
            };

            EconomicSeriesCollection.Add(columns); // Adds the created columns into the encomic graph
            TitleRightGraph = "Economic Impact comparison on " + compareAsset.SampleSize + " packagings"; // Sets economic graphs title
            LabelRigthGraph = new[] { "Total Linear", "Total Circular" }; // Sets title for the columns group
        }
        private void LoadAssetsComboBox() // Loads assets for the first combo box
        {
            AssetsLoad = new ObservableCollection<string>(); 

            foreach (string asset in loadedAssets)
            {
                AssetsLoad.Add(asset);
            }
            AssetSelected = 0; // By default, the selected item will be the one at index 0 
        }
        private void LoadCompareComboBox() // Loads assets for the first combo box
        {
            CompareAssetFirstList = new ObservableCollection<string>();
            CompareAssetFirstList.Add("None");

            foreach (string asset in loadedAssets)
            {
                if (asset.Substring(asset.IndexOf('-') + 1) != currentAsset.AssetName)
                {
                    CompareAssetFirstList.Add(asset);
                }
            }
            CompareSelected = 0;
        }
        private void LoadCompareComboBox2() // Loads assets for the first combo box
        {
            CompareAssetSecondList = new ObservableCollection<string>();

            if (CompareSelected > 0)
            {
                compareAsset = SqliteDatabaseAccess.RetrieveAssets(CompareAssetFirstList[CompareSelected]);

                foreach (string asset in CompareAssetFirstList)
                {
                    if (asset.Substring(asset.IndexOf('-') + 1) != compareAsset.AssetName)
                    {
                        CompareAssetSecondList.Add(asset);
                    }
                }
                CompareSelected2 = 0;
            }
            else
            {
                CompareAssetSecondList.Add("Select Second Asset First");
                CompareSelected2 = 0;
            }

        }

        #endregion Methods

        #region RoutedEventArgs

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            Views.Data context = new Views.Data();
            Window dataWindow = new Views.Data();
            dataWindow.DataContext = context;
            dataWindow.Show();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion RoutedEventArgs

        #region Dropdown menus
        private void LoadEnvironmentalThemeComboBox(object sender, RoutedEventArgs e) // Loads the environmental theme combobox 
        {
            EnvironmentalThemeComboBox.SelectedIndex = 0; // By default, the selected item will be the one at index 0
            EnvironmentalThemeComboBox.Items.Add("Default"); // Adding values
            EnvironmentalThemeComboBox.Items.Add("Alternative"); // Adding values
            EnvironmentalThemeComboBox.Items.Add("Alternative 2"); // Adding values
        }
        private void LoadEconomicThemeComboBox(object sender, RoutedEventArgs e) // Load the economic theme combobox
        {
            EconomicThemeComboBox.SelectedIndex = 0; // By default, the selected item will be the one at index 0
            EconomicThemeComboBox.Items.Add("Default"); // Adding values
            EconomicThemeComboBox.Items.Add("Alternative"); // Adding values
        }

        #endregion Dropdown menus

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object gets a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// We use this function to update the error notification message while verifying
        /// the manual inputs, so we can update the body's error depending on the header's error.
        /// </summary>
        /// The <param name="propertyName"> is the property that is updating its value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// The <param name="propertyName"> is the property that is updating its value.</param>
        protected virtual bool OnPropertyChanged<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion INotifyPropertyChanged Members

        private void Form_KeyDown(object sender, KeyEventArgs e)  //Exits the application with ESC

        {
            if (e.Key == Key.Escape)
            {
                Environment.Exit(0);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}