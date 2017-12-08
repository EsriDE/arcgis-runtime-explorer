using System.Windows;
using EsriDe.RuntimeExplorer.Properties;
using MahApps.Metro;

namespace EsriDe.RuntimeExplorer.Theme
{
    public class AppThemeMenuData : AccentColorMenuData
    {
        protected override void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);

            Settings.Default.UserTheme = appTheme.Name;
            Settings.Default.Save();
        }
    }

}