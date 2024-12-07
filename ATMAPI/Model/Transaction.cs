using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMAPI.Model
{
    public class Transaction
    {
        public int Id { get; set; } 
        public decimal OperationAmount { get; set; }
        public DateTime OperationDateTime { get; set; }
        public decimal UserBalanceBeforeOperation { get; set; }
        public decimal UserBalanceAfterOperation { get; set; }
        public int? ReceiverId { get; set; }
        public bool IsCompleteTransfer { get; set; }
        //public int? UserId { get; set; }
        //public virtual User? User { get; set; }
        public ICollection<UserTransaction> userTransactions { get; set; } 
        //= new List<User>();

        public int? OperationId { get; set; }
        public virtual Operation? Operation { get; set; }

    }
}
