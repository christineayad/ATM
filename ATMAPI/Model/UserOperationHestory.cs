using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMAPI.Model
{
    public class UserOperationHestory
    {
        public int Id { get; set; }

        public DateTime OperationDateTime { get; set; }
        public int? UserId { get; set; }
        public virtual User? User {get; set; }

        public int? OperationId { get; set; }
        public virtual Operation? Operation { get; set; }


    }
}
