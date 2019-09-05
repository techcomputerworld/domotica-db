using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_db.Data;
using Domotica_db.Data.CustomIdentity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistem_Ventas.Library;

namespace Domotica_db.Library
{
    public class ListObject
    {
        /* Digamos que todo esto son objetos que estamos inicializando siempre por eso los ponemos como campos */
        //campo que almacena usersRole
        public LUsuarios _usuarios;
        public UserRoles _usersRole;
        public ApplicationDbContext _context;
        public UploadImage _image;
        public List<SelectListItem> _userRoles;
        public List<SelectListItem> _userList;
        //ApplicationRole y ApplicationUser es para usar el Identity modificado por nosotros.
        public RoleManager<ApplicationRole> _roleManager;
        public UserManager<ApplicationUser> _userManager;
        public SignInManager<ApplicationUser> _signInManager;
        public IHostingEnvironment _environment;

    }
}
