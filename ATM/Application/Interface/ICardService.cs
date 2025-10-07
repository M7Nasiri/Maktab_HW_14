using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Application.Interface
{
    public interface ICardService
    {
        bool IsCardExist(string cardNumber);
        bool IsCardActive(string cardNumber);
        bool IsValidCardNumer(string cardNumber);
        string ShowCardHolderName(string cardNumber);
        bool Login(string cardNumber, string password);
        float GetBalance(string cardNumber);
        bool SetVerificationCode(string cardNumber, string code);
        string GetVerificationCode(string cardNumber);
        bool ChangeCardPassword(string cardNumber, string oldPassword, string newPassword);
        bool ApproveVerificationCode(string cardNumber, string verificationCode);
        bool WriteVerificationCodeToFile(string cardNumber, string code);
        bool CardToCard(string sourceCardNumber, string destinationCardNumber, float amount);


        bool Withdraw(string cardNumber, float amount);
        bool Settle(string cardNumber, float amount);
        //bool CardToCard(string sourceCardNumber, string destinationCardNumber, float amount);

        bool HasEnoughMoney(string cardNumber,float amount);
        bool IsValidAmount(float amount);
        bool UpdateFailAttempts(string cardNumber, int count);
        bool BlockedCard(string cardNumber);
        bool AssignGenerationCode(string cardNumber);
        //bool ReGenrateVerificationCode(string cardNumber);
        bool StopTimer();
    }
}
