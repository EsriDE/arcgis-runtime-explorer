using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace EsriDe.RuntimeExplorer
{
    public static class ShowAboutCommand
    {
        public static readonly RoutedCommand Command = new RoutedCommand();

        static ShowAboutCommand()
        {
            Application.Current.MainWindow.CommandBindings.Add(new CommandBinding(Command, Execute, CanExecute));
        }

        private static void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static async void Execute(object sender, ExecutedRoutedEventArgs e)
        {
            //var menuItem = e.Parameter as HamburgerMenuItem;
            //await ((MainWindow)sender).ShowMessageAsync("", $"You clicked on Z button");


        }

        private async void ShowAwaitCustomDialog(object sender, RoutedEventArgs e)
        {
            EventHandler<DialogStateChangedEventArgs> dialogManagerOnDialogOpened = null;
            dialogManagerOnDialogOpened = (o, args) => {
                DialogManager.DialogOpened -= dialogManagerOnDialogOpened;
                Console.WriteLine("Custom Dialog opened!");
            };
            DialogManager.DialogOpened += dialogManagerOnDialogOpened;;

            var dialog = (BaseMetroDialog)this.Resources["CustomCloseDialogTest"];

            await this.ShowMetroDialogAsync(dialog);
            await dialog.WaitUntilUnloadedAsync();
        }
    }
}