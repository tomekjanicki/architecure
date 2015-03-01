using System;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace Architecture.Util
{
    public static class WindowsServiceHelper
    {
        private static readonly string[] InstallArguments = { "/i", "/install", "-i", "-install" };
        private static readonly string[] UninstallArguments = { "/u", "/uninstall", "-u", "-uninstall" };

        private static void AttachConsole()
        {
            AllocConsole();
        }

        public static bool ManageServiceIfRequested(string[] arguments)
        {
            try
            {
                if (!arguments.Any(x => InstallArguments.Contains(x)) && !arguments.Any(x => UninstallArguments.Contains(x)))
                    return false;
                AttachConsole();
                var serviceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InstallUtil.InstallLog");
                if (File.Exists(serviceFile))
                    File.Delete(serviceFile);
                if (arguments.Any(x => InstallArguments.Contains(x)))
                    ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
                else if (arguments.Any(x => UninstallArguments.Contains(x)))
                    ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetExecutingAssembly().Location });
                else
                    return false;
            }
            catch (System.Exception exception)
            {
                Console.WriteLine("Error: {0}", exception.Message);
            }
            Console.WriteLine("Service configuration is done. Press any key to exit console");
            Console.ReadKey();
            return true;
        }

        public static bool RunAsConsoleIfRequested<T>() where T : ServiceBase, new()
        {
            if (!Environment.CommandLine.Contains("-console"))
                return false;
            AttachConsole();
            var service = new T();
            var onstart = service.GetType().GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            var args = Environment.GetCommandLineArgs().Where(name => name != "-console").ToArray();
            onstart.Invoke(service, new object[] { args });
            Console.WriteLine("Your service named '{0}' is up and running.\r\nPress 'ENTER' to stop it.", service.GetType().FullName);
            Console.ReadLine();
            var onstop = service.GetType().GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            onstop.Invoke(service, null);
            Console.WriteLine("Service is stopped. Press any key to exit console");
            Console.ReadKey();
            return true;
        }

        [DllImport("kernel32")]
        private static extern bool AllocConsole();
    }
}