using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projeto.Presentation.Models;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using Projeto.CrossCutting.Criptography;
using Projeto.CrossCutting.Mail;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Projeto.Presentation.Controllers
{
    public class AccountController : Controller
    {
        //atributos
        private readonly IUsuarioRepository usuarioRepository; //repositório
        private readonly IPerfilRepository perfilRepository;  //repositório
        private readonly MD5Encrypt mD5Encrypt; //crosscuting de criptografia
        private readonly MailService mailService; //crosscuting de envio de emial

        public AccountController(IUsuarioRepository usuarioRepository, IPerfilRepository perfilRepository, MD5Encrypt mD5Encrypt, MailService mailService)
        {
            this.usuarioRepository = usuarioRepository;
            this.perfilRepository = perfilRepository;
            this.mD5Encrypt = mD5Encrypt;
            this.mailService = mailService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost] //recebe o SUBMIT do formulário
        public IActionResult Login(AutenticarUsuarioModel model)
        {
            //verificar se todos os campos do forumlario passaram nas validações
            if (ModelState.IsValid)
            {
                try
                {
                    //buscar usuario no banco de dados atraves do email e senha
                    var usuario = usuarioRepository.Find(model.Email, mD5Encrypt.GenerateHash(model.Senha));
                    
                    //verificar se o usuario foi encontrado
                    if (usuario != null)
                    {
                        //gerando credencial para o usuario logado
                        var identity = new ClaimsIdentity(new[]
                            { 
                                new Claim(ClaimTypes.Name, usuario.Email), //nome de usuário
                                new Claim(ClaimTypes.Role, usuario.Perfil.Nome) //perifl
                            },
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        //realizar a autenticação
                        var autenticacao = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, autenticacao);

                        return RedirectToAction("Index", "AreaRestrita");
                    }
                    else
                    {
                        TempData["Mensagem"] = "Acesso Negado. Usuário inválido.";
                    }

                 }
                catch (Exception e)
                {
                    TempData["Mensagem"] = "Ocorreu um erro: " + e.Message;
                }


            }
            return View();
        }

        //metodo para realizar o logout do usuario
        public IActionResult Logout()
        {
            //destruir o ticket(credencial) de acesso do usuário
            //este ticket esta gravado na forma de um cookie
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View(GerarUsuarioModel());
        }

        [HttpPost] //recebe o SUBMIT do formulário
        public IActionResult Register(CriarUsuarioModel model)
        {
            //verificar se todos os campos do forumlario passaram nas validações
            if (ModelState.IsValid)
            {
                try
                {
                    if (usuarioRepository.Find(model.Email) != null)
                    {
                        TempData["Mensagem"] = "Este e-mail já encontra-se cadastrado, por favor tente outro.";
                    }
                    else
                    {
                        var usuario = new Usuario();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = mD5Encrypt.GenerateHash(model.Senha);
                        usuario.DataCriacao = DateTime.Now;
                        usuario.Status = 1; //ativo
                        usuario.IdPerfil = model.IdPerfil; //chave estrangeira

                        usuarioRepository.Create(usuario);

                        TempData["Mensagem"] = "Usuário cadastrado com sucesso";
                        ModelState.Clear();

                        EnviarEmailDeBoasVindas(usuario);
                    }
                }
                catch (Exception e)
                {
                    TempData["Mensagem"] = e.Message;
                }

            }
            return View(GerarUsuarioModel());
        }

        private void EnviarEmailDeBoasVindas(Usuario usuario)
        {
            var assunto = "Conta de Usuário Criada com Sucesso - COTI INFORMATICA";
            var texto = new StringBuilder();

            texto.Append($"Olá, {usuario.Nome}\n \n");
            texto.Append($"Sua conta de usuário foi criada com sucesso! \n");
            texto.Append($"Faça seu login para ter acesso ao sistema. \n");
            texto.Append($" \n \n");
            texto.Append($" Atenciosamente, \n");
            texto.Append($" Equipe COTI Informática \n");

            mailService.SendMail(usuario.Email, assunto, texto.ToString());
        }

        private CriarUsuarioModel GerarUsuarioModel()
        {
            var model = new CriarUsuarioModel();
            model.ListagemPerfis = new List<SelectListItem>();

            foreach (var item in perfilRepository.FindAll())
            {
                var opcao = new SelectListItem();
                opcao.Value = item.IdPerfil.ToString();//valor do dorpdownlist
                opcao.Text = item.Nome; //texto exibido no dorpdownlist

                model.ListagemPerfis.Add(opcao);
            }

            return model;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}