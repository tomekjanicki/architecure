using System.Collections.Generic;
using System.Web.Http;
using Architecture.Business.Facade.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.Web.Code.Attribute;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Controllers.Api
{

    /// <summary>
    /// Provides product related actions
    /// </summary>
    public class ProductController : BaseApiController
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="businessLogicFacade"></param>
        public ProductController(IBusinessLogicFacade businessLogicFacade)
            : base(businessLogicFacade)
        {
        }

        /// <summary>
        /// Provides paged product list
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="pageSize"></param>
        /// <param name="skip"></param>
        /// <param name="sort">Sort expression in the format "field | [asc|desc]". Avaliable fields: id, code, name, price, date. When null or empty default sort is applied.</param>
        /// <returns></returns>
        [HttpGet]
        public Paged<FindProducts> FindProductsPaged(string code, string name, int pageSize, int skip, string sort)
        {
            return BusinessLogicFacade.ProductManager.FindProducts(code, name, new PageAndSortCriteria(pageSize, skip, sort));
        }

        /// <summary>
        /// Provide product list
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<FindProducts> FindProducts(string code, string name, string sort)
        {
            return BusinessLogicFacade.ProductManager.FindProducts(code, name, sort);
        }

        /// <summary>
        /// Deletes product
        /// </summary>
        /// <param name="deleteProduct"></param>
        /// <returns></returns>
        [ValidateWebApiAntiForgeryToken]
        public IHttpActionResult DeleteProduct(DeleteProduct deleteProduct)
        {
            return HandlePutOrDelete(() => BusinessLogicFacade.ProductManager.DeleteProduct(deleteProduct));
        }


    }
}
