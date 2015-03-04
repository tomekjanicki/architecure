using System;
using System.Collections.Generic;
using System.Linq;
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
        public void InsertCustomer_ValidArguments_PersistsData()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            const int returnedValue = 1;
            var data = CustomerManagerTestHelper.GetValidInsertCustomerAsync();
            commandsUnitOfWork.CustomerCommand.Returns(CustomerManagerTestHelper.GetCustomerCommand());
            commandsUnitOfWork.CustomerCommand.InsertCustomer(Arg.Is(data)).Returns(returnedValue);
            commandsUnitOfWork.CustomerCommand.IsMailUnique(Arg.Is<IsMailUnique>(@async => @async.Mail == data.Mail)).Returns(true);

            var returnedData = businessLogicFacade.CustomerManager.InsertCustomer(data);

            Assert.True(returnedData.Item1 == returnedValue);
            commandsUnitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void InsertCustomer_NotUniqueMail_ReturnsValidationResults()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = CustomerManagerTestHelper.GetValidInsertCustomerAsync();
            commandsUnitOfWork.CustomerCommand.Returns(CustomerManagerTestHelper.GetCustomerCommand());
            commandsUnitOfWork.CustomerCommand.IsMailUnique(Arg.Is<IsMailUnique>(@async => @async.Mail == data.Mail)).Returns(false);

            var returnedData = businessLogicFacade.CustomerManager.InsertCustomer(data);

            Assert.That(returnedData.Item2.Count > 0 && returnedData.Item2.Values.FirstOrDefault(l => l.Contains(Const.CustomerMailIsNotUniqueMessage)) != null);
        }

        [Test]
        public void InsertCustomer_DefaultArgument_ReturnsValidationResults()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = new InsertCustomer();
            commandsUnitOfWork.CustomerCommand.Returns(CustomerManagerTestHelper.GetCustomerCommand());

            var returnedData = businessLogicFacade.CustomerManager.InsertCustomer(data);

            Assert.That(returnedData.Item1 == null);
            Assert.That(returnedData.Item2.Count > 0);
        }
      
        [Test]
        public void FindCustomers_ValidArguments_ReturnResults()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();

            const string name = "n1";
            const int pageSize = 10;
            commandsUnitOfWork.CustomerCommand.Returns(CustomerManagerTestHelper.GetCustomerCommand());
            var data = new List<FindCustomers> { new FindCustomers { Name = name } };
            var paged = new Paged<FindCustomers>(data.Count, data);
            commandsUnitOfWork.CustomerCommand.FindCustomers(name, new PageAndSortCriteria(pageSize, data.Count, null)).Returns(paged);

            var returnedData = businessLogicFacade.CustomerManager.FindCustomers(name, new PageAndSortCriteria(pageSize, data.Count, null));

            Assert.True(returnedData.Count == data.Count && returnedData.Items.FirstOrDefault(customer => customer.Name == name) != null);
        }

        
    }
}