using Calculator.Models;
using CalculatorLogic.Interfaces;
using System.Data;

namespace CalculatorLogic
{
    public class CalculatorService : ICalculatorService
    {
        private DataTable _dt;

        public CalculatorService() 
        {
            this._dt = new DataTable();
        }

        public ComputeResult ComputeExpression(string expression, bool whole) 
        {
            try
            {
                var output = this._dt.Compute(expression, "").ToString();

                if (output != null)
                {
                    if (double.TryParse(output, out double computed))
                    {
                        return new ComputeResult()
                        {
                            Success = true,
                            Value = (whole ? Math.Round(computed, 0) : computed)

                        };
                    }
                    else
                    {
                        SendError(new ArgumentException($"Could not convert result to double: {computed}"));
                    }
                }
                else
                {
                    SendError(new NullReferenceException("Output of compute was null"))
                }
            }
            catch (SyntaxErrorException e) 
            {
                SendError(e);
            }

            return new ComputeResult()
            {
                Success = false,
                Value = 0
            };
        }

        private void SendError(Exception e) 
        {
            
        }
    }
}