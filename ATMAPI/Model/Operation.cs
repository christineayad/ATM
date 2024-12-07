using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMAPI.Model
{
   

    public class Operation
    {
       
        public int Id { get; set; } 
        public string OperationName { get; set; }

        // Navigation properties
        //public ICollection<Transaction> Transactions { get; set; }
        //public ICollection<UserOperationHestory> userOperationHestories { get; set; }
    }
}
