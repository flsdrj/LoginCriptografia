using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Projeto.Presentation.Validations
{
    //Toda classe de validação customizada criada no Asp.Net
    //deve haver a clase 'ValidationAttribute' e sobrescrever
    //o metodo Isvalid da superclasse
    public class SenhaValidator : ValidationAttribute
    {
        //esse metodo recebe um parametro 'object value', que contem
        //o valor do campo que esta sendo validado
        public override bool IsValid(object value)
        {
            if (value==null)
            {
                return false;
            }
            var senha = value.ToString();
            
            return senha.Any(char.IsUpper)
                && senha.Any(char.IsLower)
                && senha.Any(char.IsDigit)
                && (senha.Contains("!") 
                || senha.Contains("@")
                || senha.Contains("#")
                || senha.Contains("$")
                || senha.Contains("%")
                || senha.Contains("&"));
        }
        
    }
}
