using Domotica_db.Data.CustomIdentity;
using Domotica_db.Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Domotica_db.Library
{
    public class UserRoles : ListObject
    {
        //private SelectListItem _userRoles;

        public UserRoles()
        {
            _userRoles = new List<SelectListItem>();
        }
        public async Task<List<SelectListItem>> GetRole(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, int ID)
        {
            //aqui obtenemos el usuario con el ID
            string Id = Convert.ToString(ID);
            var users = await userManager.FindByIdAsync((Id));
            _userRoles.Clear();
            //aquí obtenemos los roles de los usuarios de users
            var roles = await userManager.GetRolesAsync(users);
            if (roles.Count.Equals(0))
            {
                _userRoles.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "No Role"
                });
            }
            else
            {
                var roleUser = roleManager.Roles.Where(m => m.Name.Equals(roles[0]));
                foreach (var Data in roleUser)
                {
                    _userRoles.Add(new SelectListItem
                    {
                        Valor = Data.Id,
                        Text = Data.Name
                    });
                }
            }

            return _userRoles;
            /*
            no se puede convertir implicitamente el tipo 
            System.Collection.Generic.List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>' en 
            Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            */
        }
        // este método me sirve perfectamente para obtener los roles desde la base de datos
        public List<SelectListItem> getRoles(RoleManager<ApplicationRole> roleManager)
        {
            //Roles representa a la tabla Roles de la base de datos
            var roles = roleManager.Roles.ToList();
            _userRoles.Clear();
            
            roles.ForEach(item =>
            {

                _userRoles.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Valor = item.Id,
                    Text = item.Name
                });

            });

            return _userRoles;
        }
    }
}
