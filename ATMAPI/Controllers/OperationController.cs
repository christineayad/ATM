using ATMAPI.Data;
using ATMAPI.DTO;
using ATMAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace ATMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationController : Controller
    {
        private readonly ApplicationDBContext _context;
        

        public OperationController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet("checkbalance")]
        public async Task<IActionResult> checkBalance([FromQuery] int userId)
        {
            if (!StoreLogin.loggedInUsers.Contains(userId))
                return Unauthorized("User not logged in.");
           
            // Fetch the user's details
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
            {
                return NotFound("User not found.");
            }
            decimal oldbalance = user.Currentbalance;
            var transactions = new Transaction
            {
                OperationDateTime = DateTime.Now,
                OperationAmount = 0,
                OperationId = 3,
                UserBalanceAfterOperation = user.Currentbalance,
                UserBalanceBeforeOperation = oldbalance,
                ReceiverId = null,
                IsCompleteTransfer = false
            };
            _context.Transactions.Add(transactions);
          await  _context.SaveChangesAsync();
            var usertransaction = new UserTransaction()
            {
               
                TransactionId = transactions.Id,
                UserId = user.UserId
            };
            _context.UserTransactions.Add(usertransaction);

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Balance is", Balance = user.Currentbalance });
        }
        [HttpPost("Deposit")]
        public async Task<IActionResult> Deposit( [FromQuery] int userId, [FromQuery] decimal Amount)
        {
            if (!StoreLogin.loggedInUsers.Contains(userId))
                return Unauthorized("User not logged in.");
            if (Amount <= 0)
                return BadRequest("Amount must be greater than zero.");
          
            var user = await _context.Users.FindAsync(userId);
            decimal oldbalance = user.Currentbalance;
            if (user == null)
            {
                return NotFound("User not found.");
            }
            user.Currentbalance += Amount;
            await _context.SaveChangesAsync();
            var transactions = new Transaction
            {
                OperationDateTime = DateTime.Now,
                OperationAmount =Amount,
                OperationId = 4,
                UserBalanceAfterOperation = user.Currentbalance,
                UserBalanceBeforeOperation = oldbalance,
                ReceiverId = null,
                IsCompleteTransfer = false
            };
            _context.Transactions.Add(transactions);
            await _context.SaveChangesAsync();
            var usertransaction = new UserTransaction()
            {
                TransactionId = transactions.Id,
                UserId = user.UserId
            };
            _context.UserTransactions.Add(usertransaction);

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Deposit successful", NewBalance = user.Currentbalance });
        }
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromQuery] int userId, [FromQuery] decimal amount)
        {
            if (!StoreLogin.loggedInUsers.Contains(userId))
                return Unauthorized("User not logged in.");

            if (amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var user = await _context.Users.FindAsync(userId);
            decimal oldbalance = user.Currentbalance;
            if (user == null)
                return NotFound("User not found.");

            if (user.Currentbalance < amount)
                return BadRequest("Insufficient balance.");

            user.Currentbalance -= amount;
            await _context.SaveChangesAsync();
            var transactions = new Transaction
            {
                OperationDateTime = DateTime.Now,
                OperationAmount = amount,
                OperationId = 5,
                UserBalanceAfterOperation = user.Currentbalance,
                UserBalanceBeforeOperation = oldbalance,
                ReceiverId = null,
                IsCompleteTransfer = false
            };
            _context.Transactions.Add(transactions);
            await _context.SaveChangesAsync();
            var usertransaction = new UserTransaction()
            {
                TransactionId = transactions.Id,
                UserId = user.UserId
                
            };
            _context.UserTransactions.Add(usertransaction);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Withdrawal successful", NewBalance = user.Currentbalance });
        }
        [HttpPost("TransferMoney")]
        public async Task<IActionResult> TransferMoney(int senderId, int receiverId, decimal amount)
        {
           
                if (senderId == receiverId)
                    return BadRequest("Sender and receiver cannot be the same.");
           
            if (amount <= 0)
                    return BadRequest("Transfer amount must be greater than zero.");

                var sender = await _context.Users.FindAsync(senderId);
            decimal oldbalance = sender.Currentbalance;
            if (sender == null)
                    return NotFound("Sender not found.");

                if (sender.Currentbalance < amount)
                    return BadRequest("Insufficient balance.");
            sender.Currentbalance -= amount;
            await _context.SaveChangesAsync();
        
            var transaction = new Transaction
                {
               OperationDateTime = DateTime.Now,
                OperationAmount = amount,
                OperationId = 6,
                UserBalanceAfterOperation = sender.Currentbalance,
                UserBalanceBeforeOperation = oldbalance,
                ReceiverId = receiverId,
                IsCompleteTransfer = false
                    
                   
            };

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
            var usertransactions = new UserTransaction
            {
                TransactionId = transaction.Id,
                UserId = senderId
            };
            _context.UserTransactions.Add(usertransactions);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Transfer request created successfully." });
            
        }
        [HttpPost("ReciveMoney")]
        public async Task<IActionResult> RecieveMoney([FromQuery] bool status, [FromQuery] int transactionId)
        {
            // Fetch the transaction with related details
            var transaction = await _context.UserTransactions
                .Include(ut => ut.Transaction) // Include the related Transaction
                .Include(t => t.User) // Include the sender User
                .FirstOrDefaultAsync(ut => ut.TransactionId == transactionId);

            if (transaction == null)
                return NotFound(new { Message = "Transaction not found" });

            var transactionDetails = transaction.Transaction;

            // Handle the case when the transfer is canceled
            if (!status)
            {
                transaction.User.Currentbalance += transactionDetails.OperationAmount;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Transfer Canceled" });
            }

            // Fetch the receiver using ReceiverId
            var receiver = await _context.Users.FindAsync(transactionDetails.ReceiverId);
            if (receiver == null)
                return NotFound(new { Message = "Receiver not found" });

            // Update balances
            var balanceBeforeReceiving = receiver.Currentbalance;
           
          
            receiver.Currentbalance += transactionDetails.OperationAmount;
            //transaction.User.Currentbalance -= transactionDetails.OperationAmount;
            var balanceAfterReceiving = receiver.Currentbalance;
            var newTransaction = new Transaction
            {
                OperationDateTime = DateTime.Now,
                OperationAmount = transactionDetails.OperationAmount,
                OperationId = 7,
                UserBalanceBeforeOperation = balanceBeforeReceiving,
                UserBalanceAfterOperation = balanceAfterReceiving,
                ReceiverId = transactionDetails.ReceiverId,
                IsCompleteTransfer = status
            };

            _context.Transactions.Add(newTransaction);
            await _context.SaveChangesAsync();

            var userTransaction = new UserTransaction
            {
                TransactionId = newTransaction.Id,
                UserId = receiver.UserId
            };

            _context.UserTransactions.Add(userTransaction);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Transfer Received" });
        }


    }
}
