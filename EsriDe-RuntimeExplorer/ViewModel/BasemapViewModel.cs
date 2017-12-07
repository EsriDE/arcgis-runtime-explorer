using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class BasemapViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand ApplyBasemapCommand { get; private set; }
    }
}