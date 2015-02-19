using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Results;
using Architecture.Business.Exception;
using Architecture.Util.Ninject;
using Architecture.ViewModel;
using Architecture.Web.Controllers.Api;
using Architecture.Web.Test.Unit.Api.Base;
using NSubstitute;
using NUnit.Framework;

namespace Architecture.Web.Test.Unit.Api
{
    public class OrderControllerTest : BaseControllerTest
    {

        [Test]
        public void PostOrder_ValidArguments_ReturnsCreatedContentResult()
        {
            const int id = 1;
            var controller = SetupControllerForTest(HttpMethod.Post, "order");
            var insertOrder = new InsertOrder();
            controller.BusinessLogicFacade.OrderManager.InsertOrder(Arg.Is(insertOrder)).Returns(new Tuple<int?, Dictionary<string, IList<string>>>(1, new Dictionary<string, IList<string>>()));
            var expectedUrl = string.Format("{0}/{1}", controller.Request.RequestUri.AbsoluteUri, id);

            var result = Call<CreatedNegotiatedContentResult<InsertOrder>>(() => controller.PostOrder(insertOrder));

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUrl, result.Location.AbsoluteUri);
        }

        [Test]
        public void PostOrder_InvalidArguments_ReturnsInvalidModelStateResult()
        {
            var controller = SetupControllerForTest(HttpMethod.Post, "order");
            var insertOrder = new InsertOrder();
            controller.BusinessLogicFacade.OrderManager.InsertOrder(Arg.Is(insertOrder)).Returns(new Tuple<int?, Dictionary<string, IList<string>>>(null, new Dictionary<string, IList<string>> { { "", new[] { "" } } }));

            var result = Call<InvalidModelStateResult>(() => controller.PostOrder(insertOrder));

            Assert.IsNotNull(result);
        }

        [Test]
        public void PutOrder_ValidArguments_ReturnsOkResult()
        {
            var controller = SetupControllerForTest(HttpMethod.Put, "order");
            var updateOrder = new UpdateOrder();
            controller.BusinessLogicFacade.OrderManager.UpdateOrder(Arg.Is(updateOrder)).Returns(new Dictionary<string, IList<string>>());

            var result = Call<OkResult>(() => controller.PutOrder(updateOrder));
                
            Assert.IsNotNull(result);
            controller.BusinessLogicFacade.OrderManager.Received(1).UpdateOrder(Arg.Is(updateOrder));
        }

        [Test]
        public void PutOrder_InvalidArguments_ReturnsNotFoundResult()
        {
            var controller = SetupControllerForTest(HttpMethod.Put, "order");
            var updateOrder = new UpdateOrder();
            controller.BusinessLogicFacade.OrderManager.UpdateOrder(Arg.Is(updateOrder)).Returns(info => { throw new ObjectNotFoundException("", typeof(UpdateOrder)); });

            var result = Call<NotFoundResult>(() => controller.PutOrder(updateOrder));

            Assert.IsNotNull(result);
        }

        private OrderController SetupControllerForTest(HttpMethod method, string key)
        {
            return GetConfiguredWebApiController(Factory.Resolve<OrderController>, method, key);
        }

    }
}
