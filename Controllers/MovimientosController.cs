using Bodegas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bodegas.Controllers
{
    public class MovimientosController : Controller
    {
        public AppDb Db { get; }

        public MovimientosController(AppDb db)
        {
            Db = db;
        }

        // GET api/blog
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await Db.Connection.OpenAsync();
            var query = new MovimientosQuery(Db);
            var result = await query.allMovimientosAsync();
            return View(result);
            // return new OkObjectResult(result);
        }


        [HttpGet]
        public async Task<IActionResult> DetalleMovimiento(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new MovimientosQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();

            var r = result.First();// OrderBy(x => x.id_movimiento).Skip(1).Take(1).ToList();
            ViewData["fecha"] = r.fecha.ToShortDateString();
            ViewData["NumeroPedido"] = r.NumeroPedido.ToString();
            ViewData["Concepto"] = r.Concepto.ToString();
            ViewData["Estatus"] = r.Estatus.ToString();
            ViewData["id_movimiento"] = r.id_movimiento.ToString();
            return View(result);
            //return new OkObjectResult(result);
        }


        [HttpPost, ActionName("DetalleMovimiento")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarMovimento (int id, MovimientosModel collection)
        {
            await Db.Connection.OpenAsync();
            collection.Db = Db;
            var query = new MovimientosQuery(Db);
            var result = await query.FindMovOneAsync(id);

            if (result is null)
                return new NotFoundResult();

            result.id_movimiento = collection.id_movimiento;
            result.id_TipoMovimiento = collection.id_TipoMovimiento;

            if (result.id_TipoMovimiento != 3)
            {
                
               
                //await result.UpdatekardexAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {

                ViewBag.mensaje = "Movimiento ya fue cancelado!!...";
                return View();
                
            }
           
        }





        // GET: MovimientosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MovimientosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MovimientosController/Create
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

        // GET: MovimientosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MovimientosController/Edit/5
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

        // GET: MovimientosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MovimientosController/Delete/5
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
