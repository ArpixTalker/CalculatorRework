using Microsoft.EntityFrameworkCore;
using DAL.Models;

namespace DAL
{

    public class CalculatorDBContext: DbContext 
    {
        public CalculatorDBContext(DbContextOptions options) : base(options) 
        {
        
        }

        public DbSet<MathExpression> Expressions { get; set; }
    }
}