using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Projeto.CrossCutting.Criptography
{
    public class MD5Encrypt
    {
        //método para receber um valor string e retorna-lo
        //criptografado em MD5 (HASH)
        public string GenerateHash(string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            //realizando a criptografia
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            var result = string.Empty;
            foreach (var item in hash)
            {
                result += item.ToString("X2"); //X2 -> HEXADECIMAL
            }

            return result;
        }
    }
}
