using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.WinService
{
    [RunInstaller(true)]
    public partial class WorkerServiceProjectInstaller : System.Configuration.Install.Installer
    {
        public WorkerServiceProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
