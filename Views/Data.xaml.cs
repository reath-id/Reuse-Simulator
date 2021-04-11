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

namespace ReathUIv0._3.Views
{
    /// <summary>
    /// Interaction logic for Data.xaml
    /// </summary>
    public partial class Data : Window
    {
        

        public Data()
        {
            InitializeComponent();
            textBlock_exportPath.Text = "Select Export Path";
        }

        private void comboBox_AssetSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_cancelExport_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_exportPath_Click(object sender, RoutedEventArgs e)
        {
            textBlock_exportPath.Text = "SET TO NEW PATH";
        }
    }
}
