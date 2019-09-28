using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sales.Backend.Models;
using Sales.Common.Models;
using Sales.Backend.Helpers;

namespace Sales.Backend.Controllers
{
    public class ProductsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/ImgProducts";

                if (productView.ImageFile != null)
                {
                    pic = FileHelper.UploadPhoto(productView.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var product = ToProducts(productView, pic);
                
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productView);
        }

        private Product ToProducts(ProductView productView,string pic)
        {
            return new Product
            {
                ProductID = productView.ProductID,
                Description = productView.Description,
                Notes = productView.Notes,
                ImagePath = pic,
                Price = productView.Price,
                IsAvailable = productView.IsAvailable,
                PublishOn = productView.PublishOn,
            };
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var productView = ToProductsView(product);

            return View(productView);
        }

        private ProductView ToProductsView(Product product)
        {

            return new ProductView {
                ProductID = product.ProductID,
                Description = product.Description,
                Notes = product.Notes,
                ImagePath = product.ImagePath,
                Price = product.Price,
                IsAvailable = product.IsAvailable,
                PublishOn = product.PublishOn,
            };
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var pic = productView.ImagePath;
                var folder = "~/Content/ImgProducts";

                if (productView.ImageFile != null)
                {
                    pic = FileHelper.UploadPhoto(productView.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var product = ToProducts(productView, pic);
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productView);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
