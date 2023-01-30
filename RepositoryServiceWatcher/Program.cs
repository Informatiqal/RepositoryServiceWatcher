using System.ServiceProcess;

namespace RepositoryServiceWatcher
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new RepositoryServiceWatcher()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
