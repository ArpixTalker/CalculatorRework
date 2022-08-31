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


        public ApiController(CalculatorDBContext database, ICalculatorService calc)
        {
            this._database = database;
            this._calc = calc;
        }

        [HttpPost]
        [Route("api/v1/math/compute")]
        public string ComputeExpression([FromForm]string expression) 
        {
            try
            {
                var output =  this._calc.ComputeExpression(expression);

                if (output.Success == true)
                {
                    this.SaveMathExpression(expression + "=" + output.Value);
                    return output.Value.ToString();
                }
            }
            catch (SyntaxErrorException) 
            {
                SendError(LogLevel.Error, $"Received Expression Has incorrect format: {expression}");
            }
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
