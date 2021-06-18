using NUnit.Framework;
using AccountManagementApi.Controllers;
using AccountManagementApi.Repository;
using AccountManagementApi.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace AccountManagementTesting
{
    public class Tests
    {
        private Mock<IAccountManagementRepo> config;
        private AccountManagementController TokenObj;

        [SetUp]
        public void Setup()
        {
            config = new Mock<IAccountManagementRepo>();
            TokenObj = new AccountManagementController(config.Object);
        }

        [Test]
        public void returnValidCreateAccount()
        {
            config.Setup(p => p.addStatusForAccountCreation(1, "Savings")).Returns(new AccountCreationStatus
            {
                Message = "Account has been successfully created",
                AccountID = 1
            });
            var result = TokenObj.createAccount(new AccountCreationDTO { CustomerId = 1 });
            Assert.IsNotNull(result);
        }

        [Test]
        public void returnInValid_CreateAccount()
        {
            config.Setup(p => p.addStatusForAccountCreation(1, "Savings")).Returns(new AccountCreationStatus
            {
                Message = "Account has been successfully created",
                AccountID = 1
            });
            var result = TokenObj.createAccount(new AccountCreationDTO { CustomerId = 0 });
            Assert.IsNull(result);
        }

        [Test]
        public void getCutomerAccounts_Called_When_Given_Valid_CustomerId()
        {

            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = obj.getCustomerAccounts(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void getCutomerAccounts_Called_When_Given_InValid_CustomerId()
        {

            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = obj.getCustomerAccounts(0);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void getaccount_Called_When_Given_Valid_AccountId()
        {
            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = obj.getAccount(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void getaccount_Called_When_Given_InValid_AccountId()
        {
            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = obj.getAccount(0);
            Assert.IsNull(result);
        }

        [Test]
        public void getstatements_Called_When_Given_Valid_AccountId()
        {
            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = TokenObj.getAccountStatement(new GetStatementDTO() { AccountId = 1, FromDate = DateTime.Now, ToDate = DateTime.Now });
            Assert.IsNotNull(result);
        }

        [Test]
        public void getstatements_Called_When_Given_InValid_AccountId()
        {
            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = TokenObj.getAccountStatement(new GetStatementDTO() { AccountId = 0, FromDate = DateTime.Now, ToDate = DateTime.Now });
            Assert.IsNull(result);
        }

        [Test]
        public void deposit_Called_When_Given_Valid_AccountId()
        {
            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = TokenObj.deposit(new TransactionDTO { AccountId = 1, Amount = 200 });
            Assert.IsNotNull(result);
        }

        [Test]
        public void deposit_Called_When_Given_InValid_AccountId()
        {
            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = TokenObj.deposit(new TransactionDTO { AccountId = 0, Amount = 200 });
            Assert.IsNull(result);
        }

        [Test]
        public void withdraw_Called_When_Given_Valid_AccountId()
        {
            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = TokenObj.withdraw(new TransactionDTO { AccountId = 1, Amount = 200 });
            Assert.IsNotNull(result);
        }

        [Test]
        public void withdraw_Called_When_Given_InValid_AccountId()
        {
            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = TokenObj.withdraw(new TransactionDTO { AccountId = 0, Amount = 200 });
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void getCustomerAccounts_Called_When_Given_valid_CustomerId()
        {
            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = TokenObj.getCustomerAccounts(1);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void getCustomerAccounts_Called_When_Given_Invalid_CustomerId()
        {
            var mock = new Mock<IAccountManagementRepo>();
            AccountManagementController obj = new AccountManagementController(mock.Object);
            var result = TokenObj.getCustomerAccounts(1);
            Assert.That(result, Is.EqualTo(0));
        }
    }
}