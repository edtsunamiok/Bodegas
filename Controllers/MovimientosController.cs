using Bodegas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
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

            var r = result.First();
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

            //result.id_movimiento = collection.id_movimiento;
            //result.id_TipoMovimiento = collection.id_TipoMovimiento;

               await result.Update_m_movimientoAsync();      
                 

                return RedirectToAction(nameof(Index));
          
           
        }





        

        // GET: MovimientosController/Create
        public async Task<IActionResult> Create()
        {
            await Db.Connection.OpenAsync();
            var query = new MovimientosQuery(Db);
            var result = await query.allProductosExistenciasAsync();
            ViewBag.Producto = new SelectList(result, "id_producto","Codigo");

            var query2 = new MovimientosQuery(Db);
            var result2 = await query2.allTipoMovimientoAsync();
            ViewBag.Tipo = new SelectList(result2, "id_TipoMovimiento", "NombreMovimiento");

            return View();
        }

        // POST: MovimientosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Tipo, string Producto, MovimientosModel collection)
        {
            try
            {
                await Db.Connection.OpenAsync();
                collection.Db = Db;

                collection.id_producto = int.Parse(Producto.ToString());
                collection.id_TipoMovimiento = int.Parse(Tipo.ToString());
                await collection.InsertAsync();
                return RedirectToAction(nameof(Index));
                // return new OkObjectResult(collection);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
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
