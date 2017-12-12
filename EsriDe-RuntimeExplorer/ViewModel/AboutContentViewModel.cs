using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class AboutContentViewModel : ViewModelBase
    {
        public ICommand OpenWebsiteCommand
        {
            get
            {
                if (_openWebsiteCommand == null)
                {
                    _openWebsiteCommand = new RelayCommand<object>(OpenWebsite);
                }

                return _openWebsiteCommand;
            }
        }


        private RelayCommand<object> _openWebsiteCommand;

        private void OpenWebsite(object url)
        {
            System.Diagnostics.Process.Start(url as string);
        }
    }
}