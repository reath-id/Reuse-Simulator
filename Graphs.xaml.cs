using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ReathUIv0._1
{
    /// <summary>
    /// Interaction logic for Graphs.xaml
    /// </summary>
    public partial class Graphs : Window
    {

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

        private double sampleSize;
        public double SampleSize
        {
            get { return sampleSize; }
            set { OnPropertyChanged(ref sampleSize, value); }
        }

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

        private string material3;
        public string Material3
        {
            get { return material3; }
            set { OnPropertyChanged(ref material3, value); }
        }

        private ChartValues<double> material1Value;
        public ChartValues<double> Material1Value
        {
            get { return material1Value; }
            set { OnPropertyChanged(ref material1Value, value); }
        }

        private ChartValues<double> material2Value;
        public ChartValues<double> Material2Value
        {
            get { return material2Value; }
            set { OnPropertyChanged(ref material2Value, value); }
        }

        private ChartValues<double> material3Value;
        public ChartValues<double> Material3Value
        {
            get { return material3Value; }
            set { OnPropertyChanged(ref material3Value, value); }
        }

        private ChartValues<double> economicImpactValue;
        public ChartValues<double> EconomicImpactValue
        {
            get { return economicImpactValue; }
            set { OnPropertyChanged(ref economicImpactValue, value); }
        }
        public ICommand GraphsCommand { get; set; }

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

        public Graphs()
        {
            InitializeComponent();
            GraphsCommand = new RelayCommand(param => LoadGraphs()) ;
            LegendPosition = "Top";
            XRotationText = 0;
            YRotationText = 15;
            SpeedValue = "00:00:0.5";
            TitleLabelText2 = "Economic Impact";
            XLabelText = "";
            YLabelText = "";
            SampleSize = 100;

            TitleLabelText = "Environmental Impact";
            Labels = new[] { "Linear", "Circular" };
            Formatter = (x) => string.Format("{0:N2}", x) + " kg CO2e";
            Material1Value = new ChartValues<double>();
            Material2Value = new ChartValues<double>();
            Material3Value = new ChartValues<double>();
            Material1 = "Aluminium";
            Material2 = "Plastic";
            Material3 = "Cardboard";

            TitleLabelText2 = "Economic Impact per " + SampleSize + " packagings";
            Labels2 = new[] { "Linear", "Circular" };
            EconomicImpact = "Cost in £";
            Formatter2 = (x) => string.Format("{0:N2}", x) + " £";
            EconomicImpactValue = new ChartValues<double>();

           
        }

        void LoadGraphs()
        {
            LoadEnvironmentalGraph();
            LoadEconomicsGraph();
        }
        void LoadEnvironmentalGraph()
        {
            // Adding mock-up Values
            Material1Value.Clear();
            Material2Value.Clear();
            Material3Value.Clear();

            Material1Value.Add(15);
            Material1Value.Add(6);
            Material2Value.Add(26);
            Material2Value.Add(12);
            Material3Value.Add(43);
            Material3Value.Add(1);
        }

        void LoadEconomicsGraph()
        {
            // Adding mock-up Values
            EconomicImpactValue.Clear();
            EconomicImpactValue.Add(26);
            EconomicImpactValue.Add(5);
        }
        private void btnInput_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        //Added keybind so I can exit the application with ESC
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Environment.Exit(0);
            }
        }

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

        #endregion // INotifyPropertyChanged Members
    }
}

