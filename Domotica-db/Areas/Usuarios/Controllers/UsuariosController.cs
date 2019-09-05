using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_db.Areas.Usuarios.Models;
using Domotica_db.Data;
using Domotica_db.Data.CustomIdentity;
using Domotica_db.Library;
using Domotica_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sistem_Ventas.Library;
using System.IO;
namespace Domotica_db.Areas.Usuarios.Controllers
{
    //[Authorize]
    [Area("Usuarios")]
    public class UsuariosController : Controller
    {
        private ListObject objeto = new ListObject();
        
        public UsuariosController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            objeto._signInManager = signInManager;
            objeto._usuarios = new LUsuarios(userManager, signInManager, roleManager, context);
        }
        // GET: Usuarios
        public async  Task<ActionResult> Index(int id, String Search)
        {
            if (objeto._signInManager.IsSignedIn(User))
            {
                
                var url = Request.Scheme + "://" + Request.Host.Value;
                //esta forma de realizar digamos la búsqueda no me ha convencido nada
                var objects = new Paginador<InputModelRegistrar>().paginador(await objeto._usuarios.GetUsuariosAsync(Search),
                    id, "Usuarios", "Usuarios", "Index", url);
                var models = new DataPaginador<InputModelRegistrar>
                {
                    //posicion 2 tiene el query que contiene la coleccion de datos de todas las paginas que tenemos.
                    List = (List<InputModelRegistrar>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new InputModelRegistrar()

                };
                   
                return View(models);
            }
            else
            {
                //aqui tal cual paso la vista más adelante lo mismo paso un modelo que quiero cargar o algo
                return View("Index2");
            }
          
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}