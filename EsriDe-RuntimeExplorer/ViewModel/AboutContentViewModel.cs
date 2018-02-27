using System;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class AboutContentViewModel : ViewModelBase
    {
        private RelayCommand<object> _openWebsiteCommand;

        public ICommand OpenWebsiteCommand
        {
            get
            {
                return _openWebsiteCommand ?? (_openWebsiteCommand =
                           new RelayCommand<object>(url => { System.Diagnostics.Process.Start(url as string); }));
            }
        }

        private string _arcGISRuntimeVersion;

        public string ArcGISRuntimeVersion
        {
            get => _arcGISRuntimeVersion;
            set => Set(ref _arcGISRuntimeVersion, value);
        }

        public AboutContentViewModel()
        {
            ArcGISRuntimeVersion = GetArcGISRuntimeVersion();
        }

        private string GetArcGISRuntimeVersion()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var runtimeAssembly = assemblies.Single(_ => _.GetName().Name.Equals(ArcGISRuntimeAssemblyIdentifier));
            return runtimeAssembly.GetName().Version.ToString();
        }

        public const string ArcGISRuntimeAssemblyIdentifier = "Esri.ArcGISRuntime";
    }
}