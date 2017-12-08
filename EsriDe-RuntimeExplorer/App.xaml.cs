using System.Windows;
using EsriDe.RuntimeExplorer.Properties;
using Microsoft.Practices.ServiceLocation;
using EsriDe.RuntimeExplorer.ViewModel;
using MahApps.Metro;

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
            LoadUserLayout();

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

        private static void LoadUserLayout()
        {
            var theme = Settings.Default.UserTheme;
            var accent = Settings.Default.UserAccent;

            if (!string.IsNullOrEmpty(theme) || !string.IsNullOrEmpty(accent))
            {
                // get the current app style (theme and accent) from the application
                // you can then use the current theme and custom accent instead set a new theme
                var currentStyle = ThemeManager.DetectAppStyle(Current);

                // now set the Green accent and dark theme
                ThemeManager.ChangeAppStyle(Current,
                    string.IsNullOrEmpty(accent) ? currentStyle.Item2 : ThemeManager.GetAccent(accent),
                    string.IsNullOrEmpty(theme) ? currentStyle.Item1 : ThemeManager.GetAppTheme(theme));
            }
        }
    }
}
