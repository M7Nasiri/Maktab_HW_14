using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Domain.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public float Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsSuccessfull { get; set; }

        public string SourceCardNumber { get; set; }
        public Card SourceCard { get; set; }

        public string DestinationCardNumber { get; set; }
        public Card DestinationCard { get; set; }

    }
}
