using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_db.Data;
using Domotica_db.Data.CustomIdentity;
using Domotica_db.Library;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domotica_db.Areas.Usuarios.Models;

namespace Domotica_db.Areas.Usuarios.Pages.Eliminar
{
    public class EliminarModel : PageModel
    {
        private ListObject objeto = new ListObject();
        private static InputModel model;
        public EliminarModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context, IHostingEnvironment environment)
        {
            objeto._context = context;
            objeto._environment = environment;
            //objeto._image = new Uploadimage();
            objeto._usuarios = new LUsuarios(userManager, signInManager, roleManager, context);
        }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            
            if (id != null)
            {
                var data = await objeto._usuarios.GetUsuariosAsync(id);
                Input = new InputModel
                {
                    Id = data[0].Id,
                    
                    NIF = data[0].NIF,
                    Nombre = data[0].Nombre,
                    Apellido = data[0].Apellido,
                    Email = data[0].Email,
                };
                model = Input;
                return Page();
            }
            else
            {
                //var url = Request.Scheme + "://" + Request.Host.Value;
                return Redirect("/Usuarios?area=Usuarios");
            }
        }
        public InputModel Input { get; set; }
        public class InputModel : InputModelRegistrar
        {

        }

    }
}