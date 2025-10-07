using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Domain.Exceptions
{
    public class CardIsNotActiveException : ValidateException
    {
        public CardIsNotActiveException(string message):base(message)
        {
            
        }
    }
}
