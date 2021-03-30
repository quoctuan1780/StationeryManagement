using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(Constant.AREA_ADMIN)]
    public class WarehouseController : Controller
    {
        // GET: WarehouseController
        public ActionResult Index()
        {
            return View();
        }

        // GET: WarehouseController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult PartialCreateReceipt()
        {
            return View();
        }

        // GET: WarehouseController/Create
        public ActionResult CreateReceipt()
        {
            return View();
        }

        // POST: WarehouseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReceipt(IFormCollection collection)
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

        // GET: WarehouseController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WarehouseController/Edit/5
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

        // GET: WarehouseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WarehouseController/Delete/5
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
