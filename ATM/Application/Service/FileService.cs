using ATM.Application.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Application.Service
{
    public class FileService : IFileService
    {
        private const string path = "d://tempCard";
        private const string fileName = "VerificationCode.txt";
        public bool CreateDirectory()
        {
            if (Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }
            return false;
        }

        public bool CreateFile()
        {
            string p = Path.Combine(path, fileName);
            if (File.Exists(p))
            {
                File.Create(p);
                return true;
            }
            return false;
        }

        public bool WriteToFile(string text)
        {
            File.WriteAllText(path, text);
            return true;
        }
    }
}
