using System;
using System.Web.Http;
using System.Web.Mvc;

namespace Architecture.Web.Test.Unit.Api.Base
{
    public static class Extension
    {
        public static IHttpActionResult CallWithModelValidation<TController, THttpActionResult, TModel>(this TController controller, Func<TController, THttpActionResult> action, TModel model)
            where TController : ApiController
            where THttpActionResult : IHttpActionResult
            where TModel : class
        {
            var provider = new DataAnnotationsModelValidatorProvider();
            var metadata = ModelMetadataProviders.Current.GetMetadataForProperties(model, typeof(TModel));
            foreach (var modelMetadata in metadata)
            {
                var validators = provider.GetValidators(modelMetadata, new ControllerContext());
                foreach (var validator in validators)
                {
                    var results = validator.Validate(model);
                    foreach (var result in results)
                        controller.ModelState.AddModelError(modelMetadata.PropertyName, result.Message);
                }
            }
            return action(controller);
        }
    }
}
