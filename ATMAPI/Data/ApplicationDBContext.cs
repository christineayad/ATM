using ATMAPI.Model;

using Microsoft.EntityFrameworkCore;

namespace ATMAPI.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<UserCategory> UserCategories { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserOperationHestory> UserOperationHestories { get; set; }
        public DbSet<UserTransaction> UserTransactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCategory>().HasData(new UserCategory {Id=1, Name = "Admin" }, new UserCategory {Id=2, Name = "Ordinary" });
            modelBuilder.Entity<Operation>().HasData(
              new Operation { Id = 1, OperationName = "Register" }
            , new Operation { Id = 2, OperationName = "Login" }
            , new Operation { Id = 3, OperationName = "CheckBalance" }
            , new Operation { Id = 4, OperationName = "Deposit" }
            , new Operation { Id = 5, OperationName = "Withdraw" }
            , new Operation { Id = 6, OperationName = "TransferMoney" }
            , new Operation { Id = 7, OperationName = "RecieveMoney" }
            , new Operation { Id = 8, OperationName = "RemoveUser" }
            , new Operation { Id = 9, OperationName = "LogOut" });
            base.OnModelCreating(modelBuilder);
        }
    }
}
