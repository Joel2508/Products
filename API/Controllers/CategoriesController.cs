﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Backend.Context;
using Backend.Model;
using System.Collections.Generic;
using API.Response;
using System;

namespace API.Controllers
{    
    [Authorize]
    public class CategoriesController : ApiController
    {
        ContextBackend db;

        public CategoriesController()
        {
            db = new ContextBackend();
        }
        // GET: api/Categories
        public async Task<IHttpActionResult> GetCategories()
        {
            var categories = await db.Categories.ToListAsync();
            var categoriesResponse = new List<CategoryResponse>();

            foreach (var category in categories)
            {
                var productResponse = new List<ProductResponse>();

                foreach (var item in category.Products)
                {
                    productResponse.Add(new ProductResponse
                    {
                        Description = item.Description,
                        Image = item.Image,
                        IsActive = item.IsActive,
                        LastPurchase = item.LastPurchase,
                        Price = item.Price,
                        ProductId = item.ProductId,
                        Remarks = item.Remarks,
                        Sctock = item.Sctock,
                    });

                    categoriesResponse.Add(new CategoryResponse
                    {
                        CategoryId = item.CategoryId,
                        Description = item.Description,
                        Products = productResponse,
                    });
                }
            }
            return Ok(categoriesResponse.OrderBy(c => c.Description));
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
            await db.SaveChangesAsync();

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