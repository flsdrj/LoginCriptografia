using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Projeto.Presentation.Models
{
    public class AutenticarUsuarioModel
    {
        [EmailAddress(ErrorMessage ="Por favor, informe um email válido")]
        [Required(ErrorMessage="Por favor, informe o email do usuário")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Por favor, informe a senha do usuário")]
        public string Senha { get; set; }
    }
}
