using ATMAPI.Data;
using ATMAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ATMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly ApplicationDBContext _context;

        public TransactionController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet("HistoryOperation")]
        public async Task<IActionResult> GetOperationHistory([FromQuery] int userId)
        {
            if (!StoreLogin.loggedInUsers.Contains(userId))
                return Unauthorized("User not logged in.");
            var operationHistory = await _context.UserOperationHestories
    .Include(h => h.Operation) 
    .Include(h => h.User) 
    .Where(h => h.UserId == userId) 
    .OrderByDescending(h => h.OperationDateTime) 
    .Select(h => new
    {
       
        ID=h.OperationId,
        Name = h.User.UserName, 
        Operation = h.Operation.OperationName,
        
        OperationDate = h.OperationDateTime
        
    })
    .ToListAsync();

            if (!operationHistory.Any())
                return NotFound("No operations found.");

            return Ok(operationHistory);
        }

        [HttpGet("HistoryTransaction")]
        public async Task<IActionResult> GetTransaction([FromQuery] int userId)
        {
            if (!StoreLogin.loggedInUsers.Contains(userId))
                return Unauthorized("User not logged in.");
            var transaction = await _context.UserTransactions
    .Include(h => h.Transaction)
    .Include(h => h.User)
    .Where(h => h.UserId == userId)
    .OrderByDescending(h => h.Transaction.OperationDateTime)
    .Select(h => new
    {

        ID=h.TransactionId,
        Name = h.User.UserName,
        Operation = h.Transaction.Operation.OperationName,

        OperationDate = h.Transaction.OperationDateTime,
        OldBalance=h.Transaction.UserBalanceBeforeOperation,
        NewBalance=h.Transaction.UserBalanceAfterOperation,
        status = h.Transaction.IsCompleteTransfer


    })
    .ToListAsync();

            if (!transaction.Any())
                return NotFound("No Transactions found.");

            return Ok(transaction);
        }
    }
}
