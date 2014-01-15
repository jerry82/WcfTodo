using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace WcfService
{
    public class Utils
    {
        public static string GetHash(string password) 
        {
            string hash = String.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                hash = Convert.ToBase64String(data);
            }
            return hash;
        }

        public static bool VerifyHash(string input, string hashValue)
        {
            string inputHash = GetHash(input);
            bool result = (hashValue.Equals(inputHash));
            return result;
        }
    }
}