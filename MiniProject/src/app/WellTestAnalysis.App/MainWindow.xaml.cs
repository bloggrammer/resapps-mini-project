using System.Windows;
using System.Windows.Threading;
using WellTestAnalysis.App.Data;
using WellTestAnalysis.Services;
using WellTestAnalysis.Utils;

namespace WellTestAnalysis.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var sessionFactory = new SessionFactory(IOService.ConnectionString);
            repo = new Repository(sessionFactory.GetSession());
            viewModel = new MainViewModel();
            service = new WellTestAnalysisService();
            viewModel.Initialize(service, repo);
            DataContext = viewModel;
            GlobalExceptionHandler();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        #region Exception Handling
        public void GlobalExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
            //Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
            System.Windows.Application.Current.Dispatcher.UnhandledExceptionFilter += OnFilterDispatcherException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        /// <summary>
        /// This methods gets invoked for every unhandled excption
        /// that is raise on the application Dispatcher, the AppDomain
        /// or by the GC cleaning up a faulted Task.
        /// </summary>
        /// <param name="e">The unhandled exception</param>
        public void OnUnhandledException(Exception e, string ex)
        {

            IOService.LogError($"{ex} => {e.Message} Inner: {e.InnerException?.Message}");
            IOService.LogError($"stack trace: {e.StackTrace}");
        }

        /// <summary>
        /// Override this method to decide if the <see cref="OnUnhandledException(Exception)"/>
        /// method should be called for the passes Dispatcher exception.
        /// </summary>
        /// <param name="exception">The unhandled excpetion on the applications dispatcher.</param>
        /// <returns>True if the <see cref="OnUnhandledException(Exception)"/> method should
        /// be called. False if not</returns>
        protected virtual void CatchDispatcherException(Exception e)
        {
            IOService.LogError($"{e.Message} Inner: {e.InnerException?.Message}");
            IOService.LogError($"stack trace: {e.StackTrace}");
        }




        /// <summary>
        /// This method is invoked whenever there is an unhandled
        /// exception on a delegate that was posted to be executed
        /// on the UI-thread (Dispatcher) of a WPF application.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            OnUnhandledException(e.Exception, "DispatcherUnhandledExceptionEventArgs");
            e.Handled = true;
        }

        /// <summary>
        /// This event is invoked whenever there is an unhandled
        /// exception in the default AppDomain. It is invoked for
        /// exceptions on any thread that was created on the AppDomain. 
        /// </summary>
        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            IOService.LogError($"Unhandled exception on current AppDomain  {e.IsTerminating}");
            MessageBox.Show("A UnhandledException has occurred. Shutting down the application. Please check the log file for more details.");
            Environment.Exit(0);
        }


        /// <summary>
        /// This method is called when a faulted task, which has the
        /// exception object set, gets collected by the GC. This is useful
        /// to track Exceptions in asnync methods where the caller forgets
        /// to await the returning task
        /// </summary>
        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            OnUnhandledException(e.Exception, "UnobservedTaskExceptionEventArgs");
            e.SetObserved();
            MessageBox.Show("A UnobservedTaskException has occurred. Shutting down the application. Please check the log file for more details.");
            Environment.Exit(0);
        }

        /// <summary>
        /// The method gets called for any unhandled exception on the
        /// Dispatcher. When e.RequestCatch is set to true, the exception
        /// is catched by the Dispatcher and the DispatcherUnhandledException
        /// event will be invoked.
        /// </summary>
        private void OnFilterDispatcherException(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
        {
            OnUnhandledException(e.Exception, "DispatcherUnhandledExceptionFilterEventArgs");
            MessageBox.Show("A DispatcherUnhandledExceptionFilter has occurred. Shutting down the application. Please check the log file for more details.");
            Environment.Exit(0);
        }

        #endregion

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel = DataIO.ReadXML<MainViewModel>(IOService.GetOpenPath());
                viewModel.Initialize(service, repo);
                viewModel.Replot();
                DataContext = viewModel;
                MessageBox.Show("Data imported successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {

                MessageBox.Show("Something went wrong", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataIO.WriteToXML(IOService.GetWTAPath(), viewModel);

                MessageBox.Show("Data exported successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {

                MessageBox.Show("Something went wrong", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void New_Click(object sender, RoutedEventArgs e)
        {
            viewModel = new MainViewModel();
            viewModel.Initialize(service, repo);
            DataContext = viewModel;
        }
        private MainViewModel viewModel;
        private IWellTestAnalysisService service;
        private IRepository repo;

       
    }
}