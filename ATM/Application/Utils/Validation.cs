using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Application.Utils
{
    public class Validation
    {
        public static bool IsValidCardNumber(string cardNumber)
        {
            if (cardNumber.Length == 16)
                return true;
            return false;
        }
    }
}
