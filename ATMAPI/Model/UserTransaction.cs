using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMAPI.Model
{
    public class UserTransaction
    {
        public int Id { get; set; }
        public int? TransactionId { get; set; }
        public virtual Transaction? Transaction { get; set; }
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
