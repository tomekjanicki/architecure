using System.Collections.Generic;
using System.Web.Http;
using Architecture.Business.Facade.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Controllers.Api
{
    public class ProductController : BaseApiController
    {
        public ProductController(IBusinessLogicFacade businessLogicFacade)
            : base(businessLogicFacade)
        {
        }

        public Paged<FindProducts> GetProducts(string code, string name, int pageSize, int skip, string sort)
        {            
            return BusinessLogicFacade.ProductManager.FindProducts(code, name, new PageAndSortCriteria(pageSize, skip, sort));
        }

        public IEnumerable<FindProducts> GetProducts(string code, string name, string sort)
        {
            return BusinessLogicFacade.ProductManager.FindProducts(code, name, sort);
        }

        public IHttpActionResult DeleteProduct(DeleteProduct deleteProduct)
        {
            return HandlePutOrDelete(() => BusinessLogicFacade.ProductManager.DeleteProduct(deleteProduct));
        }


    }
}
