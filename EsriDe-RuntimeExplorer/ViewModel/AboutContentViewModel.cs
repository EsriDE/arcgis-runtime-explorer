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
    }
}