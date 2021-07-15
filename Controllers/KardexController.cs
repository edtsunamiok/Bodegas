using Bodegas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bodegas.Controllers
{
    public class KardexController : Controller
    {
        public AppDb Db { get; }

        public KardexController(AppDb db)
        {
            Db = db;
        }

        // GET api/blog
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await Db.Connection.OpenAsync();
            var query = new KardexQuery(Db);
            var result = await query.allKardexAsync();
            return View(result);
            // return new OkObjectResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> AgregarExistencias(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new KardexQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return View(result);
            //return new OkObjectResult(result);
        }


        [HttpPost, ActionName("AgregarExistencias")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarConfirmacion(int id, Kardex collection)
        {
            await Db.Connection.OpenAsync();
            collection.Db = Db;
            var query = new KardexQuery(Db);
            var result = await query.FindOneAsync(id);

            if (result is null)
                return new NotFoundResult();

            result.id_producto = collection.id_producto;
            result.Cantidad = collection.Cantidad;
            result.Precio = collection.Precio;
            await result.UpdatekardexAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: KardexController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KardexController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KardexController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: KardexController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: KardexController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: KardexController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KardexController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
