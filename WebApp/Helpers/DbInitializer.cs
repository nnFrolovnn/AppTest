using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class DbInitializer : CreateDatabaseIfNotExists<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var clientRole = new IdentityRole { Name = "client" };
            var category = new Category() { Name = "item" };

            context.Categories.Add(category);
            context.SaveChanges();

            var result = roleManager.Create(clientRole);
            if (!result.Succeeded)
            {
                throw new Exception("Проблема с созданием роли");
            }

            base.Seed(context);
        }
    }
}