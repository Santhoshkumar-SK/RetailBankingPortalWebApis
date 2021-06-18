using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TransactionMicroservice.Controllers;
using TransactionMicroservice.Models;
using TransactionMicroservice.Repository;

namespace TransactionTesting
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void deposit_WhenCalledWithNonZeroValues_ReturnsOk()
        {
            var mock = new Mock<ITransactionsRepo>();
            TransactionsController obj = new TransactionsController(mock.Object);
            var result = obj.deposit(new TransactionDTO() { AccountId = 1, Amount = 1.00 });
            Assert.IsNotNull(result);
        }

        [Test]
        public void deposit_WhenCalledWithZeroValues_ReturnsOk()
        {
            var mock = new Mock<ITransactionsRepo>();
            TransactionsController obj = new TransactionsController(mock.Object);
            var result = obj.deposit(new TransactionDTO() { AccountId = 0, Amount = 0.00 });
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void withdraw_WhenCalledWithNonZeroValues_Ok()
        {
            var mock = new Mock<ITransactionsRepo>();
            TransactionsController obj = new TransactionsController(mock.Object);
            var result = obj.WithDraw(new TransactionDTO() { AccountId = 1, Amount = 1.00 });
            Assert.IsNotNull(result);
        }

        [Test]
        public void withdraw_WhenCalledWithZeroValues_ReturnsNotFound()
        {
            var mock = new Mock<ITransactionsRepo>();
            TransactionsController obj = new TransactionsController(mock.Object);
            var result = obj.WithDraw(new TransactionDTO { AccountId = 0, Amount = 0.00 });
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void Transfer_WhenCalledWithNonZeroValues_ReturnsOk()
        {
            var mock = new Mock<ITransactionsRepo>();
            TransactionsController obj = new TransactionsController(mock.Object);
            var result = obj.transfer(new TransferDTO { SourceAccountId = 1, DestinationAccountId = 1, Amount = 1 });
            Assert.IsNotNull(result);
        }

        [Test]
        public void Transfer_WhenCalledWithZeroValue_ReturnsOk()
        {
            var mock = new Mock<ITransactionsRepo>();
            TransactionsController obj = new TransactionsController(mock.Object);
            var result = obj.transfer(new TransferDTO { SourceAccountId = 0, DestinationAccountId = 0, Amount = 0 });
            Assert.IsNull(result);
        }

        [Test]
        public void GetTransactionHistory_WhenCalledWithNonZero_ReturnsOk()
        {
            int CustomerId = 1;
            var mock = new Mock<ITransactionsRepo>();
            TransactionsController obj = new TransactionsController(mock.Object);
            var result = obj.transactionHistory(CustomerId);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTransactionHistory_WhenCalledWithZero_ReturnsOk()
        {
            int CustomerId = 0;
            var mock = new Mock<ITransactionsRepo>();
            TransactionsController obj = new TransactionsController(mock.Object);
            var result = obj.transactionHistory(CustomerId);
            Assert.IsNull(result);
        }
    }
}