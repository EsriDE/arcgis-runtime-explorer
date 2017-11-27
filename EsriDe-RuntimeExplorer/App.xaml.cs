using System.IO;
using System.Windows;
using EsriDe.RuntimeExplorer.ViewModel;
using Microsoft.Practices.ServiceLocation;

namespace EsriDe.RuntimeExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 1) //make sure an argument is passed
            {
                ViewModelLocator.Initialize();
                var model = ServiceLocator.Current?.GetInstance<MainViewModel>();
                if (model != null)
                {
                    model.FilePath = e.Args[0];
                }
            }
        }
    }
}
