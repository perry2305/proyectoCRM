using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoCRM.Models;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ProyectoCRM.Controllers
{
    public class ContactosController : Controller
    {

       IContactoRepository repo;
       
       private string fotosDir;

        public ContactosController(IContactoRepository contactosRepo, 
        IConfiguration config)
        {
            repo = contactosRepo;
            fotosDir = config.GetValue<String>("ContactosRepository:imgfolder");
        }
    // GET: Contactos
        public ActionResult Index()
        {
            var model = repo.Leer();
            return View(model);

        }

        // GET: Contactos/Details/5
        public ActionResult Details(int id)
        {
            var model = repo.LeerPorId(id);

            if(model == null)
            {
                return NotFound();
            }

            var foto = ($"{Request.Scheme}://{Request.Host}/{fotosDir}/{model.ID}.jpg"); 
          
            model.Foto = foto;

            return View(model);
        }

        // GET: Contactos/Create
        public ActionResult Create()
        {
            var model = new Contacto();
            return View(model);
        }

        // POST: Contactos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contacto model)
        {
            try
            {
                repo.Crear(model);
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contactos/Edit/5
        public ActionResult Edit(int id)
        {
            var model = repo.LeerPorId(id);

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Contactos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contacto model)
        {
            var contacto = repo.LeerPorId((int)model.ID);

            if(contacto== null)
            {
                return NotFound();
            }
            
            try
            {
                contacto.Nombre = model.Nombre;
                contacto.Colonia = model.Colonia;
                contacto.Numero = model.Numero;
                contacto.Email = model.Email;
                contacto.Tipo = model.Tipo;
                contacto.Ciudad = model.Ciudad;
                // TODO: Add update logic here

                repo.Editar(contacto);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contactos/Delete/5
        public ActionResult Delete(int id)
        {
            var model = repo.LeerPorId(id);

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Contactos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                repo.Borrar(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

         [HttpPost]
        // POST: 
        public IActionResult SubirFoto(int id, IFormFile foto){
            
            var contacto = repo.LeerPorId(id);

            if (contacto == null){
                return NotFound();
            }

            if(foto == null){
                return NotFound();
            }

            var path = Path.Combine(
                Directory.GetCurrentDirectory()
                , "wwwroot"
                ,fotosDir
                ,contacto.ID.ToString()
                +".jpg"
            );
            

            using(var stream = new FileStream(path,FileMode.Create)){
                foto.CopyTo(stream);
            }

            return RedirectToAction("Details", new {id = contacto.ID});
        }

    }
}