using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Projeto.CrossCutting.Criptography;
using Projeto.CrossCutting.Mail;
using Projeto.Data.Contracts;
using Projeto.Data.Repositories;

namespace Projeto.Presentation
{
    public class Startup
    {
        //atributo para ler o arquivo appsettings.json
        private IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //mapeamento da injeção de dependencia
            var conctoinString = configuration.GetConnectionString("Aula13");

            services.AddTransient<IPerfilRepository, PerfilRepository>
                (map => new PerfilRepository(conctoinString));

            services.AddTransient<IUsuarioRepository, UsuarioRepository>
                (map => new UsuarioRepository(conctoinString));

            services.AddTransient<MD5Encrypt>();            
            services.AddTransient<SHA1Encrypt>();

            //carregando na classe MailSettings as configurações
            //para envio de email mapeadas no arquivo appsettings.json
            var mailsettings = new MailSettings();
            new ConfigureFromConfigurationOptions<MailSettings>
                (configuration.GetSection("MailSettings"))
                .Configure(mailsettings);

            //mapeando a classe MailService
            services.AddTransient<MailService>
                (map => new MailService(mailsettings));

            //habilitar o uso de autenticação para o projeto
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication
                (CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
