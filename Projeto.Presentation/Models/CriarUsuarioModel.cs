using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Projeto.Presentation.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Projeto.Presentation.Models
{
    public class CriarUsuarioModel
    {
        
        [MinLength(6, ErrorMessage="informe no minimo {1} caracteres")]
        [MaxLength(150, ErrorMessage = "informe no máximo {1} caracteres")]
        [Required(ErrorMessage="Por favor, informe o Nome do usuário")]
        public string Nome { get; set; }

        [EmailAddress(ErrorMessage = "Por favor, informe um email válido")]
        [Required(ErrorMessage = "Por favor, informe o email do usuário")]
        public string Email { get; set; }

        [SenhaValidator(ErrorMessage = 
            "Por favor, informe uma senha com pelo menos " +
            "1 caracter maiúsculo, " +
            "1 caracter minúsculo, " +
            "1 caracter especial e " +
            "1 digito numérico")]
        [MinLength(8, ErrorMessage = "informe no minimo {1} caracteres")]
        [MaxLength(20, ErrorMessage = "informe no máximo {1} caracteres")]
        [Required(ErrorMessage = "Por favor, informe a Senha do usuário")]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage ="Senhas não conferem")]
        [Required(ErrorMessage = "Por favor, confirme a Senha do usuário")]
        public string SenhaConfirmacao { get; set; }

        [Required(ErrorMessage = "Por favor, selecione um perfil")]
        public int IdPerfil { get; set; }//armazenar o perfil selecionado pelo usuario

        //listagem para exibir as opções de seleção do perfil(DropDownList)
        public List<SelectListItem> ListagemPerfis { get; set; }

    }
}
