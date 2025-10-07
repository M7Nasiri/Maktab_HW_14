using ATM.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Domain.Interface
{
    public interface ITransactionRepository
    {
        
        bool CreateTransaction(CreateTransactionDto dto);
        List<GetTransactionDto> GetTransactionByCardNumber(string cardNumber);
        bool IsReachedDailyTransferLimit(string cardNumber, float amount);
    }
}
