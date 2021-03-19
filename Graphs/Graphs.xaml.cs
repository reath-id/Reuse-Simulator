using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ReathUIv0._1
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

        #region Mats properties

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

        #endregion Mats properties

        #region other properties

        private ChartValues<double> economicImpactValue;

        public ChartValues<double> EconomicImpactValue
        {
            get { return economicImpactValue; }
            set { OnPropertyChanged(ref economicImpactValue, value); }
        }

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

        private SeriesCollection environmentalSeriesCollection;

        public SeriesCollection EnvironmentalSeriesCollection
        {
            get { return environmentalSeriesCollection; }
            set { OnPropertyChanged(ref environmentalSeriesCollection, value); }
        }

        private SeriesCollection economicSeriesCollection;

        public SeriesCollection EconomicSeriesCollection
        {
            get { return economicSeriesCollection; }
            set { OnPropertyChanged(ref economicSeriesCollection, value); }
        }

        #endregion other properties

        public ICommand GraphsCommand { get; set; }
        private CarbonResults carbonResults;
        private ReusableAsset[] reusableAssets;
        private ReusableAsset currentAsset;
        private double totalEconomicImpactLinear;
        private double totalEconomicImpactCircular;

        public Graphs()
        {
            GraphsCommand = new RelayCommand(param => LoadGraphs());

            InitializeComponent();

            CarbonCalculation.SetDB(new MockDB());

            List<string> exampleassets = File.ReadLines("exampleassets.csv").Skip(1).ToList();
            reusableAssets = new ReusableAsset[exampleassets.Count];

            int index = 0;
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
                    reusableAsset.AuxiliaryMaterial = asset_values[5];
                    reusableAsset.AuxiliaryWeight = float.Parse(asset_values[6]);
                    reusableAsset.AuxiliaryMaterialManufacturing = ReusableAsset.StringToManufacturingMethod(asset_values[7]);
                    reusableAsset.AuxiliaryDisposalMethod = ReusableAsset.StringToDisposalMethod(asset_values[9]);
                }

                reusableAsset.MaximumReuses = int.Parse(asset_values[10]);
                reusableAsset.AvgDistanceToRecycle = float.Parse(asset_values[11]);
                reusableAsset.PrepForReuseCarbonFactor = float.Parse(asset_values[12]);

                reusableAssets[index++] = reusableAsset;
            }

            assetSelected = 1;

            //Loading the graphs labels and settings
            LoadGraphSettings();
            LoadGraphs();
        }

        private void LoadGraphs()
        {
            currentAsset = reusableAssets[assetSelected];
            carbonResults = CarbonCalculation.CalculateCarbon(currentAsset);

            LoadMockValues();
            LoadEnvironmentalGraph();
            LoadEconomicsGraph();
            LoadGraphLabels();
        }

        private void LoadMockValues()
        {
            //Create random values for the input of primary and Auxiliary material cost
            Random rdn = new Random();

            float UnitCost = (float)rdn.Next(12, 120) / 100;
            totalEconomicImpactLinear = UnitCost * currentAsset.NoOfItems;
            totalEconomicImpactCircular = totalEconomicImpactLinear / currentAsset.MaximumReuses;
        }
        private void LoadGraphLabels()
        {
            //Labels and title for the Environmental Impact graph
            TitleLabelText = "Environmental Impact per " + currentAsset.NoOfItems + " packagings";
            Labels = new[] { "Linear", "Circular" };
            Formatter = (x) => string.Format("{0:N2}", x) + " kg CO2e"; // Formatter sets the y axis label. String  display 2 decimals

            //Labels and title for the Economic Impact graph
            TitleLabelText2 = "Economic Impact per " + currentAsset.NoOfItems + " packagings";
            Labels2 = new[] { "Linear", "Circular" };
            Formatter2 = (x) => string.Format("{0:N2}", x) + " £"; // Formatter sets the y axis label. String  display 2 decimals
            EconomicImpact = "Cost in £";
        }
        private void LoadGraphSettings()
        {
            LegendPosition = "Top";
            XRotationText = 0;
            YRotationText = 15;
            SpeedValue = "00:00:0.5";
            XLabelText = "";
            YLabelText = "";
        }
        private void LoadEnvironmentalGraph()
        {
            LoadEnvironmentalTheme();

            // Adding mock-up Values
            Material1 = currentAsset.PrimaryMaterial;

            EnvironmentalSeriesCollection = new SeriesCollection {
                new StackedColumnSeries
                {
                    Values = new ChartValues<ObservableValue> {
                        new ObservableValue(carbonResults.PrimaryMaterialLinearCarbon),
                        new ObservableValue(carbonResults.PrimaryMaterialCircularCarbon)
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


            if (!String.IsNullOrWhiteSpace(currentAsset.AuxiliaryMaterial))
            {
                Material2 = currentAsset.AuxiliaryMaterial;

                EnvironmentalSeriesCollection.Add(
                    new StackedColumnSeries
                    {
                        Values = new ChartValues<ObservableValue> {
                            new ObservableValue(carbonResults.AuxiliaryMaterialLinearCarbon),
                            new ObservableValue(carbonResults.AuxiliaryMaterialCircularCarbon)
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
            LoadEconomicsTheme();

            // Adding mock-up Values
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

        #region RoutedEventArgs
        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
        }
        private void btnData_Click(object sender, RoutedEventArgs e)
        {
        }
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion RoutedEventArgs

        #region dropdown menus
        private void dropDown_themeEconomicImpact_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_themeEconomicImpact.SelectedIndex = 0;
            dropDown_themeEconomicImpact.Items.Add("Default");
            dropDown_themeEconomicImpact.Items.Add("Alternative");
        }
        private void dropDown_themeEnvironmentalImpact_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_themeEnvironmentalImpact.SelectedIndex = 0;
            dropDown_themeEnvironmentalImpact.Items.Add("Default");
            dropDown_themeEnvironmentalImpact.Items.Add("Alternative");
            dropDown_themeEnvironmentalImpact.Items.Add("Alternative 2");
        }
        private void dropDown_AssetSelectiont_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < reusableAssets.Length; i++)
            {
                dropDown_assetSelection.Items.Add(reusableAssets[i].AssetName);
            }

            dropDown_assetSelection.SelectedIndex = 0;
        }

        #endregion dropdown menus

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