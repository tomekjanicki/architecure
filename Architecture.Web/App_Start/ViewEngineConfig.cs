using System.Web.Mvc;

namespace Architecture.Web
{
    public static class ViewEngineConfig
    {
        public static void RegisterViewEngine()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}