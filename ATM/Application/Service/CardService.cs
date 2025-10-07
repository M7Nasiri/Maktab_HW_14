using ATM.Application.Interface;
using ATM.Application.Utils;
using ATM.Domain.Interface;
using ATM.Infrastructure.Repository;
using System.Timers;

namespace ATM.Application.Service
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepo;
        private readonly IFileService _fileService;
        private readonly ITransactionService _transactionService;
        private System.Timers.Timer _timer;
        public CardService()
        {
            _cardRepo = new CardRepository();
            _fileService = new FileService();
            _transactionService = new TransactionService();
        }

        public bool ApproveVerificationCode(string cardNumber, string verificationCode)
        {
            return _cardRepo.ApproveVerificationCode(cardNumber, verificationCode);
        }

        //public bool CardToCard(string sourceCardNumber, string destinationCardNumber, float amount)
        //{  
        //    return _cardRepo.CardToCard(sourceCardNumber, destinationCardNumber, amount);
        //}

        public bool ChangeCardPassword(string cardNumber, string oldPassword, string newPassword)
        {
            if (_cardRepo.ChangeCardPassword(cardNumber, oldPassword, newPassword)) return true;
            return false;
        }

        public float GetBalance(string cardNumber)
        {
            return _cardRepo.GetBalance(cardNumber);
        }

        public string GetVerificationCode(string cardNumber)
        {
            return _cardRepo.GetVerificationCode(cardNumber);
        }

        public bool IsCardActive(string cardNumber)
        {
            return _cardRepo.IsCardActive(cardNumber);
        }

        public bool IsCardExist(string cardNumber)
        {
            return _cardRepo.IsCardExist(cardNumber);
        }

        public bool IsValidCardNumer(string cardNumber)
        {
            return _cardRepo.IsValidCardNumer(cardNumber);
        }

        public bool Login(string cardNumber, string password)
        {
            if (_cardRepo.Login(cardNumber, password))
            {
                //string code = Generation.CreateVerificationCode();
                //SetVerificationCode(cardNumber, code);
                //WriteVerificationCodeToFile(cardNumber, code);
                return true;
            }
            return false;

        }

        public bool AssignGenerationCode(string cardNumber)
        {
            var card = _cardRepo.GetCard(cardNumber);
            //if(card.VerificationCode == null || 
            //    card.VerificationCodeCreatedAt == null ||
            //    (DateTime.UtcNow - card.VerificationCodeCreatedAt.Value).TotalSeconds >= 30)
            //{
            string code = Generation.CreateVerificationCode();
            SetVerificationCode(cardNumber, code);
            WriteVerificationCodeToFile(cardNumber, code);

            StartVerificationCodeTimer(cardNumber);

            return true;
            //}
            //return false;    
        }


        private void StartVerificationCodeTimer(string cardNumber)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }

            _timer = new System.Timers.Timer(30000); // هر ۳۰ ثانیه (برحسب میلی‌ثانیه)
            _timer.Elapsed += (sender, e) =>
            {
                string newCode = Generation.CreateVerificationCode();
                _cardRepo.SetVerificationCode(cardNumber, newCode);
                WriteVerificationCodeToFile(cardNumber, newCode);
            };

            _timer.AutoReset = true;
            _timer.Start();
        }

        public bool StopTimer()
        {
            _timer?.Stop();
            _timer?.Dispose();
            return true;
        }

        //public bool ReGenrateVerificationCode(string cardNumber)
        //{
        //    var card = _cardRepo.GetCard(cardNumber);
        //    if (card.VerificationCode == null ||
        //       card.VerificationCodeCreatedAt == null ||
        //      (DateTime.UtcNow - card.VerificationCodeCreatedAt.Value).TotalSeconds >= 30)
        //        return true;
        //    return false;
        //}

        public bool SetVerificationCode(string cardNumber, string code)
        {
            return _cardRepo.SetVerificationCode(cardNumber, code);
        }

        public string ShowCardHolderName(string cardNumber)
        {
            return _cardRepo.ShowCardHolderName(cardNumber);
        }

        public bool WriteVerificationCodeToFile(string cardNumber, string code)
        {
            _fileService.WriteToFile($"{cardNumber} : {code}");
            return true;
        }

        public bool Withdraw(string cardNumber, float amount)
        {
            var card = _cardRepo.GetCard(cardNumber);
            if (card.Balance >= amount)
            {
                //_context.Card.Where(c => c.CardNumber == cardNumber)
                //    .ExecuteUpdate(setters => setters.SetProperty(c => c.Balance, balance - amount));
                var total = Generation.CalculateTransactionFee(amount) + amount;
                var bal = card.Balance - total;
                return _cardRepo.SetBalance(cardNumber, bal);
            }
            return false;
        }

        public bool Settle(string cardNumber, float amount)
        {
            var card = _cardRepo.GetCard(cardNumber);
            var bal = card.Balance + amount;
            //return false;
            return _cardRepo.SetBalance(cardNumber, bal);
        }

        public bool CardToCard(string sourceCardNumber, string destinationCardNumber, float amount)
        {
            if (Withdraw(sourceCardNumber, amount) && Settle(destinationCardNumber, amount))
            {
                return _cardRepo.ConfirmedTransaction();
            }
            return false;

        }

        public bool HasEnoughMoney(string cardNumber, float amount)
        {
            float currentBal = GetBalance(cardNumber);
            float fee = Generation.CalculateTransactionFee(amount);
            float total = amount + fee;
            if (currentBal >= total)
                return true;
            return false;
        }

        public bool IsValidAmount(float amount)
        {
            if (amount <= 0)
                return false;
            return true;
        }

        public bool UpdateFailAttempts(string cardNumber, int count)
        {
            return _cardRepo.UpdateFailAttempts(cardNumber, count);
        }

        public bool BlockedCard(string cardNumber)
        {
            _cardRepo.BlockedCard(cardNumber);
            return true;
        }
    }
}
