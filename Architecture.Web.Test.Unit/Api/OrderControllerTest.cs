using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Results;
using Architecture.Business.Exception;
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
            using (var configurator = new ControllerConfigurator<OrderController>(HttpMethod.Post, "order"))
            {
                const int id = 1;
                var controller = configurator.GetConfigured();
                var insertOrder = new InsertOrder();
                controller.BusinessLogicFacade.OrderManager.InsertOrder(Arg.Is(insertOrder)).Returns(new Tuple<int?, Dictionary<string, IList<string>>>(1, new Dictionary<string, IList<string>>()));
                var expectedUrl = string.Format("{0}/{1}", controller.Request.RequestUri.AbsoluteUri, id);

                var result = Call<CreatedNegotiatedContentResult<InsertOrder>>(() => controller.PostOrder(insertOrder));

                Assert.IsNotNull(result);
                Assert.AreEqual(expectedUrl, result.Location.AbsoluteUri);
            }
        }

        [Test]
        public void PostOrder_InvalidArguments_ReturnsInvalidModelStateResult()
        {
            using (var configurator = new ControllerConfigurator<OrderController>(HttpMethod.Post, "order"))
            {
                var controller = configurator.GetConfigured();
                var insertOrder = new InsertOrder();
                controller.BusinessLogicFacade.OrderManager.InsertOrder(Arg.Is(insertOrder)).Returns(new Tuple<int?, Dictionary<string, IList<string>>>(null, new Dictionary<string, IList<string>> { { "", new[] { "" } } }));

                var result = Call<InvalidModelStateResult>(() => controller.PostOrder(insertOrder));

                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void PutOrder_ValidArguments_ReturnsOkResult()
        {
            using (var configurator = new ControllerConfigurator<OrderController>(HttpMethod.Put, "order"))
            {
                var controller = configurator.GetConfigured();
                var updateOrder = new UpdateOrder();
                controller.BusinessLogicFacade.OrderManager.UpdateOrder(Arg.Is(updateOrder)).Returns(new Dictionary<string, IList<string>>());

                var result = Call<OkResult>(() => controller.PutOrder(updateOrder));

                Assert.IsNotNull(result);
                controller.BusinessLogicFacade.OrderManager.Received(1).UpdateOrder(Arg.Is(updateOrder));                
            }
        }

        [Test]
        public void PutOrder_InvalidArguments_ReturnsNotFoundResult()
        {
            using (var configurator = new ControllerConfigurator<OrderController>(HttpMethod.Put, "order"))
            {
                var controller = configurator.GetConfigured();
                var updateOrder = new UpdateOrder();
                controller.BusinessLogicFacade.OrderManager.UpdateOrder(Arg.Is(updateOrder)).Returns(info => { throw new ObjectNotFoundException("", typeof(UpdateOrder)); });

                var result = Call<NotFoundResult>(() => controller.PutOrder(updateOrder));

                Assert.IsNotNull(result);                
            }
        }


    }
}
