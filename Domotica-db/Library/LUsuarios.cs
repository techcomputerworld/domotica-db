using Domotica_db.Areas.Identity.Pages.Account;
using Domotica_db.Data.CustomIdentity;
using Domotica_db.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_db.Data.CustomIdentity;
using Microsoft.AspNetCore.Identity;
using Sistem_Ventas.Library;
using Domotica_db.Areas.Usuarios.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Domotica_db.Data;

namespace Domotica_db.Library
{
    public class LUsuarios : ListObject
    {
        public LUsuarios()
        {

        }
        public LUsuarios(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
            _usersRole = new UserRoles();
        }
        public LUsuarios(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _usersRole = new UserRoles();
        }
        //este método  es para poder verlo en una página razor por tanto me hace falta trabajar con los model
        
        public async Task<List<InputModelRegistrar>> GetUsuariosAsync(String valor)
        {
            int id = 0;
            List<ApplicationUser> Users;
            Users = _context.Users.ToList();
            string imagen = "default.png";
            //lista de la tabla AspNetUsers
            List<ApplicationUser> ListUsers;
            IEnumerable<InputModelRegistrar> imageUsers;
            string rootFolder = Directory.GetCurrentDirectory();
            string imageDefault = "default.png"; 

            //lista de la tabla Usuarios
            //List<Usuarios> ListUsuarios;
          
            //ListUsuarios = _context.Usuarios.ToList();
            if (valor != null)
            {
                //Listar los usuarios de la tabla AspNetUsers 
                ListUsers = Users.Where(u => u.Email.StartsWith(valor) || u.UserName.StartsWith(valor) ||
                    u.PhoneNumber.StartsWith(valor)).ToList();
                //relacionar el email con el Id
                var user = ListUsers.First();
                //var usuario = ListUsuarios.Where(u => u.ApplicationUserId == user.Id);
                //parts.Find(x => x.PartName.Contains("seat")));
                //coger el ID del usuario para añadir los datos de ese usuario en donde corresponde

            }
            else
            {
                ListUsers = _context.Users.ToList();
                //ListUsuarios = _context.Usuarios.ToList();
            }
            //viendo como lo voy a hacer la programación

            //lista de usuarios además utilizando datos de la otra tabla 
            //userList es la tabla de usuarios Users
            List<InputModelRegistrar> userList = new List<InputModelRegistrar>();
            //List<InputModelRegistrar> listUser = new List<InputModelRegistrar>();

            /*
            var users =
                from ListUsers0 in ListUsers
                join ListUsuarios0 in ListUsuarios on ListUsers0.Id equals ListUsuarios0.ApplicationUserId
                select ListUsuarios0;*/
            //Usuarios de AspnetUsers
            /*
            var usersA =
                from ListUsers0 in ListUsers
                join ListUsuarios0 in ListUsuarios on ListUsers0.Id equals ListUsuarios0.ApplicationUserId
                select ListUsers0;
            */
            /*var users0 =
                //AU es AspnetUsers
                from AU in ListUsers
                from Usuarios in ListUsuarios
                where Usuarios.ApplicationUserId != AU.Id
                select AU;
            */
            //Usuarios que no esta relacionado con la tabla Usuarios
            
            foreach (var item in ListUsers)
            {
                var role = _usersRole.GetRole(_userManager, _roleManager, item.Id);
                if (item.Imagen != null)
                {
                    item.Imagen = Path.Combine(rootFolder + "wwwroot\\images\\fotos\\" + item.Imagen);
                    userList.Add(new InputModelRegistrar
                    {
                        Email = item.Email,
                        Imagen =  item.Imagen,
                        Role = role.Result[0].Text,
                    });
                }
                else
                {
                    userList.Add(new InputModelRegistrar
                    {

                        Email = item.Email,
                        Imagen = Path.Combine(rootFolder + "wwwroot\\images\\fotos\\" + imageDefault),
                        Role = role.Result[0].Text,
                        
                    });
                }
                


            }
            

            /*
            foreach (var item in users)
            {
                var role = _usersRole.GetRole(_userManager, _roleManager, item.ID);
                var email = ListUsers.Where(u => u.Id == item.ApplicationUserId);
               
                //IEnumerable<InputModelRegistrar> Users0 = userList.Where(user => user.Id == ApplicationUserId);
                //userList.Where(user => user.Id == applicationUserId);
                //como relleno los datos de userList
                userList.Add(new InputModelRegistrar
                {

                    Email = email.ElementAt(0).Email,
                    Imagen = item.Imagen,
                    Role = item.Role,
                });
            }
            */
            /*
            foreach (var item in userLista)
            {
                if (item.Imagen == null)
                {
                    item.Imagen = "default.png";
                }
            }
            */
            

            
            //aqui es donde vamos a realizar la comprobación por Id de usuario
            
         
            return userList;
        }
        //método que tal vez no utilice el de CreateUserAsync
        /*
        public async Task<bool> CreateUserAsync(int id, string nombre, string apellido, string dni)
        {
            List<ApplicationUser> ListUsers;
            //lista de la tabla Usuarios
            List<Usuarios> ListUsuarios;
            ListUsers = _context.Users.ToList();
            ListUsuarios = _context.Usuarios.ToList();
            bool valor = false;
            int save = -1;
            //var user = ListUsers.Where(users => users.Id == valor);
            try
            {
                _context.Usuarios.Add(new Usuarios
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    DNI = dni,
                    ApplicationUserId = id
                });

                save = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
            }
            valor = save >= 0 ? valor = true : false;
            
            return valor;
            
            
            

        }
        */


    }
}
