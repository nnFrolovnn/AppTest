using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebApp.Models;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;

namespace WebApp.Controllers.api
{
    [Authorize]
    [Route("api/{controller}/{action}")]
    public class ItemController : ApiController
    {
        [HttpGet]
        public JsonResult<List<Item>> Index()
        {
            using (var db = new AppDbContext())
            {
                var items = db.Items
                              .Include(x => x.Categories)
                              .Include(x => x.Categories.Select(y => y.Category))
                              .ToList();

                return Json(items);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]ItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppDbContext())
                {
                    //if it is not exists
                    if (!db.Items.Any(x => x.Name == itemViewModel.Name))
                    {
                        Item item = new Item()
                        {
                            Name = itemViewModel.Name
                        };

                        db.Items.Add(item);
                        await db.SaveChangesAsync();

                        foreach (var id in itemViewModel.Categories)
                        {
                            item.Categories.Add(new ItemCategory()
                            {
                                ItemId = item.ItemId,
                                CategoryId = id
                            });
                        }

                        await db.SaveChangesAsync();

                        return Ok();
                    }
                    else
                    {
                        ModelState.AddModelError("", "item with this name is already exists");
                    }
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddCategory([FromBody]Category category)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppDbContext())
                {
                    if (db.Categories.Any(x => x.Name == category.Name))
                    {
                        ModelState.AddModelError("", "category already exists");
                    }
                    else
                    {
                        db.Categories.Add(category);
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                }
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        public JsonResult<List<Category>> Categories()
        {
            using (var db = new AppDbContext())
            {
                var categories = db.Categories.ToList();

                return Json(categories);
            }
        }
    }
}
