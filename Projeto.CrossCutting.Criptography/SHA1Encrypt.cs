using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Projeto.CrossCutting.Criptography
{
    public class SHA1Encrypt
    {
        //método para receber um valor string e retorna-lo
        //criptografado em SHA1 (HASH)
        public string GenerateHash(string value)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            //realizando a criptografia
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(value));
            var result = string.Empty;
            foreach (var item in hash)
            {
                result += item.ToString("X2"); //X2 -> HEXADECIMAL
            }

            return result;
        }
    }
}
