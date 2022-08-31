using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLogic.Interfaces
{
    public interface ICalculatorService
    {
        ComputeResult ComputeExpression(string expression, bool whole);
    }
}
