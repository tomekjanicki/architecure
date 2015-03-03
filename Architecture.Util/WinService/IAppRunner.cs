using System;

namespace Architecture.Util.WinService
{
    public interface IAppRunner : IDisposable
    {
        void OnStart(string[] args);
        void OnStop();
    }
}