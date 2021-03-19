using System.Windows;

namespace ReathUIv0._1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Window window = new Graphs();
            Graphs context = new Graphs();
            window.DataContext = context;
            window.Show();
        }
    }
}