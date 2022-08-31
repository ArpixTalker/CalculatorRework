using Calculator.Models;
using CalculatorLogic.Interfaces;
using System.Data;

namespace CalculatorLogic
{
    public class CalculatorService : ICalculatorService
    {
        DataTable _dt;

        public CalculatorService() 
        {
            this._dt = new DataTable();
        }

        public ComputeReturn ComputeExpression(string expression) 
        {
            ComputeReturn result = new ComputeReturn();
            try
            {
                var output = this._dt.Compute(expression, "").ToString();

                if (output != null)
                {
                    if (double.TryParse(output, out double computed))
                    {
                        return new ComputeReturn()
                        {
                            Success = true,
                            Value = computed
                            
                        };
                    }
                    else 
                    {
                        SendError(new ArgumentException($"Could not convert result to double {computed}"));
                    }
                }
            }
            catch (Exception e) 
            {
                SendError(e);
            }

            return new ComputeReturn()
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