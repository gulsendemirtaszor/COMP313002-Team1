using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
/// <summary>
/// Title: Encrypt and Decrypt a String in ASP.NET
/// Source: https://www.c-sharpcorner.com/blogs/encrypt-and-decrypt-a-string-in-asp-net1
/// </summary>
namespace Books.Models
{
    public class EncryptDecryptData
    {
        public string EncryptData(string _value)
        {
            byte[] _encByte = ASCIIEncoding.ASCII.GetBytes(_value);
            string _encData = Convert.ToBase64String(_encByte);
            return _encData;
        }

        public string DecryptData(string _value)
        {
            byte[] _decByte;
            string _decData;
            try
            {
                _decByte = Convert.FromBase64String(_value);
                _decData = ASCIIEncoding.ASCII.GetString(_decByte);
            }
            catch (Exception e)
            {
                _decData = string.Empty;
            }
            return _decData;
        }
    }
}
