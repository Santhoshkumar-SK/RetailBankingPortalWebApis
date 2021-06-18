using NUnit.Framework;
using CustomerApi.Controllers;
using CustomerApi.Models;
using CustomerApi.Services;
using CustomerApi.Repository;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace CustomerTesting
{
    [TestFixture]
    public class Tests
    {
        private Mock<ICustomerRepo> _config;
        private CustomerController _controller;
        [SetUp]
        public void Setup()
        {
            _config = new Mock<ICustomerRepo>();
            _controller = new CustomerController(_config.Object);
        }
        [Test]
        public void Get_When_Called_CustomerDetails_Valid_Data()
        {
            var mock = new Mock<ICustomerRepo>();
            CustomerController obj = new CustomerController(mock.Object);
            var result = obj.createCustomer
                ( new Customer { 
                    customerId = 1, 
                    customerName = "Gomu", 
                    customerAddress = "Coimbatore",
                    customerDOB= Convert.ToDateTime("1998-11-20 01:02:01 AM"),
                    customerPannumber = "BYJPY23567", 
                    customerAdhaarnumber = "23456789876", 
                    customerAccountType = "Savings" 
                });
            Assert.IsNotNull(result);
        }

        [Test]
        public void Get_When_Called_CustomerDetails_InValid_Data()
        {
            var mock = new Mock<ICustomerRepo>();
            CustomerController obj = new CustomerController(mock.Object);
            var result = obj.createCustomer
                (new Customer
                {
                    customerId = 1,
                    customerName = "wronginput",
                    customerAddress = "Coimbatore",
                    customerDOB = Convert.ToDateTime("1998-11-20 01:02:01 AM"),
                    customerPannumber = "wronginput",
                    customerAdhaarnumber = "wronginput",
                    customerAccountType = "Savings"
                });
            Assert.IsNull(result);
        }

        [Test]
        public void Called_When_Given_Valid_CustomerId()
        {
            _config.Setup(p => p.getCustomerDetails(1)).Returns(new Customer { });

            var result = _controller.getCustomerDetails(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void Called_When_Given_CustomerId_Notinthelist()
        {
            var result = _controller.getCustomerDetails(0);

            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

    }
}