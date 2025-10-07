using ATM.Application.Utils;
using ATM.Domain.Entities;
using ATM.Domain.Interface;
using ATM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Infrastructure.Repository
{
    public class CardRepository : ICardRepository
    {

        private readonly BankDbContext _context;
        public CardRepository()
        {
            _context = new BankDbContext();
        }
        public bool ApproveVerificationCode(string cardNumber, string verificationCode)
        {
           return _context.Card.Any(c => c.CardNumber == cardNumber && c.VerificationCode == verificationCode);
        }

        public bool ChangeCardPassword(string cardNumber,string oldPassword, string newPassword)
        {
            _context.Card.Where(c => c.CardNumber == cardNumber && c.Password == oldPassword)
                .ExecuteUpdate(setters => setters.SetProperty(c => c.Password, newPassword));
            //_context.SaveChanges();
            return true;
        }


        public string GetVerificationCode(string cardNumber)
        {
            return _context.Card.FirstOrDefault(c => c.CardNumber == cardNumber).VerificationCode;
        }

        public float GetBalance(string cardNumber)
        {
            return _context.Card.Where(c => c.CardNumber == cardNumber).Select(x => x.Balance).First();
        }

        public bool IsCardActive(string cardNumber)
        {
            return _context.Card.Any(c=>c.CardNumber == cardNumber && c.IsActive);
        }

        public bool IsCardExist(string cardNumber)
        {
            return _context.Card.Any(c => c.CardNumber == cardNumber);
        }

        public bool Login(string cardNumber, string password)
        {
            return _context.Card.Any(c => c.CardNumber == cardNumber && c.Password == password);
        }

        public bool SetVerificationCode(string cardNumber, string code)
        {
            _context.Card.Where(c => c.CardNumber == cardNumber)
                .ExecuteUpdate(setters => setters.SetProperty(c => c.VerificationCode, code)
                .SetProperty(c=>c.VerificationCodeCreatedAt,DateTime.UtcNow)
                );
            return true;
        }

        public string ShowCardHolderName(string cardNumber)
        {
            return _context.Card.Where(c=>c.CardNumber ==cardNumber).Select(c=>c.HolderName).First();
        }

        public bool IsValidCardNumer(string cardNumber)
        {
            return Validation.IsValidCardNumber(cardNumber);
        }

        public bool SetBalance(string cardNumber,float amount)
        {
            //_context.Card.Where(c => c.CardNumber == cardNumber)
            //       .ExecuteUpdate(setters => setters.SetProperty(c => c.Balance, amount));
             var card = GetCardInner(cardNumber);
            card.Balance = amount;
            return true;
        }

        public Card GetCard(string cardNumber)
        {
            return _context.Card.FirstOrDefault(c=>c.CardNumber==cardNumber);   
        }
       

        public bool ConfirmedTransaction()
        {
            _context.SaveChanges();
            return true;
        }

        public bool UpdateFailAttempts(string cardNumber, int count)
        {
            _context.Card.Where(c => c.CardNumber == cardNumber)
               .ExecuteUpdate(setters => setters.SetProperty(c => c.FailedAttempt, count));
            return true;
        }
        private Card GetCardInner(string cardNumber)
        {
            return _context.Card.FirstOrDefault(c => c.CardNumber == cardNumber);
        }

        public bool BlockedCard(string cardNumber)
        {
            _context.Card.Where(c => c.CardNumber == cardNumber)
                .ExecuteUpdate(setters => setters.SetProperty(c => c.IsActive, false));
            return true;
        }
        //public bool Withdraw(string cardNumber, float amount)
        //{
        //    //var balance = _context.Card.Where(c => c.CardNumber == cardNumber).Select(c => c.Balance).First();
        //    var card = _context.Card.FirstOrDefault(c => c.CardNumber == cardNumber);
        //    if (card.Balance >= amount)
        //    {
        //        //_context.Card.Where(c => c.CardNumber == cardNumber)
        //        //    .ExecuteUpdate(setters => setters.SetProperty(c => c.Balance, balance - amount));
        //        card.Balance 
        //        return true;
        //    }
        //    return false;
        //}

        //public bool Settle(string cardNumber, float amount)
        //{
        //    var balance = _context.Card.Where(c => c.CardNumber == cardNumber).Select(c => c.Balance).First();
        //    _context.Card.Where(c => c.CardNumber == cardNumber)
        //            .ExecuteUpdate(setters => setters.SetProperty(c => c.Balance, balance + amount));
        //    return false;
        //}

        //public bool CardToCard(string sourceCardNumber, string destinationCardNumber, float amount)
        //{
        //   if( Withdraw(sourceCardNumber, amount) && Settle(destinationCardNumber, amount))
        //    {
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    return false;

        //}
    }
}
