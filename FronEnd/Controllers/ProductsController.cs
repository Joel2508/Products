namespace FronEnd.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using Backend.Model;
    using Models;
    using ModelView;
    using System.IO;
    using Herpels;
    using System;

    [Authorize(Users = "joelarias2508@gmail.com")]
    public class ProductsController : Controller
    {
        ContextFronEnd db;

        public ProductsController()
        {
            db = new ContextFronEnd();
        }
        // GET: Products
        public async Task<ActionResult> Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(await products.ToListAsync());
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
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images";
                
                if(view.ImageFile != null)
                {
                    pic = FileHerpels.FileUpload(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                var productoView = ToProductView(view);
                productoView.Image = pic;
                db.Products.Add(productoView);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", view.CategoryId);
            return View(view);
        }

        private Product ToProductView(ProductView view)
        {
            return new Product
            {
                Category=view.Category,
                CategoryId = view.CategoryId,
                Description = view.Description,
                Image = view.Image,
                IsActive = view.IsActive,
                LastPurchase = view.LastPurchase,
                Price = view.Price,
                ProductId = view.ProductId,
                Remarks = view.Remarks,
                Sctock = view.Sctock,
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
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", product.CategoryId);
            var view = ToViewProduct(product);
            return View(view);
        }

        private ProductView ToViewProduct(Product product)
        {
            return new ProductView
            {
                Category=product.Category,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Image = product.Image,
                IsActive = product.IsActive,
                LastPurchase = product.LastPurchase,
                Price = product.Price,
                ProductId = product.ProductId,
                Remarks = product.Remarks,
                Sctock = product.Sctock,
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FileHerpels.FileUpload(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                var viewProduct = ToProductView(view);
                viewProduct.Image = pic;
                db.Entry(viewProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", view.CategoryId);
            return View(view);
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
