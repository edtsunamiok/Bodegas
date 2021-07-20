using Bodegas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bodegas.Controllers
{
    public class ProductoController : Controller
    {
        public AppDb Db { get; }

        public ProductoController(AppDb db)
        {
            Db = db;
        }

        // GET api/blog
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await Db.Connection.OpenAsync();
            var query = new ProdutoQuery(Db);
            var result = await query.LatestPostsAsync();
            return View(result);
           // return new OkObjectResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> EditarProducto(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ProdutoQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return View(result);
            //return new OkObjectResult(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProducto(int id, Producto collection)
        {
            await Db.Connection.OpenAsync();
            var query = new ProdutoQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.Producto_ = collection.Producto_;
            await result.UpdateAsync();
            return RedirectToAction(nameof(Index));
           // return new OkObjectResult(result);
        }


        [HttpGet]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ProdutoQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return View(result);
            //return new OkObjectResult(result);
        }

        [HttpPost, ActionName("EliminarProducto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmacion(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ProdutoQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return RedirectToAction(nameof(Index));
        }



       

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto collection)
        {
            try
            {
                await Db.Connection.OpenAsync();
                collection.Db = Db;
               
                await collection.InsertAsync();
                return RedirectToAction(nameof(Index));
                // return new OkObjectResult(collection);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        

       


        //// GET: HomeController1/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: HomeController1/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
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
