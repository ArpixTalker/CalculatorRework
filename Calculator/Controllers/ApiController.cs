using Calculator.Models;
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

        /// <summary>
        /// Returns value of the expression o error string base on result of Calculator service
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="whole"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/math/compute")]
        public string ComputeExpression(string expression, bool whole) 
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = this._calc.ComputeExpression(expression, whole);

                if (output.Success == true)
                {
                    this.SaveMathExpression(expression + "=" + output.Value);
                    return output.Value.ToString();
                }
                else 
                {
                    SendError("Could not get result");
                }    
            }
            else 
            {
                SendError("Expression was nul or empty");   
            }
            
            return "error";
        }

        /// <summary>
        /// Returns last 10 expression saved in database
        /// </summary>
        /// <returns></returns>
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
                SendError("Failed to select expressions from database");
            }
            return new JsonResult(models);
        }

        /// <summary>
        /// Saves math expression to database (if valid and computed)
        /// </summary>
        /// <param name="expression"></param>
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
                SendError("Received Expression was null");
            }
        }

        /// <summary>
        /// Send error method
        /// </summary>
        /// <param name="message"></param>
        private void SendError(string message) 
        {
            
        }
    }
}
