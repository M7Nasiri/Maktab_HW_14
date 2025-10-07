

using ATM.Domain.Dto;
using ATM.Domain.Entities;
using ATM.Domain.Interface;
using ATM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace ATM.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankDbContext _context;
        public TransactionRepository()
        {
            _context = new BankDbContext();
        }
        public bool CreateTransaction(CreateTransactionDto dto)
        {
            var transaction = new Transaction
            {
                TransactionDate = DateTime.Now,
                Amount = dto.Amount,
                DestinationCardNumber = dto.DestinationCardNumber,
                SourceCardNumber = dto.SourceCardNumber,
                IsSuccessfull = dto.IsSuccessfull,
            };
            _context.Add(transaction);
            _context.SaveChanges();
            return true;
        }

        public List<GetTransactionDto> GetTransactionByCardNumber(string cardNumber)
        {
            return _context.Transaction.Include(c => c.SourceCard).Include(c => c.DestinationCard)
                .Where(c => c.SourceCardNumber == cardNumber || c.DestinationCardNumber == cardNumber).Select(c => new GetTransactionDto
                {
                    TransactionId = c.TransactionId,
                    Amount = c.Amount,
                    DestinationCardNumber = c.DestinationCardNumber,
                    SourceCardNumber = c.SourceCardNumber,
                    IsSuccessfull = c.IsSuccessfull,
                    TransactionDate = c.TransactionDate,
                }).ToList();
        }

        public bool IsReachedDailyTransferLimit(string cardNumber,float amount)
        {

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var totalAmount = _context.Transaction
                .Where(t => t.SourceCardNumber == cardNumber
                            && t.TransactionDate >= today
                            && t.TransactionDate < tomorrow)
                .Sum(t => t.Amount);
            totalAmount += amount;
            const float dailyLimit = 2_000f;
            return totalAmount <= dailyLimit;
        }
    }
}
