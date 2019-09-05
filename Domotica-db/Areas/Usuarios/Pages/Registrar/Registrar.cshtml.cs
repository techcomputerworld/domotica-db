using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domotica_db.Data.CustomIdentity;
using Domotica_db.Data;
using Domotica_db.Library;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Domotica_db.Areas.Usuarios.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Domotica_db.Areas.Usuarios.Controllers;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;

namespace Domotica_db.Areas.Usuarios.Pages.Registrar
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class RegistrarModel : PageModel
    {
        private ListObject objeto = new ListObject();
       
        public RegistrarModel(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager,
           ApplicationDbContext context, IHostingEnvironment environment)
        {
            objeto._roleManager = roleManager;
            objeto._userManager = userManager;
            objeto._environment = environment;
            objeto._context = context;
            objeto._usuarios = new LUsuarios();
            objeto._usersRole = new UserRoles();
            objeto._image = new UploadImage();
            objeto._userRoles = new List<SelectListItem>();
        }
        public void OnGet()
        {
            //para obtener los roles de usuario SuperAdmin,Admin, User,
            Input = new InputModel
            {
                rolesLista = objeto._usersRole.getRoles(objeto._roleManager)
            };

        }
        public async Task<IActionResult> OnPostAsync()
        {
            var valor = await guardarAsync();
            if (valor)
            {
                return RedirectToAction(nameof(UsuariosController.Index), "Usuarios");
            }
            else
            {
                return Page();
            }
        }
        //enlazamos las propiedades del front end con las del backend 
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : InputModelRegistrar
        {
            [TempData]
            public string ErrorMessage { get; set; }

            public IFormFile AvatarImage { get; set; }
            public List<SelectListItem> rolesLista { get; set; }
        }
        private async Task<bool> guardarAsync()
        {
            var valor = false;
            var rootFolder = Directory.GetCurrentDirectory();

            string filePath;
            try
            {
                if (ModelState.IsValid)
                {
                    objeto._userRoles.Add(new SelectListItem
                    {
                        Text = Input.Role
                    });
                    var userList = objeto._userManager.Users.Where(u => u.Email.Equals(Input.Email)).ToList();
                    if (userList.Count.Equals(0))
                    {
                        var user = new ApplicationUser
                        {
                            UserName = Input.Email,
                            Email = Input.Email,
                            PhoneNumber = Input.PhoneNumber
                            
                        };
                        var result = await objeto._userManager.CreateAsync(user, Input.Password);
                        if (result.Succeeded)
                        {
                            string imageName;
                            await objeto._userManager.AddToRoleAsync(user, Input.Role);
                            //A traves del email doy con la tupla que me hace falta en la base de datos
                            var listUser = objeto._userManager.Users.Where(u => u.Email.Equals(Input.Email)).ToList();
                            //Copiamos la imagen en el caso de que elija el usuario una imagen
                            
                            
                            /* a traves del nombre de la imagen podemos saber si se ha copiado o no, y si se ha copiado pues  
                             * ponerla en Imagen la cadena de texto completa donde esta la imagen. 
                             * seguimos por aqui programando y viendo como solucionar el problema de registro de usuarios.
                             */
                            if (Input.AvatarImage != null)
                            {
                                imageName = Input.AvatarImage.FileName;
                                string pathimage = Path.Combine(rootFolder, "wwwroot\\images\\fotos\\Usuarios\\");
                                filePath = pathimage + imageName;
                                /*
                                 * Copia la imagen si el usuario selecciona una imagen pondre restricciones a la hora de que el usuario
                                 * no pueda usar imagenes grandes de mas de 150 KB por ejemplo o algo menos tal vez.
                                 */
                                await objeto._image.copiarImagenAsync(Input.AvatarImage, imageName, objeto._environment, "Usuarios");

                            }
                            else
                            {
                                string imageDefault = "default.png";
                                filePath = Path.Combine(rootFolder, "wwwroot\\images\\fotos\\" + imageDefault);
                            }
                            Input.Imagen = filePath;
                            user.Imagen = Input.Imagen;

                            //prueba a: var usuarios45 =  objeto._context.Usuarios.ToList();
                            await objeto._context.AddAsync(user);
                            objeto._context.SaveChanges();
                            
                            valor = true;
                        }
                        else
                        {
                            foreach (var item in result.Errors)
                            {
                                Input = new InputModel
                                {
                                    ErrorMessage = item.Description,
                                    rolesLista = objeto._userRoles
                                };

                            };
                            valor = false;
                        }

                    }
                    else
                    {
                        Input = new InputModel
                        {
                            ErrorMessage = "El " + Input.Email + " ya esta registrado",
                            rolesLista = objeto._userRoles
                        };
                        valor = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Input = new InputModel
                {
                    ErrorMessage = ex.Message,
                    rolesLista = objeto._userRoles
                };
            }
            return valor;
        }
    }
}