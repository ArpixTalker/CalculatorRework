using Calculator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DAL;
using DAL.Models;

namespace Calculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CalculatorDBContext _database;

        public HomeController(ILogger<HomeController> logger, CalculatorDBContext context)
        {
            _database = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("api/v1/math/saveExpression")]
        public bool SaveMathExpression([FromForm]string expression)
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
                return true;
            }
            return false;
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
            return new JsonResult(models);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}