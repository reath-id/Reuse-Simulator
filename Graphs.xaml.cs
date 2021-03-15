using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ReathUIv0._1
{
    /// <summary>
    /// Interaction logic for Graphs.xaml
    /// </summary>
    public partial class Graphs : Window, INotifyPropertyChanged
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

        private Brush material3Colour;
        public Brush Material3Colour
        {
            get { return material3Colour; }
            set { OnPropertyChanged(ref material3Colour, value); }
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
            set { OnPropertyChanged(ref environmentalSelectedTheme, value); LoadEnvironmentalTheme(); }
        }        
        
        private int economicsSelectedTheme;
        public int EconomicsSelectedTheme
        {
            get { return economicsSelectedTheme; }
            set { OnPropertyChanged(ref economicsSelectedTheme, value); LoadEconomicsTheme(); }
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
            Random rnd = new Random();

            // Adding mock-up Values
            Material1Value.Clear();
            Material2Value.Clear();
            Material3Value.Clear();

            Material1Value.Add(rnd.Next(22, 54));
            Material1Value.Add(rnd.Next(9, 18));
            Material2Value.Add(rnd.Next(9, 30));
            Material2Value.Add(rnd.Next(1, 8));
            Material3Value.Add(rnd.Next(13, 22));
            Material3Value.Add(rnd.Next(2, 7));
        }

        void LoadEnvironmentalTheme()
        {
            switch (EnvironmentalSelectedTheme)
            {
                case 1:
                    Material1Colour = (Brush)new BrushConverter().ConvertFromString("#4C3F54");
                    Material2Colour = (Brush)new BrushConverter().ConvertFromString("#E5D8ED");
                    Material3Colour = (Brush)new BrushConverter().ConvertFromString("#9BEBDD");
                    break;
                case 2:
                    Material1Colour = (Brush)new BrushConverter().ConvertFromString("#A986C2");
                    Material2Colour = (Brush)new BrushConverter().ConvertFromString("#FFA275");
                    Material3Colour = (Brush)new BrushConverter().ConvertFromString("#FE4E61");
                    break;
            }
        }

        void LoadEconomicsGraph()
        {
            Random rnd = new Random();

            // Adding mock-up Values
            EconomicImpactValue.Clear();
            EconomicImpactValue.Add(rnd.Next(18, 66));
            EconomicImpactValue.Add(rnd.Next(2, 6));
        }

        void LoadEconomicsTheme()
        {
            switch (EconomicsSelectedTheme)
            {
                case 1:
                    EconomicImpactColour = (Brush)new BrushConverter().ConvertFromString("#D5440B");
                    break;
                case 2:
                    EconomicImpactColour = (Brush)new BrushConverter().ConvertFromString("#42454B");
                    break;

            }
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


        private void dropDown_themeEconomicImpact_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_themeEconomicImpact.Items.Add("Theme for Economic Impact");
            dropDown_themeEconomicImpact.SelectedIndex = 1;
            dropDown_themeEconomicImpact.Items.Add("Default");
            dropDown_themeEconomicImpact.Items.Add("Alternative"); 
        }

        private void dropDown_themeEnvironmentalImpact_Loaded(object sender, RoutedEventArgs e)
        {
            dropDown_themeEnvironmentalImpact.Items.Add("Theme for Environmnetal Impact");
            dropDown_themeEnvironmentalImpact.SelectedIndex = 1;
            dropDown_themeEnvironmentalImpact.Items.Add("Default");
            dropDown_themeEnvironmentalImpact.Items.Add("Alternative");       
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

