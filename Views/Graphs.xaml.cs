using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ReathUIv0._3.Connections;
using ReathUIv0._3.Models;
using System;
using System.Collections.Generic;
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

        private string titleLabelText;

        public string TitleLabelText
        {
            get { return titleLabelText; }
            set { OnPropertyChanged(ref titleLabelText, value); }
        }

        private string titleLabelText2;

        public string TitleLabelText2
        {
            get { return titleLabelText2; }
            set { OnPropertyChanged(ref titleLabelText2, value); }
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

        public string[] Labels { get; set; }
        public string[] Labels2 { get; set; }
        private Func<double, string> formatter;

        public Func<double, string> Formatter
        {
            get { return formatter; }
            set { OnPropertyChanged(ref formatter, value); }
        }

        private Func<double, string> formatter2;

        public Func<double, string> Formatter2
        {
            get { return formatter2; }
            set { OnPropertyChanged(ref formatter2, value); }
        }

        private Brush material1Colour;

        public Brush Material1Colour
        {
            get { return material1Colour; }
            set { OnPropertyChanged(ref material1Colour, value); }
        }

        private Brush material2Colour;

        public Brush Material2Colour
        {
            get { return material2Colour; }
            set { OnPropertyChanged(ref material2Colour, value); }
        }

        #endregion Graph Settings properties

        #region Materials properties

        private string economicImpact;

        public string EconomicImpact
        {
            get { return economicImpact; }
            set { OnPropertyChanged(ref economicImpact, value); }
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

        private ChartValues<ObservableValue> material1Value;

        public ChartValues<ObservableValue> Material1Value
        {
            get { return material1Value; }
            set { OnPropertyChanged(ref material1Value, value); }
        }

        private ChartValues<ObservableValue> material2Value;

        public ChartValues<ObservableValue> Material2Value
        {
            get { return material2Value; }
            set { OnPropertyChanged(ref material2Value, value); }
        }

        #endregion Materials properties

        #region Other properties

        private Brush economicImpactColour;
        public Brush EconomicImpactColour
        {
            get { return economicImpactColour; }
            set { OnPropertyChanged(ref economicImpactColour, value); }
        }

        private int environmentalSelectedTheme;
        public int EnvironmentalSelectedTheme
        {
            get { return environmentalSelectedTheme; }
            set { OnPropertyChanged(ref environmentalSelectedTheme, value); LoadEnvironmentalGraph(); }
        }

        private int economicsSelectedTheme;
        public int EconomicsSelectedTheme
        {
            get { return economicsSelectedTheme; }
            set { OnPropertyChanged(ref economicsSelectedTheme, value); LoadEconomicsGraph(); }
        }

        private int assetSelected;
        public int AssetSelected
        {
            get { return assetSelected; }
            set { OnPropertyChanged(ref assetSelected, value); LoadGraphs(); }
        }

        private SeriesCollection environmnetalSeriesCollection;
        public SeriesCollection EnvironmnetalSeriesCollection
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

        #endregion Other properties

        public ICommand GraphsCommand { get; set; }
        private CarbonResults carbonResults;
        private ReusableAsset currentAsset;
        private readonly List<string> loadedAssets;

        #region Methods

        public Graphs()
        {
            GraphsCommand = new RelayCommand(param => LoadGraphs()); // Setting the command to its method

            InitializeComponent();

            loadedAssets = SqliteDatabaseAccess.RetreiveAssetAndId();  // Retrieving all assets from the user's database

            // By default selects the first value from the dropdown menu and displays it when the graph tab is loaded
            currentAsset = SqliteDatabaseAccess.RetrieveAssets(loadedAssets[0]); // Sets the currentAsset to the first asset found in the database
            AssetSelected = 0; // Sets the selected asset from the dropdown menu as 0, to match the currentAsset

            LoadGraphs();  //Loading the graphs labels and settings
        }

        private void LoadGraphs()
        {
            currentAsset = SqliteDatabaseAccess.RetrieveAssets(loadedAssets[AssetSelected]);

            if (currentAsset != null) // Check if currentAsset is not null
            {
                // Using hte currentAsset information to perform the calculations necessary for the graphs
                carbonResults = CarbonCalculation.CalculateCarbon(currentAsset);

                // Loading settings, graphs and setting the graph's labels
                LoadGraphSettings();
                LoadEnvironmentalGraph();
                LoadEconomicsGraph();
                LoadGraphLabels();
            }
        }

        private void LoadGraphLabels()
        {
            //Labels and title for the Environmental Impact graph
            TitleLabelText = "Environmental Impact per " + currentAsset.SampleSize + " packagings";
            Labels = new[] { "Linear", "Circular" };
            Formatter = (x) => string.Format("{0:N2}", x) + " kg CO2e"; // Formatter sets the y axis label, displaying 2 decimals

            //Labels and title for the Economic Impact graph
            TitleLabelText2 = "Economic Impact per " + currentAsset.SampleSize + " packagings";
            Labels2 = new[] { "Linear", "Circular" };
            Formatter2 = (x) => string.Format("{0:N2}", x) + " £"; // Formatter sets the y axis label, displaying 2 decimals
            EconomicImpact = "Cost in £";
        }

        private void LoadGraphSettings()
        {
            // Settings
            LegendPosition = "Top"; // Position of the legend
            XRotationText = 0; // X axis text rotation
            YRotationText = 15; // Y axis text rotation
            SpeedValue = "00:00:0.5"; // Animation speed for the graphs
            XLabelText = "";
            YLabelText = "";
        }

        private void LoadEnvironmentalGraph()
        {
            LoadEnvironmentalTheme(); // Loads theme

            // Loading Material2 from the currentAsset. Material1 is used to
            // display the current material on the graph's legend
            Material1 = currentAsset.PrimaryMaterial;

            // Creating environmental graph
            EnvironmnetalSeriesCollection = new SeriesCollection {
                new StackedColumnSeries
                {
                    // Adding values for the linear and circular column
                    Values = new ChartValues<ObservableValue> {
                        new ObservableValue(carbonResults.Primary.LinearCarbon), // Primary material Linear environmnetal impact
                        new ObservableValue(carbonResults.Primary.CircularCarbon) // Primary material Circular environmental impact
                    },
                    PointGeometry=null,
                    Fill=Material1Colour,
                    Stroke=Material1Colour,
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
                EnvironmnetalSeriesCollection.Add(
                    new StackedColumnSeries
                    {
                        // Adding values for the linear and circular column
                        Values = new ChartValues<ObservableValue> {
                            new ObservableValue(carbonResults.Auxiliary.LinearCarbon), // Auxiliary material Linear environmnetal impact
                            new ObservableValue(carbonResults.Auxiliary.CircularCarbon) // Auxiliary material Circular environmental impact
                        },
                        PointGeometry = null,
                        Fill = Material2Colour,
                        Stroke = Material2Colour,
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

        private void LoadEnvironmentalTheme()
        {
            // Switches colours for the environmental graph's theme
            switch (EnvironmentalSelectedTheme)
            {
                case 0:
                    Material1Colour = (Brush)new BrushConverter().ConvertFromString("#4C3F54");
                    Material2Colour = (Brush)new BrushConverter().ConvertFromString("#E5D8ED");
                    break;

                case 1:
                    Material1Colour = (Brush)new BrushConverter().ConvertFromString("#A986C2");
                    Material2Colour = (Brush)new BrushConverter().ConvertFromString("#9BEBDD");
                    break;

                case 2:
                    Material1Colour = (Brush)new BrushConverter().ConvertFromString("#FFA275");
                    Material2Colour = (Brush)new BrushConverter().ConvertFromString("#FE4E61");
                    break;

                default:
                    Material1Colour = (Brush)new BrushConverter().ConvertFromString("#4C3F54");
                    Material2Colour = (Brush)new BrushConverter().ConvertFromString("#E5D8ED");
                    break;
            }
        }

        private void LoadEconomicsGraph()
        {
            LoadEconomicsTheme(); // Loads theme

            // Calculating the total of the economic impact: linear and circular
            var totalEconomicImpactLinear = currentAsset.UnitCost * currentAsset.SampleSize; // Linear economic impact
            var totalEconomicImpactCircular = totalEconomicImpactLinear / currentAsset.MaximumReuses; // Circular economic impact

            EconomicSeriesCollection = new SeriesCollection {
                new ColumnSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(totalEconomicImpactLinear),
                        new ObservableValue(totalEconomicImpactCircular)
                    },
                    PointGeometry = null,
                    Fill = EconomicImpactColour,
                    Stroke = EconomicImpactColour,
                    StrokeThickness = 8,
                    DataLabels = false,
                    FontSize = 13,
                    MaxColumnWidth = 100,
                    LabelsPosition = BarLabelPosition.Perpendicular,
                    Title = EconomicImpact
                }
            };
        }

        private void LoadEconomicsTheme()
        {
            // Switches colours for the economics graph's theme
            switch (EconomicsSelectedTheme)
            {
                case 0:
                    EconomicImpactColour = (Brush)new BrushConverter().ConvertFromString("#D5440B");
                    break;

                case 1:
                    EconomicImpactColour = (Brush)new BrushConverter().ConvertFromString("#42454B");
                    break;

                default:
                    EconomicImpactColour = (Brush)new BrushConverter().ConvertFromString("#D5440B");
                    break;
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
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion RoutedEventArgs

        #region Dropdown menus

        private void dropDown_themeEconomicImpact_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_themeEconomicImpact.SelectedIndex = 0; // By default, the selected item will be the one at index 0
            dropDown_themeEconomicImpact.Items.Add("Default");
            dropDown_themeEconomicImpact.Items.Add("Alternative");
        }

        private void dropDown_themeEnvironmentalImpact_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_themeEnvironmentalImpact.SelectedIndex = 0; // By default, the selected item will be the one at index 0
            dropDown_themeEnvironmentalImpact.Items.Add("Default");
            dropDown_themeEnvironmentalImpact.Items.Add("Alternative");
            dropDown_themeEnvironmentalImpact.Items.Add("Alternative 2");
        }

        private void dropDown_AssetSelection_Loaded(object sender, RoutedEventArgs e)
        {
            // Adds all the asset names from the loadedAssets array into the dropdown menu
            foreach (string asset in loadedAssets)
            {
                dropDown_assetSelection.Items.Add(asset);
            }

            dropDown_assetSelection.SelectedIndex = 0; // By default, the selected item will be the one at index 0
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

        //Added keybind so I can exit the application with ESC
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Environment.Exit(0);
            }
        }
    }
}