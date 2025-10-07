using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace ATM.Domain.Exceptions
{
    public class CardIsNotExistException : ValidateException
    {
        public CardIsNotExistException(string message):base(message)
        {
            
        }
    }
}
