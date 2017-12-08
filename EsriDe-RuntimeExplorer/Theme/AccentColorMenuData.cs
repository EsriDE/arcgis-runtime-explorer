using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using EsriDe.RuntimeExplorer.Properties;
using MahApps.Metro;

namespace EsriDe.RuntimeExplorer.Theme
{
    public class AccentColorMenuData
    {
        public string Name { get; set; }
        public Brush BorderColorBrush { get; set; }
        public Brush ColorBrush { get; set; }

        private ICommand _changeAccentCommand;

        public ICommand ChangeAccentCommand
        {
            get { return this._changeAccentCommand ?? (_changeAccentCommand = new SimpleCommand { CanExecuteDelegate = x => true, ExecuteDelegate = x => this.DoChangeTheme(x) }); }
        }

        protected virtual void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);

            Settings.Default.UserAccent = accent.Name;
            Settings.Default.Save();
        }
    }

}