﻿namespace API.Controllers
{
    using Backend.Context;
    using Backend.Model;
    using Response;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

 
    [Authorize]
    public class CategoriesController : ApiController
    {
        private ContextBackend db = new ContextBackend();

        // GET: api/Categories
        public async Task<IHttpActionResult> GetCategories()
        {
            var categories = await db.Categories.ToListAsync();

            var categoriesResponse = new List<CategoryResponse>();

            foreach (var category in categories.OrderBy(c => c.Description))
            {
                var productResponse = new List<ProductResponse>();

                foreach (var product in category.Products.OrderBy(p => p.Description))
                {
                    productResponse.Add(new ProductResponse
                    {
                        Description = product.Description,
                        Image = product.Image,
                        IsActive = product.IsActive,
                        LastPurchase = product.LastPurchase,
                        Price = product.Price,
                        ProductId = product.ProductId,
                        Remarks = product.Remarks,
                        Sctock = product.Sctock,
                    });
                }
                categoriesResponse.Add(new CategoryResponse
                {
                    CategoryId = category.CategoryId,
                    Description = category.Description,
                    Products = productResponse,
                });
            }
            return Ok(categoriesResponse);
        }


        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            var category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null 
                    && ex.InnerException.InnerException != null
                    && ex.InnerException.InnerException.Message.Contains("Index"))
                {
                    return BadRequest("There are a record with the same description.");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
            

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> DeleteCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }
    }
}