using Architecture.Job.Helper;
using Architecture.Util.Ninject;
using Architecture.Util.WinService;

namespace Architecture.WinService
{
    static class Program
    {
        static void Main()
        {
            Factory.Init(Registration.GetRegisteredKernel());
            Runner.Run<WorkerService, WorkerAppRunner>();
        }
    }

}
