using System.Linq;
using System.Web.Mvc;

namespace Architecture.Web
{
    public class ModelValidatorProvidersConfig
    {
        public static void Configure(ModelValidatorProviderCollection providers)
        {
            var ps = providers.Where(validatorProvider => validatorProvider is DataAnnotationsModelValidatorProvider).ToList();
            foreach (var modelValidatorProvider in ps)
                providers.RemoveAt(providers.IndexOf(modelValidatorProvider));
        }

    }
}