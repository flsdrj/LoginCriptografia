using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;

namespace Projeto.Presentation.Controllers
{
    [Authorize]
    public class AreaRestritaController : Controller
    {
        //atributos..
        private readonly IUsuarioRepository usuarioRepository;

        public AreaRestritaController(IUsuarioRepository usuarioRepository)
        {
            this.usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MinhaConta()
        {
            Usuario usuario = null; //vazio

            try
            {
                usuario = usuarioRepository.Find(User.Identity.Name);

            }
            catch (Exception e)
            {
                TempData["Mensagem"] = e.Message;
            }
            return View(usuario);
        }

        [Authorize(Roles = "Pessoa Jurídica")]
        public IActionResult Usuarios()
        {
            var lista = new List<Usuario>();
            try
            {
                //ober usuarios do banco de dados
                lista = usuarioRepository.FindAll();
            }
            catch (Exception e)
            {
                TempData["Mensagem"] = e.Message;
            }
            return View(lista);
        }
    }
}