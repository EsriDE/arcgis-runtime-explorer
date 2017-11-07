using System.Diagnostics;
using System.Windows.Input;
using Esri.ArcGISRuntime.Mapping;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(MapViewModel mapViewModel)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            FileOpenCommand = new RelayCommand(() =>
            {
                Debug.WriteLine("File open");
                var dlg = new OpenFileDialog();
                dlg.Filter =
                    "Mobile Map Packages (*.mmpk)|*.mmpk|Runtime Geodatabases (*.geodatabase)|*.geodatabase|All Files (*.*)|*.*";
                if (dlg.ShowDialog() == true)
                {
                    FilePath = dlg.FileName;
                    //var mmpk = await MobileMapPackage.OpenAsync(FilePath);
                    //mapViewModel.Map = mmpk.Maps[0];
                }
            });
            PropertyChanged += async (sender, args) =>
            {
                if (args.PropertyName == nameof(FilePath))
                {
                    var mmpk = await MobileMapPackage.OpenAsync(FilePath);
                    mapViewModel.Map = mmpk.Maps[0];
                }
            };

        }

        public ICommand FileOpenCommand { get; private set; }

        private string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set { Set(ref _filePath, value); }
        }
    }
}