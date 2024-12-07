using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMAPI.Model
{
    public class User
    {
        public int UserId { get; private set; }
     
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public decimal Currentbalance { get; set; }
       
        public int? UserCategoryId { get; set; }
        public virtual UserCategory? usercategory { get; set; }
        public ICollection<UserOperationHestory> UserOperationHistories { get; set; }
        public ICollection<UserTransaction> userTransactions { get; set; }

    }
}
