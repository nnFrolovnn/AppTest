﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            using (var db = new AppDbContext())
            {
                var items = db.Items
                              .Include(x => x.Categories)
                              .Include(x => x.Categories.Select(y => y.Category))
                              .ToList();

                return View(items);
            }
        }

        public ActionResult Create()
        {
            bool result = LoadViewBagInfoForCreateItem();
            if (!result)
            {
                return RedirectToAction("AddCategory");
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

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "item with this name is already exists");
                    }
                }
            }

            LoadViewBagInfoForCreateItem();
            return View(itemViewModel);
        }


        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory(Category category)
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
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(category);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>have DB categories or not</returns>
        public bool LoadViewBagInfoForCreateItem()
        {
            ViewBag.url = HttpContext.Request.Url;
            using (var db = new AppDbContext())
            {
                if (db.Categories.FirstOrDefault() != null)
                {
                    ViewBag.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "Name");
                    ViewBag.CountCategories = db.Categories.Count();
                    return true;
                }
                return false;
            }
        }


    }
}