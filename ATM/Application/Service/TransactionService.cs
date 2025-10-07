using ATM.Application.Interface;
using ATM.Domain.Dto;
using ATM.Domain.Interface;
using ATM.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Application.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _tranRepo;
        private readonly IFileService _fileService;
        public TransactionService()
        {
            _tranRepo = new TransactionRepository();
            _fileService = new FileService();
        }
        public bool CreateTransaction(CreateTransactionDto dto)
        {
           return _tranRepo.CreateTransaction(dto);
        }

        public bool IsReachedDailyTransferLimit(string cardNumber, float amount)
        {
            return _tranRepo.IsReachedDailyTransferLimit(cardNumber,amount);
        }

        public List<GetTransactionDto> GetTransactionByCardNumber(string cardNumber)
        {
            return _tranRepo.GetTransactionByCardNumber(cardNumber);
        }

        public bool Init()
        {
            _fileService.CreateDirectory();
            _fileService.CreateFile();
            return true;
        }
    }
}
