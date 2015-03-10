using System.Runtime.InteropServices;

namespace Architecture.Util
{
    internal class NativeMethods
    {
        [DllImport("kernel32")]
        public static extern bool AllocConsole();
        
    }
}