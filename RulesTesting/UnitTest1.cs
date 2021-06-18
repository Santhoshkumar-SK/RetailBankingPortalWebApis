using Moq;
using NUnit.Framework;
using RulesMicroService.Controllers;
using RulesMicroService.Models;
using RulesMicroService.Services;
using RulesMicroService.Scheduler;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace RulesTesting
{
    public class Tests
    {
        RulesController controller;
        
        [SetUp]
        public void Setup()
        {
            controller = new RulesController();
        }

        [Test]
        public void EvaluateMinBal_ValidInput_int()
        {
            
            var result = controller. EvaluateMinBal(new TransactionDTO { AccountId =1,Amount=5000});
            RulesStatus status = new RulesStatus { Status = "allowed" };
            Assert.AreEqual(result.Status, status.Status);
        }

        [Test]
        public void EvaluateMinBal_InValidInput_int()
        {
            var result = controller.EvaluateMinBal(new TransactionDTO { AccountId = 2, Amount = 1000 });
            RulesStatus status = new RulesStatus { Status = "denied" };
            Assert.AreNotEqual(result.Status, status.Status);
        }

        [Test]
        public void GetAccounts_Savings()
        {
            var result = controller.GetServiceCharges("Savings");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetAccounts_Current()
        {
            var result = controller.GetServiceCharges("Current");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetAccounts_Exception()
        {
            var result = controller.GetServiceCharges("WrongInput");
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

    }
}