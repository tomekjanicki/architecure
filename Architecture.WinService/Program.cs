using Architecture.Job.Helper;
using Architecture.Util.Ninject;
using Architecture.Util.WinService;

namespace Architecture.WinService
{
    static class Program
    {
        static void Main()
        {
            AppBootstrapper.ConfigureLog4Net();
            Factory.Init(AppBootstrapper.GetRegisteredKernel());
            Runner.Run<WorkerService, WorkerAppRunner>();
        }
    }

}
