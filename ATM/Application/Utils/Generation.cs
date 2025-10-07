using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Application.Utils
{
    public class Generation
    {
        public static string CreateVerificationCode()
        {
            Random rnd = new Random();
            string result = rnd.Next(10000, 100000).ToString();
            return result;
        }
        public static float CalculateTransactionFee(float amount)
        {
            if(amount <= 1000)
            {
                return (amount * 5 )/ 100;
            }
            else
            {
                return (amount * 15) / 100;
            }
        }
    }
}
