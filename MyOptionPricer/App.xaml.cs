using System.Configuration;
using System.Data;
using System.Windows;
using System.Threading;

namespace MyOptionPricer;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    //Gestion des erreur en les affichant dans une fenetre personalise 

    private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        CustomMessageBox customMessageBox = new($"Il ya une erreur !!! 🍍: \n {e.Exception.Message}", "Erreur General");
        customMessageBox.ShowDialog();

        e.Handled = true;
    }

    //gestion des instance de l'app

    private const string MutexName = "My Option Pricer";
    private static Mutex mutex;

    protected override void OnStartup(StartupEventArgs e)
    {
        bool createdNew;
        mutex = new Mutex(true, MutexName, out createdNew);

        if (!createdNew)
        {
            MessageBox.Show("Une instance de l'application est déjà en cours d'exécution.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
            return;
        }

        base.OnStartup(e);
    }
    protected override void OnExit(ExitEventArgs e)
    {
        if (mutex != null)
        {
            mutex.ReleaseMutex();
            mutex.Dispose();
        }

        base.OnExit(e);
    }


}



