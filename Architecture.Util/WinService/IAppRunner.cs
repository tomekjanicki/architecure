namespace Architecture.Util.WinService
{
    public interface IAppRunner
    {
        void OnStart(string[] args);
        void OnStop();
    }
}