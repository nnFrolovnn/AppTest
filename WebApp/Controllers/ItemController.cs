using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.url = HttpContext.Request.Url;
            using (var db = new AppDbContext())
            {
                if (db.Categories.FirstOrDefault() != null)
                {
                    ViewBag.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "Name");
                    ViewBag.CountCategories = db.Categories.Count();
                }
                else
                {
                    return RedirectToAction("AddCategory");
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string param)
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();

            var itemViewModel = JsonConvert.DeserializeObject<ItemViewModel>(json);
            ValidateModel(itemViewModel);
            if (ModelState.IsValid)
            {
                using (var db = new AppDbContext())
                {
                    Item item = new Item()
                    {
                        Name = itemViewModel.Name
                    };

                    db.Items.Add(item);

                    foreach (var id in itemViewModel.Categories)
                    {
                        item.Categories.Add(new ItemCategory()
                        {
                            ItemId = item.ItemId,
                            CategoryId = id
                        });
                    }

                    await db.SaveChangesAsync();
                }
            }

            return View(itemViewModel);
        }


        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory(Category category)
        {
            if(ModelState.IsValid)
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
                    }
                }
            }

            return View(category);
        }
    }
}