using System.ComponentModel;
using System.Configuration.Install;

namespace Architecture.WinService
{
    [RunInstaller(true)]
    public partial class WorkerServiceProjectInstaller : Installer
    {
        public WorkerServiceProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
