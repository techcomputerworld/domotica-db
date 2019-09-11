using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_db.Data;
using Domotica_db.Data.CustomIdentity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domotica_db.Library;
using Domotica_db.Areas.Usuarios.Models;
using Microsoft.AspNetCore.Http;
using Domotica_db.Areas.Usuarios.Controllers;

namespace Domotica_db.Areas.Usuarios.Pages.Editar
{
    public class EditModel : PageModel
    {
        private ListObject objeto = new ListObject();
        private static List<ApplicationUser> userList1;
       

        public EditModel(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHostingEnvironment environment)
        {
            //inicializamos todos los objetos que necesitamos a la creación dle objeto EditModel cuando lo llamamos con el
            //UsuariosController
            objeto._roleManager = roleManager;
            objeto._userManager = userManager;
            objeto._environment = environment;
            objeto._context = context;
            objeto._usuarios = new LUsuarios();
            objeto._usersRole = new UserRoles();
            objeto._image = new UploadImage();
            objeto._userRoles = new List<SelectListItem>();
        }
        //método OnGet importante para recibir los datos que nos manda el usuario cuando hace click en el boton editar
        //id en realidad puede ser el email del usuario que queremos editar
        public async void OnGetAsync(string id)
        {
            Input = new InputModel
            {
                rolesLista = objeto._usersRole.getRoles(objeto._roleManager)
            };
            if (id != null)
            {

                await editAsync(id);
            }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : InputModelRegistrar
        {
            [TempData]
            public string ErrorMessage { get; set; }
            public IFormFile AvatarImage { get; set; }
            public List<SelectListItem> rolesLista { get; set; }
        }
        //para mostrar la información de una tupla
        private async Task editAsync(string id ) 
        {
            //en mi caso id es igual al correo electronico 
            string Email = id;
            userList1 = objeto._userManager.Users.Where(u => u.Email.Equals(Email)).ToList();
            //userList2 = objeto._context.Usuarios.Where(u => u.ApplicationUserId.Equals(userList1[0].Id)).ToList();
            var userRoles = await objeto._usersRole.GetRole(objeto._userManager, objeto._roleManager, userList1[0].Id);
            //vamos básicamente estoy mostrando al usuario esta información tal cual.

            Input = new InputModel
            {
                Nombre = userList1[0].Nombre,
                Apellido = userList1[0].Apellido,
                NIF = userList1[0].NIF,
                PhoneNumber = userList1[0].PhoneNumber,
                Email = userList1[0].Email,
                Password = "*********",
                Role = userRoles[0].Text,
                rolesLista = getRoles(userRoles[0].Text)
            };
            
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (Input.Email != null)
            {
                var valor = await actualizarAsync();
                if (valor)
                {
                    return RedirectToAction(nameof(UsuariosController.Index), "Usuarios");
                }
                else
                {
                    
                    return Page();
                }
            }
            else
            {
                return Page();
            }
        }
        private async Task<bool> actualizarAsync()
        {
            var valor = false;
            try
            {
                if (ModelState.IsValid)
                {
                    objeto._userRoles.Add(new SelectListItem
                    {
                        Text = Input.Role
                    });
                    var identityUser = new ApplicationUser
                    {
                        Id = userList1[0].Id,
                        UserName = Input.Email,
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        EmailConfirmed = userList1[0].EmailConfirmed,
                        LockoutEnabled = userList1[0].LockoutEnabled,
                        LockoutEnd = userList1[0].LockoutEnd,
                        NormalizedEmail = userList1[0].NormalizedEmail,
                        NormalizedUserName = userList1[0].NormalizedUserName,
                        PasswordHash = userList1[0].PasswordHash,
                        PhoneNumberConfirmed = userList1[0].PhoneNumberConfirmed,
                        SecurityStamp = userList1[0].SecurityStamp,
                        TwoFactorEnabled = userList1[0].TwoFactorEnabled,
                        AccessFailedCount = userList1[0].AccessFailedCount,
                        ConcurrencyStamp = userList1[0].ConcurrencyStamp,
                        Nombre = Input.Nombre,
                        Apellido = Input.Apellido,
                        NIF = Input.NIF,
                        Imagen = Input.Imagen,
                    };
                    //esta linea es la que añade el dato que necesito a cada usuario con identityUser sabremos que usuario  
                    //estamos actualizando.
                    await objeto._userManager.AddToRoleAsync(identityUser, Input.Role);
                    objeto._context.Update(identityUser);
                    await objeto._context.SaveChangesAsync();
                   
                    //La imagen dijimos que iba a cogerla del nombre que haya
                    string imageName;
                    if (Input.Imagen != null)
                    {
                        imageName = Input.Imagen;
                    }
                    else
                    {
                        imageName = "default.png";
                    }
                    
                    await objeto._image.copiarImagenAsync(Input.AvatarImage, imageName, objeto._environment, "Users");

                    valor = true;
                }
                else
                {
                    Input = new InputModel
                    {
                        ErrorMessage = "Seleccione un role",
                        rolesLista = objeto._usersRole.getRoles(objeto._roleManager)
                    };
                    valor = false;
                }
            }
            catch (Exception ex)
            {

                Input = new InputModel
                {
                    ErrorMessage = ex.Message,
                    rolesLista = getRoles(Input.Role)
                };
                valor = false;
            }

            
            return valor;
        }
        //rellena el listado del combobox que he puesto en la página
        private List<SelectListItem> getRoles(String role)
        {
            objeto._userRoles.Add(new SelectListItem
            {
                Text = role
            });
            var roles = objeto._usersRole.getRoles(objeto._roleManager);
            roles.ForEach(item => {
                if (item.Text != role)
                {
                    objeto._userRoles.Add(new SelectListItem
                    {
                        Text = item.Text
                    });
                }
            });
            return objeto._userRoles;
        }
        
    }
}