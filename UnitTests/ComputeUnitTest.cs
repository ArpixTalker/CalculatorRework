using CalculatorLogic;
using Xunit.Sdk;

namespace UnitTests
{
    [TestClass]
    public class ComputeUnitTest
    {
        CalculatorService service = new CalculatorService();

        [TestMethod]
        public void ComputeTestWholeNumbers()
        {
            string expression = "5+4";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 9);

            expression = "5*8";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 40);

            expression = "5 / 2";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 2.5);

            expression = "5-4/1";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 1);



        }

        [TestMethod]
        public void ComputeDecimals() 
        {
            string expression = "5.2+4.9";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 10.1);

            expression = "4.8/0.8";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 6);

            expression = "5.1*10";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 51);

            expression = "5.9-4.1";
            Assert.AreEqual(service.ComputeExpression(expression).Success, true);
            Assert.AreEqual(service.ComputeExpression(expression).Value,1.8);
        }

        [TestMethod]
        public void ComputeErrors() 
        {
            string expression = "5+4----";
            Assert.AreEqual(service.ComputeExpression(expression).Success, false);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 0);

            expression = "*5-4/ww1";
            Assert.AreEqual(service.ComputeExpression(expression).Success, false);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 0);

            expression = "";
            Assert.AreEqual(service.ComputeExpression(expression).Success, false);
            Assert.AreEqual(service.ComputeExpression(expression).Value, 0);
        }
    }
}