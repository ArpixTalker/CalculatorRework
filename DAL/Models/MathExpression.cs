using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class MathExpression
    {
        [Key]
        public Guid ExpressionID { get; set; }
        public string Expression { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
