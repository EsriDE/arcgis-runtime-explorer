using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace EsriDe.RuntimeExplorer
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : BaseMetroDialog
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        protected AboutDialog(MetroWindow owningWindow, MetroDialogSettings settings) : base(owningWindow, settings)
        {
        }
    }
}
