using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Application.Interface
{
    public interface IFileService
    {
        bool CreateDirectory();
        bool CreateFile();
        bool WriteToFile(string text);
    }
}
