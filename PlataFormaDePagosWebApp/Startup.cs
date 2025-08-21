using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using PlataFormaDePagosWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartupAttribute(typeof(PlataFormaDePagosWebApp.Startup))]
namespace PlataFormaDePagosWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Inicializacion();
        }

        private void Inicializacion()
        {
            var context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Administrador"))
                roleManager.Create(new IdentityRole("Administrador"));
            if (!roleManager.RoleExists("Cajero"))
                roleManager.Create(new IdentityRole("Cajero"));

            string adminEmail = "donald@bancopatitos.com";
            string adminPassword = "Admin2025*";

            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
            };

            var result = userManager.Create(adminUser, adminPassword);
            if (result.Succeeded)
            {
                userManager.AddToRole(adminUser.Id, "Administrador");
            }

        }
    }
}
