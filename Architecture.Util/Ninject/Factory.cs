using Ninject;

namespace Architecture.Util.Ninject
{
    public static class Factory
    {
        private static IKernel _kernel;

        public static void Init(IKernel kernel)
        {
            _kernel = kernel;
        }

        public static T Resolve<T>()
        {
            return _kernel.Get<T>();
        }
    }
}
