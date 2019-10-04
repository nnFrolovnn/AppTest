using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApp.Models;

namespace WebApp.Helpers
{
    public static class ConverterVM
    {
        public static User ToUser(RegisterViewModel registerViewModel)
        {
            return new User()
            {
                UserName = registerViewModel.Email,
                NickName = registerViewModel.NickName,
                Color = registerViewModel.Color,
                Email = registerViewModel.Email,

            };
        }

    }
}