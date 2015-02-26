using System.Collections.Generic;
using Architecture.Business.Exception;
using Architecture.Business.Test.Unit.Base;
using Architecture.Business.Test.Unit.Helper;
using Architecture.Util.Mail;
using Architecture.ViewModel;
using Architecture.ViewModel.Internal;
using NSubstitute;
using NUnit.Framework;

namespace Architecture.Business.Test.Unit
{
    public class OrderManagerTest : BaseManagerTest
    {
        [Test]
        public void GetOrder_ValidArguments_ReturnsData()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = new GetOrder();
            commandsUnitOfWork.OrderCommand.Returns(OrderManagerTestHelper.GetOrderCommand());
            commandsUnitOfWork.OrderCommand.GetOrder(Arg.Is(data.Id)).Returns(data);

            var returnedData = businessLogicFacade.OrderManager.GetOrder(data.Id);

            Assert.AreSame(data, returnedData);
        }

        [Test]
        public void GetOrder_WrongArguments_ThrowsObjectNotFoundException()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            const int id = 1;
            commandsUnitOfWork.OrderCommand.Returns(OrderManagerTestHelper.GetOrderCommand());
            commandsUnitOfWork.OrderCommand.GetOrder(Arg.Is(id)).Returns(info => null);

            Assert.Catch<ObjectNotFoundException>(() => businessLogicFacade.OrderManager.GetOrder(id));
        }

        [Test]
        public void InsertOrder_ValidArguments_PersistsData()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            const int returnedValue = 1;
            var data = OrderManagerTestHelper.GetValidInsertOrder();
            commandsUnitOfWork.OrderCommand.Returns(OrderManagerTestHelper.GetOrderCommand());
            commandsUnitOfWork.OrderCommand.InsertOrder(Arg.Is(data)).Returns(returnedValue);
            commandsUnitOfWork.MailCommand.Returns(OrderManagerTestHelper.GetMailCommand());

            var returnedData = businessLogicFacade.OrderManager.InsertOrder(data);

            Assert.True(returnedData.Item1 == returnedValue);
            commandsUnitOfWork.MailCommand.Received(1).Insert(Arg.Any<MailDefinition>());
            commandsUnitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void UpdateOrder_ValidArguments_PersistsData()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = OrderManagerTestHelper.GetValidUpdateOrder();
            commandsUnitOfWork.OrderCommand.Returns(OrderManagerTestHelper.GetOrderCommand());
            commandsUnitOfWork.OrderCommand.GetOrderVersion(Arg.Is(data.Id)).Returns(data.Version);
            commandsUnitOfWork.OrderCommand.GetCustomerMail(Arg.Is(data.Id)).Returns(string.Empty);
            commandsUnitOfWork.MailCommand.Returns(OrderManagerTestHelper.GetMailCommand());

            businessLogicFacade.OrderManager.UpdateOrder(data);

            commandsUnitOfWork.OrderCommand.Received(1).UpdateOrder(Arg.Is(data));
            commandsUnitOfWork.MailCommand.Received(1).Insert(Arg.Any<MailDefinition>());
            commandsUnitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void UpdateOrder_NotFound_ThrowsObjectNotFoundException()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = OrderManagerTestHelper.GetValidUpdateOrder();
            commandsUnitOfWork.OrderCommand.Returns(OrderManagerTestHelper.GetOrderCommand());
            commandsUnitOfWork.OrderCommand.GetOrderVersion(Arg.Is(data.Id)).Returns(info => null);

            Assert.Catch<ObjectNotFoundException>(() => businessLogicFacade.OrderManager.UpdateOrder(data));
        }

        [Test]
        public void UpdateOrder_WrongVersion_ThrowsOptimisticConcurrencyException()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = OrderManagerTestHelper.GetValidUpdateOrder();
            commandsUnitOfWork.OrderCommand.Returns(OrderManagerTestHelper.GetOrderCommand());
            commandsUnitOfWork.OrderCommand.GetOrderVersion(Arg.Is(data.Id)).Returns(new byte[] { 5, 18 });

            Assert.Catch<OptimisticConcurrencyException>(() => businessLogicFacade.OrderManager.UpdateOrder(data));
        }

        [Test]
        public void DeleteOrder_ValidArguments_PersistData()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();

            var data = OrderManagerTestHelper.GetValidDeleteOrder();
            commandsUnitOfWork.OrderCommand.Returns(OrderManagerTestHelper.GetOrderCommand());
            commandsUnitOfWork.OrderCommand.GetOrderVersion(Arg.Is(data.Id)).Returns(data.Version);
            commandsUnitOfWork.OrderCommand.GetCustomerMail(Arg.Is(data.Id)).Returns(string.Empty);
            commandsUnitOfWork.MailCommand.Returns(OrderManagerTestHelper.GetMailCommand());

            businessLogicFacade.OrderManager.DeleteOrder(data);

            commandsUnitOfWork.OrderCommand.Received(1).DeleteOrder(Arg.Is(data));
            commandsUnitOfWork.MailCommand.Received(1).Insert(Arg.Any<MailDefinition>());
            commandsUnitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void DeleteOrder_NotFound_ThrowsObjectNotFoundException()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();

            var data = OrderManagerTestHelper.GetValidDeleteOrder();
            commandsUnitOfWork.OrderCommand.Returns(OrderManagerTestHelper.GetOrderCommand());
            commandsUnitOfWork.OrderCommand.GetOrderVersion(Arg.Is(data.Id)).Returns(info => null);

            Assert.Catch<ObjectNotFoundException>(() => businessLogicFacade.OrderManager.DeleteOrder(data));
        }

        [Test]
        public void DeleteOrder_WrongVersion_ThrowsOptimisticConcurrencyException()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = OrderManagerTestHelper.GetValidDeleteOrder();
            commandsUnitOfWork.OrderCommand.Returns(OrderManagerTestHelper.GetOrderCommand());
            commandsUnitOfWork.OrderCommand.GetOrderVersion(Arg.Is(data.Id)).Returns(new byte[] { 5, 18 });

            Assert.Catch<OptimisticConcurrencyException>(() => businessLogicFacade.OrderManager.DeleteOrder(data));
        }

        [Test]
        public void CreateOrderConfirmationReminders_NoProblems_CreateReminders()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();

            var data = new List<GetNotConfirmedOrdersToRemind>
            {
                new GetNotConfirmedOrdersToRemind {CustomerMail = "example@example@com", Id = 1},
                new GetNotConfirmedOrdersToRemind {CustomerMail = "example@example@com", Id = 1}
            };
            commandsUnitOfWork.OrderCommand.Returns(OrderManagerTestHelper.GetOrderCommand());
            commandsUnitOfWork.OrderCommand.GetNotConfirmedOrdersToRemind().Returns(data);

            businessLogicFacade.OrderManager.CreateOrderConfirmationReminders();

            commandsUnitOfWork.MailCommand.Received(data.Count).Insert(Arg.Any<MailDefinition>());
            commandsUnitOfWork.OrderCommand.Received(data.Count).UpdateReminderCreated(Arg.Any<int>());
            commandsUnitOfWork.Received(1).SaveChanges();
        }

    }
}