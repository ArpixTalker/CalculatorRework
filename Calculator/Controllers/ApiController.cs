﻿using Calculator.Models;
using CalculatorLogic.Interfaces;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Calculator.Controllers
{
    public class ApiController : Controller
    {

        private readonly CalculatorDBContext _database;
        private readonly ICalculatorService _calc;
        private readonly ILogger<ApiController> _logger;

        public ApiController(CalculatorDBContext database, ICalculatorService calc, ILogger<ApiController> logger)
        {
            this._database = database;
            this._calc = calc;
            this._logger = logger;
        }

        [HttpPost]
        [Route("api/v1/math/compute")]
        public string ComputeExpression(string expression, bool whole) 
        {
            if (!string.IsNullOrEmpty(expression))
            {
                try
                {
                    var output = this._calc.ComputeExpression(expression, whole);

                    if (output.Success == true)
                    {
                        this.SaveMathExpression(expression + "=" + output.Value);
                        return output.Value.ToString();
                    }
                }
                catch (SyntaxErrorException)
                {
                    SendError(LogLevel.Error, $"Received Expression has incorrect format: {expression}");
                }
            }
            SendError(LogLevel.Warning, "Expression was nul or empty");
            return "error";
        }

        [HttpGet]
        [Route("api/v1/math/getLatestExpressions")]
        public JsonResult GetExpressions()
        {
            List<ExpressionViewModel> models = new List<ExpressionViewModel>();

            var expressions = this._database.Expressions.OrderByDescending(x => x.CreatedAt).Take(10).ToList();

            if (expressions != null)
            {
                expressions.ForEach(entity =>
                {
                    models.Add(new ExpressionViewModel() { Expression = entity.Expression });
                });
            }
            else 
            {
                SendError(LogLevel.Error, "Failed to select expressions from database");
            }
            return new JsonResult(models);
        }

        private void SaveMathExpression(string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                _database.Expressions.Add(new MathExpression()
                {
                    ExpressionID = new Guid(),
                    Expression = expression,
                    CreatedAt = DateTime.UtcNow
                });

                _database.SaveChanges();
            }
            else 
            {
                SendError(LogLevel.Error, "Received Expression was null");
            }
        }

        private void SendError(LogLevel level, string message) 
        {
            
        }
    }
}
