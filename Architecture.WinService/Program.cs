using System;
using System.ServiceProcess;
using Architecture.Util.Ninject;

namespace Architecture.WinService
{
    static class Program
    {
        static void Main()
        {
            Factory.Init(Registration.GetRegisteredKernel());
            // runs the app as a  console application if the command argument "-console" is used
            if (WindowsServiceHelper.RunAsConsoleIfRequested<WorkerService>())
                return;
            // uses "-install" and "-uninstall" to manage the service.
            if (WindowsServiceHelper.ManageServiceIfRequested(Environment.GetCommandLineArgs()))
                return;
            var servicesToRun = new ServiceBase[] { new WorkerService() };
            ServiceBase.Run(servicesToRun);
        }
    }

}
