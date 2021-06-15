using Microsoft.AspNetCore.Mvc;
using RulesMicroService.Models;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RulesMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(RulesController));

        /// <summary>
        /// This function take AccountType (Savings or Current)
        /// It generates the Service charge.
        /// </summary>
        /// <param name="AccountType"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetServiceCharges")]
        public float GetServiceCharges(string AccountType)
        {
            if (AccountType == "Savings")
            {
                return 100;
            }
            else if (AccountType == "Current")
            {
                return 200;
            }
            else
            {
                return 0;
            }
        }


        /// <summary> 
        /// It will Checks the given amount is greater than or equal to minBalance(5000)
        /// <summary>
        /// <param name = "id"> </param>
        /// <returns>Status (allowed or denied) </returns>

        [HttpGet]
        [Route("EvaluateMinBal")]
        public RulesStatus EvaluateMinBal([FromQuery] TransactionDTO transaction)
        {
            _log4net.Info("Evaluating Minimum Balance");
            try
            {
                Double MinBalance = 5000;

                if (transaction.Amount >= MinBalance)
                {
                    return new RulesStatus { Status = "allowed" };
                }
                else
                {
                    return new RulesStatus { Status = "denied" };
                }
            }
            catch (NullReferenceException e)
            {
                _log4net.Error("NullReferenceException caught. Issue in calling Account API");
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
