using ATM.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Application.Interface
{
    public interface ITransactionService
    {
        bool CreateTransaction(CreateTransactionDto dto);
        List<GetTransactionDto> GetTransactionByCardNumber(string cardNumber);
        bool IsReachedDailyTransferLimit(string cardNumber, float amount);
        bool Init();
        
    }
}
