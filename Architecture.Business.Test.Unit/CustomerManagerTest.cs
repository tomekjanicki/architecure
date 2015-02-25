using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Architecture.Business.Test.Unit.Base;
using Architecture.Business.Test.Unit.Helper;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.ViewModel.Internal;
using NSubstitute;
using NUnit.Framework;

namespace Architecture.Business.Test.Unit
{
    public class CustomerManagerTest : BaseManagerTest
    {
        [Test]
        public async Task InsertCustomerAsync_ValidArguments_PersistsData()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            const int returnedValue = 1;
            var data = CustomerManagerTestHelper.GetValidInsertCustomerAsync();
            commandsUnitOfWork.CustomerCommand.Returns(CustomerManagerTestHelper.GetCustomerCommand());
            commandsUnitOfWork.CustomerCommand.InsertCustomerAsync(Arg.Is(data)).Returns(Delayed(returnedValue));
            commandsUnitOfWork.CustomerCommand.IsMailUniqueAsync(Arg.Is<IsMailUniqueAsync>(@async => @async.Mail == data.Mail)).Returns(Delayed(true));

            var returnedData = await businessLogicFacade.CustomerManager.InsertCustomerAsync(data);

            Assert.True(returnedData.Item1 == returnedValue);
            commandsUnitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public async Task InsertCustomerAsync_NotUniqueMail_ReturnsValidationResults()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = CustomerManagerTestHelper.GetValidInsertCustomerAsync();
            commandsUnitOfWork.CustomerCommand.Returns(CustomerManagerTestHelper.GetCustomerCommand());
            commandsUnitOfWork.CustomerCommand.IsMailUniqueAsync(Arg.Is<IsMailUniqueAsync>(@async => @async.Mail == data.Mail)).Returns(Delayed(false));

            var returnedData = await businessLogicFacade.CustomerManager.InsertCustomerAsync(data);

            Assert.That(returnedData.Item2.Count > 0 && returnedData.Item2.Values.FirstOrDefault(l => l.Contains(Const.CustomerMailIsNotUniqueMessage)) != null);
        }

        [Test]
        public async Task InsertCustomerAsync_DefaultArgument_ReturnsValidationResults()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = new InsertCustomerAsync();
            commandsUnitOfWork.CustomerCommand.Returns(CustomerManagerTestHelper.GetCustomerCommand());

            var returnedData = await businessLogicFacade.CustomerManager.InsertCustomerAsync(data);

            Assert.That(returnedData.Item1 == null);
            Assert.That(returnedData.Item2.Count > 0);
        }
        //testowy komentarz
      
        [Test]
        public async Task FindCustomersAsync_ValidArguments_ReturnResults()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();

            const string name = "n1";
            const int pageSize = 10;
            commandsUnitOfWork.CustomerCommand.Returns(CustomerManagerTestHelper.GetCustomerCommand());
            var data = new List<FindCustomersAsync> { new FindCustomersAsync { Name = name } };
            var paged = new Paged<FindCustomersAsync>(data.Count, data);
            commandsUnitOfWork.CustomerCommand.FindCustomersAsync(name, new PageAndSortCriteria(pageSize, data.Count, null)).Returns(Delayed(paged));

            var returnedData = await businessLogicFacade.CustomerManager.FindCustomersAsync(name, new PageAndSortCriteria(pageSize, data.Count, null));

            Assert.True(returnedData.Count == data.Count && returnedData.Items.FirstOrDefault(customer => customer.Name == name) != null);
        }

        
    }
}