using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Domain.Entities
{
    public class Card
    {
        public string CardNumber { get; set; }
        public string HolderName { get; set; }
        public float Balance { get; set; }
        public bool IsActive { get; set; } = true;
        public string Password { get; set; }
        public int FailedAttempt { get; set; }
        public string VerificationCode { get; set; }
        public DateTime? VerificationCodeCreatedAt { get; set; }

        public List<Transaction> SourceTransactions { get; set; }
        public List<Transaction> DestinationTransactions { get; set; }

    }
}
