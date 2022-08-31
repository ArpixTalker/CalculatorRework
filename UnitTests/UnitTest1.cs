using CalculatorLogic;
using DAL;
using Xunit.Sdk;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        CalculatorService service = new CalculatorService();

        [TestMethod]
        public void ComputeTest()
        {
            string expression = "5+4";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 9);

            expression = "5.2+4.8";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 10);

            expression = "5-4/1";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 1);

            expression = "5+4----";
            Assert.AreEqual(service.ComputeExpression(expression).Success, false);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 0);

            expression = "*5-4/ww1";
            Assert.AreEqual(service.ComputeExpression(expression).Success, false);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 0);

        }
    }
}