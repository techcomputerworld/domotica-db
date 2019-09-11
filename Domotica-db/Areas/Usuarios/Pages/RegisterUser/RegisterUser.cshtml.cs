using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Domotica_db.Data;
using Domotica_db.Data.CustomIdentity;
using Domotica_db.Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Domotica_db.Areas.Usuarios.Pages.RegisterUser
{
    //necesito personalizar el registro de usuarios y no puedo desde la otra página
    public class RegisterUser : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterUser> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        
       
        #region constructor 
        public RegisterUser(UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<ApplicationRole> roleManager,
           ILogger<RegisterUser> logger,
           IEmailSender emailSender,
           ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            
            _context = context;
            //objeto._usuarios = new LUsuarios();
            //objeto._usersRole = new UserRoles();
            //en otra version de la aplicación lo usare para que el usuario pueda subir su foto en el registro.
            //objeto._image = new UploadImage();
            //objeto._userRoles = new List<SelectListItem>();
        }
        #endregion
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                   

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                //añadir los datos de DNI, Nombre y Apellido a la tabla Usuarios
                /* El Role que es en realidad el Rol de un usuario lo que hara es mirar en la tabla AspnetUsers si hay usuarios
                 * en principio nos pondra como usuario normal o sea "User". 
                 * 
                 */
                //int numUsers = 0;
                //numUsers = _context.Users.Count();
                //numUsers es que si no hay ningun usuario registrado en la aplicación se creara un SuperAdmin y si lo hay será, un 
                //User lo que se creara en la aplicación.
                /*
                if (numUsers == 0)
                {
                    Input.Role = "SuperAdmin";
                }
                else
                {
                    Input.Role = "User";
                }
                */
                
                //prueba a: var usuarios45 =  objeto._context.Usuarios.ToList();
                //await _context.AddAsync(usuarios);
                //_context.SaveChanges();
                //añadir el código para añadir datos de la tabla Usuarios
                //objeto._context.Usuarios.Where(u => u.ApplicationUserId == user.Id);




            }
            return Page();
        }

        public InputModelRegister Input { get; set; }

        public class InputModelRegister
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            //campos con los que trabajaremos en el registro
            [Required]
            [Display(Name = "NIF")]
            public string NIF { get; set; }

            [Required]
            [Display(Name = "Nombre")]
            public string Nombre { get; set; }

            [Required]
            [Display(Name = "Apellido")]
            public string Apellido { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }


        }
    }
}